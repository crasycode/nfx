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
 * Author: Alexey Miheev <mihadev@yandex.ru>
 */

using System;
using System.IO;
using NFX.IO.FileSystem;

namespace NFX.Web.IO.FileSystem.DropBox.FileSystemObject
{
    public class DropBoxFileSystemStream : FileSystemStream
    {
        public DropBoxFileSystemStream(FileSystemSessionItem item, Action<FileSystemStream> disposeAction) 
                                       : base(item, disposeAction) {}

        protected override void DoFlush()
        {
            throw new NotImplementedException();
        }

        protected override long DoGetLength()
        {
            throw new NotImplementedException();
        }

        protected override long DoGetPosition()
        {
            throw new NotImplementedException();
        }

        protected override void DoSetPosition(long position)
        {
            throw new NotImplementedException();
        }

        protected override int DoRead(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        protected override long DoSeek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        protected override void DoSetLength(long value)
        {
            throw new NotImplementedException();
        }

        protected override void DoWrite(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
