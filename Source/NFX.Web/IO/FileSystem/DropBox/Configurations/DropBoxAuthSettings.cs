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

namespace NFX.Web.IO.FileSystem.DropBox.Configurations
{
    public sealed class DropBoxAuthSettings
    {
        #region Private Fields

        private const string DefaultNameConfiguration = "client_auth_options.lac";
        private readonly DropBoxFileSystemSessionConnectParams _connectionParameters =
                                                                new DropBoxFileSystemSessionConnectParams();

        #endregion

        #region Public Property

        public DropBoxFileSystemSessionConnectParams Config
        {
            get { return _connectionParameters; }
        }
        
        #endregion

        #region .ctor

        public DropBoxAuthSettings() : this(DefaultNameConfiguration) {}

        public DropBoxAuthSettings(string pathFileConfiguration)
        {
            LaconicConfiguration urlConfig =(LaconicConfiguration) Configuration.ProviderLoadFromFile(pathFileConfiguration);
            ConfigAttribute.Apply(_connectionParameters, urlConfig.Root);
        }

        public DropBoxAuthSettings(IConfigSectionNode node)
        {
            ConfigAttribute.Apply(_connectionParameters, node);
        }

        #endregion

    }
}