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


using System.IO;
using System.Net.Http;
using NFX.Environment;
using NFX.Web.IO.FileSystem.DropBox.BO;
using NFX.Web.IO.FileSystem.DropBox.Configurations;

namespace NFX.Web.IO.FileSystem.DropBox.BL
{
    public class DropBoxFileOperationRequestProvider
    {
        #region Private Fields

        private readonly DropBoxFileSystemSessionConnectParams _authenticationOptions;

        #endregion

        #region .ctor

        internal DropBoxFileOperationRequestProvider(IConfigSectionNode node = null)
        {
            _authenticationOptions = new DropBoxAuthSettings(node).Config;
        }

        internal DropBoxFileOperationRequestProvider(DropBoxFileSystemSessionConnectParams sessionParams)
        {
            _authenticationOptions = sessionParams;
        }

        #endregion

        #region Public Methods

        public DropBoxRequest CreateCopyRequest(string root, string from, string to, string locale = "", string fromRef = "")
        {
            DropBoxRequest request = new DropBoxRequest
            {
                RequestUrl = DropBoxUrls.CopyUrl,
                MethodName = new HttpMethod("POST"),
                AuthorizationToken = _authenticationOptions.Token
            };
            request.AddParameter("root", root);
            request.AddParameter("from_path", from);
            request.AddParameter("to_path", to);
            request.AddParameter("locale", locale);
            request.AddParameter("from_copy_ref", fromRef);

            return request;
        }

        public DropBoxRequest CreateDeleteRequest(string root, string path, string locale = "")
        {
            DropBoxRequest request = new DropBoxRequest
            {
                RequestUrl = DropBoxUrls.DeleteUrl,
                MethodName = new HttpMethod("POST"),
                RequestTimeout = _authenticationOptions.Timeout,
                AuthorizationToken = _authenticationOptions.Token
            };
            request.AddParameter("root", root);
            request.AddParameter("path", path);
            request.AddParameter("locale", locale);

            return request;
        }

        public DropBoxRequest CreateFolderRequest(string root, string path, string locale = "")
        {
            DropBoxRequest request = new DropBoxRequest
            {
                RequestUrl = DropBoxUrls.CreateFolderUrl,
                MethodName = new HttpMethod("POST"),
                RequestTimeout = _authenticationOptions.Timeout,
                AuthorizationToken = _authenticationOptions.Token
            };
            request.AddParameter("root", root);
            request.AddParameter("path", path);
            request.AddParameter("locale", locale);

            return request;
        }

        public DropBoxRequest CreateDownloadRequest(string path, string rev = "")
        {
            DropBoxRequest request = new DropBoxRequest
            {
                RequestUrl = DropBoxUrls.GetUrl.AsFormat(path),
                MethodName = new HttpMethod("GET"),
                RequestTimeout = _authenticationOptions.Timeout,
                AuthorizationToken = _authenticationOptions.Token
            };
            request.AddParameter("rev", rev);

            return request;
        }

        public DropBoxRequest CreateUploadRequest(Stream content, string path, string locale = "", string overwrite = ""
                                      , string parentRev = "", string autorename = "")
        {
            DropBoxRequest request = new DropBoxRequest
            {
                RequestUrl = DropBoxUrls.PutUrl.AsFormat(path),
                MethodName = new HttpMethod("PUT"),
                AuthorizationToken = _authenticationOptions.Token,
                RequestTimeout = _authenticationOptions.Timeout,
                Content = content,
            };
            request.AddParameter("locale", locale);
            request.AddParameter("overwrite", overwrite);
            request.AddParameter("parent_rev", parentRev);
            request.AddParameter("autorename", autorename);

            return request;
        }

        public DropBoxRequest CreateChunkUploadRequest(Stream content, string uploadId = "", string offset = "")
        {
            DropBoxRequest request = new DropBoxRequest
            {
                RequestUrl = DropBoxUrls.ChunkPutUrl,
                MethodName = new HttpMethod("PUT"),
                AuthorizationToken = _authenticationOptions.Token,
                Content = content
            };

            request.AddParameter("upload_id", uploadId);
            request.AddParameter("offset", offset);

            return request;
        }

        public DropBoxRequest CreateCommitChunkUploadRequest(string path, string uploadId, string locale = "", string overwrite = "",
                                                             string parentRev = "", string autorename = "")
        {
            DropBoxRequest request = new DropBoxRequest
            {
                RequestUrl = DropBoxUrls.ChunkCommitUrl.AsFormat(path),
                MethodName = new HttpMethod("POST"),
                AuthorizationToken = _authenticationOptions.Token
            };
            request.AddParameter("upload_id", uploadId);
            request.AddParameter("locale", locale);
            request.AddParameter("overwrite", overwrite);
            request.AddParameter("parent_rev", parentRev);
            request.AddParameter("autorename", autorename);

            return request;
        }

        #endregion
    }
}