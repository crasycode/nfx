/*<FILE_LICENSE>
* NFX (.NET Framework Extension) Unistack Library
* Copyright 2003-2014 IT Adapter Inc / 2015 Aum Code LLC
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
</FILE_LICENSE>*/


/* NFX by ITAdapter
 * Author: Andrey Kolbasov <andrey@kolbasov.com>
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NFX.IO.FileSystem;
using NFX.Web.IO.FileSystem.DropBox.BL;
using NFX.Web.IO.FileSystem.DropBox.BO;

namespace NFX.Web.IO.FileSystem.DropBox.FileSystemObject
{
    class DropBoxFileStream : FileSystemStream
    {
        #region Private Fields

        private readonly DropBoxFileStore _fileStore;
        private readonly FileSystemSessionItem _item;
        private MemoryStream _stream;
        private readonly bool _isNewFile;
        private bool _isChanged;

        #endregion

        #region Private Property

        private MemoryStream BufferStream
        {
            get
            {
                if (!_isNewFile)
                {
                    DropBoxFile file = _fileStore.GetFile(_item.Path, 5);
                    if (file.FileContent.Length > 0)
                    {
                        _stream = new MemoryStream(file.FileContent.ToArray(), 0, (int) file.FileContent.Length, true);
                        _isChanged = false;

                    }
                }
                if (_stream == null)
                {
                    _stream = new MemoryStream();
                    _isChanged = false;
                }

                return _stream;
            }
        }

        #endregion

        #region .ctor

        public DropBoxFileStream(FileSystemSessionItem item, Action<FileSystemStream> disposeAction)
            : base(item, disposeAction)
        {
            DropBoxFileSystemSession session = item.Session as DropBoxFileSystemSession;
            _fileStore = new DropBoxFileStore(session.ConnectParameters);
            var metadataStore = new DropBoxMetadataStore(session.ConnectParameters);
            _item = item;
            IEnumerable<string> names = metadataStore.GetObjectNames(DropBoxObjectType.File, item.ParentPath, false, 5);
            if(names.IsNotEmpryOrNull())
                _isNewFile =  names.FirstOrDefault(name => name == item.Name) == null;
            else
            _isNewFile = true;
        }

        #endregion

        #region Stream Methods

        protected override void Dispose(bool disposing)
        {
            if (_stream != null)
            {
                DoFlush();
                _stream.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override void DoFlush()
        {
            if (_isChanged)
            {
                _fileStore.CreateFile(_item.ParentPath, _item.Name, BufferStream, 5);
                _isChanged = false;
            }
        }

        protected override long DoGetLength()
        {
            return BufferStream.Length;
        }

        protected override long DoGetPosition()
        {
            return BufferStream.Position;
        }

        protected override void DoSetPosition(long position)
        {
            BufferStream.Position = position;
        }

        protected override int DoRead(byte[] buffer, int offset, int count)
        {
            return BufferStream.Read(buffer, offset, count);
        }

        protected override long DoSeek(long offset, SeekOrigin origin)
        {
            return BufferStream.Seek(offset, origin);
        }

        protected override void DoSetLength(long value)
        {
            if (BufferStream.Length != value)
            {
                BufferStream.SetLength(value);
                _isChanged = true;
            }
        }

        protected override void DoWrite(byte[] buffer, int offset, int count)
        {
            BufferStream.Write(buffer, offset, count);
            _isChanged = true;
        }

        #endregion
    }
}