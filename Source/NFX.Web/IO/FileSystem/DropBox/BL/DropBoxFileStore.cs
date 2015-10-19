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
using System.Threading;
using NFX.Environment;
using NFX.IO.FileSystem;
using NFX.Serialization.JSON;
using NFX.Web.IO.FileSystem.DropBox.BO;
using NFX.Web.IO.FileSystem.DropBox.BO.DTO;
using NFX.Web.IO.FileSystem.DropBox.Configurations;
using NFX.Web.IO.FileSystem.DropBox.Http;

namespace NFX.Web.IO.FileSystem.DropBox.BL
{
    public sealed class DropBoxFileStore
    {
        #region Private Fields

        private readonly DropBoxFileOperationRequestProvider _operationRequestProvider;

        #endregion

        #region .ctor

        public DropBoxFileStore(IConfigSectionNode node = null)
        {
            _operationRequestProvider = new DropBoxFileOperationRequestProvider(node);
        }

        public DropBoxFileStore(DropBoxFileSystemSessionConnectParams sessionParam)
        {
            _operationRequestProvider = new DropBoxFileOperationRequestProvider(sessionParam);
        }

        #endregion

        #region Public Methods

        public DropBoxFile GetFile(string path, int numberOfAttempts, CancellationToken token = new CancellationToken())
        {
            return Download(path, numberOfAttempts, token);
        }

        public DropBoxObjectMetadata CreateFile(string path, string name, Stream stream
                                                , int numberOfAttempts, CancellationToken token = new CancellationToken())
        {
            return Upload(stream, DropBoxPathUtils.Combine(path, name), numberOfAttempts, token);
        }

        public DropBoxObjectMetadata CreateFile(string path, int numberOfAttempts,
                                            CancellationToken token = new CancellationToken())
        {
            return Upload(null, path, numberOfAttempts, token);
        }

        public DropBoxObjectMetadata CreateFile(string destinationPath, string sourceFilePath
                                                ,int numberOfAttempts, CancellationToken token = new CancellationToken())
        {
            FileInfo fileInfo = new FileInfo(sourceFilePath);
            if(fileInfo.Length > DropBoxRequest.DefaultFileMaxSizeBytes)
                return ChunkUpload(sourceFilePath, destinationPath, numberOfAttempts, token);
            using (Stream stream = fileInfo.OpenRead())
            {
                return Upload(stream, destinationPath, numberOfAttempts, token);
            }
        }

        public DropBoxObjectMetadata CreateDirectory(string path, string name
                                                                 ,int numberOfAttempts, CancellationToken token = new CancellationToken())
        {
            return CreateFolder(DropBoxPathUtils.Combine(path, name), numberOfAttempts, token);
        }

        public DropBoxObjectMetadata Delete(string path, int numberOfAttempts, CancellationToken token = new CancellationToken())
        {
            return DeleteObject(path, numberOfAttempts, token);
        }

        #endregion

        #region Private Methods - API

        private DropBoxObjectMetadata DeleteObject(string path, int numberOfAttempts, CancellationToken token = default(CancellationToken))
        {
            if (path.IsNullOrEmpty())
                throw new ArgumentException("Value cannot be empty or null", "path");

            DropBoxRequest request = _operationRequestProvider.CreateDeleteRequest(DropBoxRootType.DropBox, path);
            JSONDataMap jsonDataMap = DropBoxRequestExecuter.Execute(request, numberOfAttempts, token);
            return new DropBoxObjectMetadata(jsonDataMap);
        }

        private DropBoxObjectMetadata CreateFolder(string path, int numberOfAttempts, CancellationToken token = default(CancellationToken))
        {
            if (path.IsNullOrEmpty())
                throw new ArgumentException("Value cannot be empty or null", "path");

            DropBoxRequest request = _operationRequestProvider.CreateFolderRequest(DropBoxRootType.DropBox, path);
            JSONDataMap jsonDataMap = DropBoxRequestExecuter.Execute(request, numberOfAttempts, token);
            return new DropBoxContentObjectMetadata(jsonDataMap);
        }

        private DropBoxFile Download(string path, int numberOfAttempts, CancellationToken token = default(CancellationToken))
        {
            if (path.IsNullOrEmpty())
                throw new ArgumentException("Value cannot be empty or null", "path");

            DropBoxRequest request = _operationRequestProvider.CreateDownloadRequest(path);
            return DropBoxRequestExecuter.ExecuteDownload(request, numberOfAttempts, token);
        }

        private DropBoxObjectMetadata Upload(Stream content, string path, int numberOfAttempts, CancellationToken token = default(CancellationToken))
        {
            if (path.IsNullOrEmpty())
                throw new ArgumentException("Value cannot be empty or null", "path");

            if (content != null && content.Length > DropBoxRequest.DefaultFileMaxSizeBytes)
                throw new FileSystemException("The file size should not exceed 150 megabytes. Use chunk upload");

            DropBoxRequest request = _operationRequestProvider.CreateUploadRequest(content, path);
            JSONDataMap response = DropBoxRequestExecuter.ExecuteUpload(request, numberOfAttempts, token);
            return new DropBoxContentObjectMetadata(response);
        }

        private DropBoxObjectMetadata ChunkUpload(string sourceFilePath, string destinationPath, int numberOfAttempts
                                                              , CancellationToken token = default(CancellationToken))
        {
            if (sourceFilePath.IsNullOrEmpty())
                throw new ArgumentException("Value cannot be empty or null", "sourceFilePath");

            if (destinationPath.IsNullOrEmpty())
                throw new ArgumentException("Value cannot be empty or null", "destinationPath");

            DropBoxRequest request = _operationRequestProvider.CreateChunkUploadRequest(null);
            request.ContentSourcePath = sourceFilePath;
            JSONDataMap response = DropBoxRequestExecuter.ExecuteChunkUpload(request, numberOfAttempts, token);
            DropBoxChunkFile endChunk = new DropBoxChunkFile(response);
            return CommitChunkUpload(destinationPath, endChunk.UploadId, numberOfAttempts, token);
        }

        private DropBoxContentObjectMetadata CommitChunkUpload(string path, string uploadId, int numberOfAttempts, CancellationToken token = default(CancellationToken))
        {
            if (path.IsNullOrEmpty())
                throw new ArgumentException("Value cannot be empty or null", "path");

            DropBoxRequest request = _operationRequestProvider.CreateCommitChunkUploadRequest(path, uploadId);
            JSONDataMap response = DropBoxRequestExecuter.Execute(request, numberOfAttempts, token);
            return new DropBoxContentObjectMetadata(response);
        }

        #endregion
    }
}