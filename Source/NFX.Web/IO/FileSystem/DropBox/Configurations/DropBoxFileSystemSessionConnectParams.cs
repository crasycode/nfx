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

using NFX.Environment;
using NFX.IO.FileSystem;

namespace NFX.Web.IO.FileSystem.DropBox.Configurations
{
    public class DropBoxFileSystemSessionConnectParams : FileSystemSessionConnectParams
    {
        #region Private Fields

        [Config] private string code;

        [Config] private string client_id;

        [Config] private string client_secret;

        [Config] private string redirect_id;

        [Config] private int timeout_ms;

        [Config] private string access_token;

        [Config] private string access_type;

        [Config] private string access_uid;

        #endregion
        
        #region Public Property

        public string ResponseTypeCode
        {
            get { return code; }
        }

        public string ClientId
        {
            get { return client_id; }
        }

        public string ClientSecret
        {
            get { return client_secret; }
        }

        public string RedirectId
        {
            get { return redirect_id; }
        }

        public int Timeout
        {
            get { return timeout_ms; }
        }

        public string Token
        {
            get { return access_token; }
        }

        public string AccessType
        {
            get { return access_type; }
        }

        public string AccessUid
        {
            get { return access_uid; }
        }

        #endregion
        
        #region .ctor

        public DropBoxFileSystemSessionConnectParams() {}
        public DropBoxFileSystemSessionConnectParams(IConfigSectionNode node) : base(node) {}
        public DropBoxFileSystemSessionConnectParams(string connectStr, string format = Configuration.CONFIG_LACONIC_FORMAT)
                                                     : base(connectStr, format) {}

        #endregion
    }
}