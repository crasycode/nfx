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
using NFX.Web.IO.FileSystem.DropBox.Configurations;

namespace NFX.Web.IO.FileSystem.DropBox.FileSystemObject
{
    public class DropBoxFileSystemSession : FileSystemSession
    {
        #region Properties

        public DropBoxFileSystemSessionConnectParams ConnectParameters { get; private set; }
        
        #endregion

        #region .ctor

        protected internal DropBoxFileSystemSession(NFX.IO.FileSystem.FileSystem fs, IFileSystemHandle handle,
                                                    FileSystemSessionConnectParams cParams) : base(fs, handle, cParams)
        {
            if(fs == null)
                throw new ArgumentNullException("fs");

            if (cParams == null)
                throw new ArgumentNullException("cParams");

            DropBoxFileSystemSessionConnectParams pr = cParams as DropBoxFileSystemSessionConnectParams;
            if (pr == null)
                throw new NFXException(StringConsts.FS_SESSION_BAD_PARAMS_ERROR + GetType() + ".ctor_DropBoxFileSystemSession");

            ConnectParameters = pr;
        }

        #endregion
    }
}