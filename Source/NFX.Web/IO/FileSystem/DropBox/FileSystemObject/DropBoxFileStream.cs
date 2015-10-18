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
using System.IO;
using NFX.Glue.Native;
using NFX.IO.FileSystem;
using NFX.Web.IO.FileSystem.DropBox.BL;
using NFX.Web.IO.FileSystem.DropBox.BO;

namespace NFX.Web.IO.FileSystem.DropBox.FileSystemObject
{
    class DropBoxFileStream : FileSystemStream
    {
        #region Private Fields

        private readonly DropBoxFileStore _fileStore;
        private readonly DropBoxMetadataStore _metadataStore;
        private readonly FileSystemSessionItem _item;
        private MemoryStream _stream;
        private bool _HasChanges;

        #endregion

        #region Private Property

        private MemoryStream BufferStream
        {
            get
            {
                if (_stream == null)
                {
                    DownloadFile();
                }
                return _stream;
            }
        }

        #endregion

        private void DownloadFile()
        {
            if (_metadataStore.IsExist(_item.Path, 5))
            {
                using (DropBoxFile file = _fileStore.GetFile(_item.Path, 5))
                {
                    if (file.HasContent)
                    {
                        _stream = new MemoryStream(file.FileContent.ToArray(), 0, (int) file.FileContent.Length, true);
                        _HasChanges = false;
                    }
                    else
                    {
                        _stream = new MemoryStream();
                    }
                }
            }
            else            
                throw new NFXException("File is not exist. Need create file before use.");
        }

        #region .ctor

        public DropBoxFileStream(FileSystemSessionItem item, Action<FileSystemStream> disposeAction)
            : base(item, disposeAction)
        {
            DropBoxFileSystemSession session = item.Session as DropBoxFileSystemSession;
            if(session == null)
                throw new NFXException(StringConsts.FS_STREAM_BAD_TYPE_ERROR + GetType() + ".ctor_DropBoxFileSystemSession");

            _fileStore = new DropBoxFileStore(session.ConnectParameters);
            _metadataStore = new DropBoxMetadataStore(session.ConnectParameters);
            _item = item;
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
            if (_HasChanges)
            {
                _fileStore.CreateFile(_item.ParentPath, _item.Name, BufferStream, 5);
                _HasChanges = false;
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
                _HasChanges = true;
            }
        }

        protected override void DoWrite(byte[] buffer, int offset, int count)
        {
            BufferStream.Write(buffer, offset, count);
            _HasChanges = true;
        }

        #endregion
    }
}