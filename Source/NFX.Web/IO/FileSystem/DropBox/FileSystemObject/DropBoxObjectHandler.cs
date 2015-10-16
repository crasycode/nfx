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
using NFX.IO.FileSystem;
using NFX.Web.IO.FileSystem.DropBox.BL;
using NFX.Web.IO.FileSystem.DropBox.BO.DTO;

namespace NFX.Web.IO.FileSystem.DropBox.FileSystemObject
{
    public class DropBoxObjectHandler : IFileSystemHandle
    {
        #region Public Property

        public string Path { get; private set; }
        public string Name { get; private set; }
        public bool IsFolder { get; private set; }
        public ulong Size { get; private set; }
        public DateTime? CreatedDate { get; private set; }
        public DateTime? ModifiedDate { get; private set; }
        public bool IsExist { get; private set; }
        #endregion

        #region .ctor

        public DropBoxObjectHandler(DropBoxObjectMetadata fileMetadata)
        {
            if (fileMetadata != null)
            {
                Path = fileMetadata.Path;
                Name = fileMetadata.Name;
                IsFolder = fileMetadata.IsDir;
                Size = fileMetadata.Bytes;
                ModifiedDate = DateTimeUtils.GetDateTimeFromString(fileMetadata.ModifiedDate);
                CreatedDate = DateTimeUtils.GetDateTimeFromString(fileMetadata.CreatedDate);
                IsExist = true;
            }
        }

        #endregion
    }
}