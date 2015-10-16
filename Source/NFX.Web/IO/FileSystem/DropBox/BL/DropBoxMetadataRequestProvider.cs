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

using System.Net.Http;
using NFX.Environment;
using NFX.Web.IO.FileSystem.DropBox.BO;
using NFX.Web.IO.FileSystem.DropBox.Configurations;

namespace NFX.Web.IO.FileSystem.DropBox.BL
{
    public class DropBoxMetadataRequestProvider
    {
        #region Private Fields

        private readonly DropBoxFileSystemSessionConnectParams _authenticationOptions;

        #endregion
        
        #region .ctor

        internal DropBoxMetadataRequestProvider(IConfigSectionNode node = null)
        {
            _authenticationOptions = new DropBoxAuthSettings(node).Config;
        }

        internal DropBoxMetadataRequestProvider(DropBoxFileSystemSessionConnectParams sessionParams)
        {
            _authenticationOptions = sessionParams;
        }

        #endregion

        #region MetadataRequestProvider

        public DropBoxRequest CreateMetadataRequest(string path, string fileLimit = "", string hash = "",
            string list = "", string includeDeleted = "", string rev = "", string locale = "",
            string includeMediaInfo = "", string includeMembership = "")
        {
            DropBoxRequest request = new DropBoxRequest
                {
                    RequestUrl = DropBoxUrls.GetMetadataUrl.AsFormat(path),
                    MethodName = new HttpMethod("GET"),
                    RequestTimeout = _authenticationOptions.Timeout,
                    AuthorizationToken = _authenticationOptions.Token
                };
            request.AddParameter("file_limit", fileLimit);
            request.AddParameter("hash", hash);
            request.AddParameter("list", list);
            request.AddParameter("include_deleted", includeDeleted);
            request.AddParameter("rev", rev);
            request.AddParameter("locale", locale);
            request.AddParameter("include_media_info", includeMediaInfo);
            request.AddParameter("include_membership", includeMembership);

            return request;
        }

        #endregion
    }
}