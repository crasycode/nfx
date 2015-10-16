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

using NFX.IO.FileSystem;

namespace NFX.Web.IO.FileSystem.DropBox.FileSystemObject
{
    public class DropBoxFileSystemCapabilities : IFileSystemCapabilities
    {
        private static readonly DropBoxFileSystemCapabilities _instance = new DropBoxFileSystemCapabilities();

        public static DropBoxFileSystemCapabilities Instance { get { return _instance; } }

        #region Public fields

        public bool SupportsVersioning { get { return false;} }
        public bool SupportsTransactions { get { return false;} }
        public int MaxFilePathLength { get { return 1024;} }
        public int MaxFileNameLength { get { return 255;} }
        public int MaxDirectoryNameLength { get { return 255;} }
        public ulong MaxFileSize { get { return 1024*1024*150;} } // 150 Mb for one-chunk upload
        public char[] PathSeparatorCharacters { get { return new []{ '/'};} }
        public bool IsReadonly { get { return false;} }
        public bool SupportsSecurity { get { return false;} }
        public bool SupportsCustomMetadata { get { return false;} }
        public bool SupportsDirectoryRenaming { get { return true;} }
        public bool SupportsFileRenaming { get { return true;} }
        public bool SupportsStreamSeek { get { return false;} }
        public bool SupportsFileModification { get { return false;} }
        public bool SupportsCreationTimestamps { get { return false;} }
        public bool SupportsModificationTimestamps { get { return false;} }
        public bool SupportsLastAccessTimestamps { get { return false;} }
        public bool SupportsReadonlyDirectories { get { return false;} }
        public bool SupportsReadonlyFiles { get { return false;} }
        public bool SupportsCreationUserNames { get { return false;} }
        public bool SupportsModificationUserNames { get { return false;} }
        public bool SupportsLastAccessUserNames { get { return false;} }
        public bool SupportsFileSizes { get { return false;} }
        public bool SupportsDirectorySizes { get { return false;} }
        public bool SupportsAsyncronousAPI { get { return false;} }

        #endregion
    }
}
