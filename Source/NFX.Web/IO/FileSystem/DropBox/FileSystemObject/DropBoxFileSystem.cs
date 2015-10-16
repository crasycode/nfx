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
using System.Collections.Generic;
using NFX.Environment;
using NFX.IO.FileSystem;
using NFX.Web.IO.FileSystem.DropBox.BL;
using NFX.Web.IO.FileSystem.DropBox.BO;
using NFX.Web.IO.FileSystem.DropBox.BO.DTO;
using NFX.Web.IO.FileSystem.DropBox.Configurations;

namespace NFX.Web.IO.FileSystem.DropBox.FileSystemObject
{
    public class DropBoxFileSystem : NFX.IO.FileSystem.FileSystem
    {
        #region Fields

        private DropBoxFileSystemSessionConnectParams _authSettings;
        private readonly DropBoxFileStore _fileStore;
        private readonly DropBoxMetadataStore _metadataStore;

        #endregion

        #region Propeties

        public override IFileSystemCapabilities GeneralCapabilities
        {
            get { return DropBoxFileSystemCapabilities.Instance; }
        }

        public override IFileSystemCapabilities InstanceCapabilities
        {
            get { return DropBoxFileSystemCapabilities.Instance; }
        }

        #endregion
        
        #region .ctor

        public DropBoxFileSystem(string name, IConfigSectionNode node = null) : base(name, node)
        {
            _fileStore = new DropBoxFileStore(node);
            _metadataStore = new DropBoxMetadataStore(node);
        }

        #endregion

        #region Implemented

        protected override void DoConfigure(IConfigSectionNode node)
        {
            _authSettings = new DropBoxFileSystemSessionConnectParams(node);
        }

        public override FileSystemSession StartSession(FileSystemSessionConnectParams connectParams = null)
        {
            if(connectParams != null)
                return new DropBoxFileSystemSession(this, null, connectParams);

            return new DropBoxFileSystemSession(this, null, _authSettings);
        }

        protected internal override FileSystemSessionItem DoNavigate(FileSystemSession session, string path)
        {
            if (_metadataStore.IsExist(path, false, 5))
            {
                DropBoxObjectMetadata objectMetadata = _metadataStore.GetObjectMetadata(path, false, 5);
                if(objectMetadata.IsDir)
                    return new FileSystemDirectory((DropBoxFileSystemSession)session
                                                    , DropBoxPathUtils.GetParentPathFromPath(path)
                                                    , DropBoxPathUtils.GetNameFromPath(path)
                                                    ,new DropBoxObjectHandler(objectMetadata));

                return new FileSystemFile((DropBoxFileSystemSession)session
                    , DropBoxPathUtils.GetParentPathFromPath(path)
                    , DropBoxPathUtils.GetNameFromPath(path)
                    ,new DropBoxObjectHandler(objectMetadata));
            }
            return null;
        }

        protected internal override IEnumerable<string> DoGetSubDirectoryNames(FileSystemDirectory directory, bool recursive)
        {
            return _metadataStore.GetObjectNames(DropBoxObjectType.Directory, directory.Path, recursive, 5);
        }

        protected internal override IEnumerable<string> DoGetFileNames(FileSystemDirectory directory, bool recursive)
        {
            return _metadataStore.GetObjectNames(DropBoxObjectType.File, directory.Path, recursive, 5);
        }

        protected internal override void DoDeleteItem(FileSystemSessionItem item)
        {
            _fileStore.Delete(item.Path, 5);
        }

        protected internal override FileSystemFile DoCreateFile(FileSystemDirectory dir, string name, int size)
        {
            DropBoxObjectMetadata dropBoxObjectMetadata = _fileStore.CreateFile(CombinePaths(dir.ParentPath, name), 5);
            return new FileSystemFile(dir.Session, dropBoxObjectMetadata.Path,
                                      DropBoxPathUtils.GetNameFromPath(dropBoxObjectMetadata.Path),
                                      new DropBoxObjectHandler(dropBoxObjectMetadata));
        }

        protected internal override FileSystemFile DoCreateFile(FileSystemDirectory dir, string name, string localFile, bool readOnly)
        {
            DropBoxObjectMetadata file = _fileStore.CreateFile(CombinePaths(dir.ParentPath, name), localFile, 5);
            DropBoxObjectHandler handler = new DropBoxObjectHandler(file);
            return new FileSystemFile(dir.Session, dir.Path, name, handler);
        }
        
        protected internal override FileSystemDirectory DoCreateDirectory(FileSystemDirectory dir, string name)
        {
            DropBoxObjectMetadata file = _fileStore.CreateDirectory(dir.ParentPath, name, 5);
            DropBoxObjectHandler handler = new DropBoxObjectHandler(file);
            return new FileSystemDirectory(dir.Session, dir.Path, name, handler);
        }

        protected internal override ulong DoGetItemSize(FileSystemSessionItem item)
        {
            if (item is FileSystemFile)
                return _metadataStore.GetFileSize(item.Path, 5);

            return _metadataStore.GetFolderSize(item.Path, 5);
        }

        protected internal override DateTime? DoGetCreationTimestamp(FileSystemSessionItem item)
        {
            return _metadataStore.GetItemCreationTimestamp(item.Path, 5);
        }

        protected internal override DateTime? DoGetModificationTimestamp(FileSystemSessionItem item)
        {
            return _metadataStore.GetItemModificationTimestamp(item.Path, 5);
        }

        protected internal override FileSystemStream DoGetFileStream(FileSystemFile file, Action<FileSystemStream> disposeAction)
        {
            return new DropBoxFileStream(file, disposeAction);
        }


        #endregion

        #region Not Implemented

        protected internal override bool DoRenameItem(FileSystemSessionItem item, string newName)
        {
            throw new NotImplementedException();
        }
        
        protected internal override FileSystemStream DoGetPermissionsStream(FileSystemSessionItem item, Action<FileSystemStream> disposeAction)
        {
            throw new NotImplementedException();
        }

        protected internal override FileSystemStream DoGetMetadataStream(FileSystemSessionItem item, Action<FileSystemStream> disposeAction)
        {
            throw new NotImplementedException();
        }

        protected internal override DateTime? DoGetLastAccessTimestamp(FileSystemSessionItem item)
        {
            throw new NotImplementedException();
        }

        protected internal override void DoSetCreationTimestamp(FileSystemSessionItem item, DateTime timestamp)
        {
            throw new NotImplementedException();
        }

        protected internal override void DoSetModificationTimestamp(FileSystemSessionItem item, DateTime timestamp)
        {
            throw new NotImplementedException();
        }

        protected internal override void DoSetLastAccessTimestamp(FileSystemSessionItem item, DateTime timestamp)
        {
            throw new NotImplementedException();
        }

        protected internal override bool DoGetReadOnly(FileSystemSessionItem item)
        {
            return false;
        }

        protected internal override void DoSetReadOnly(FileSystemSessionItem item, bool readOnly)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
