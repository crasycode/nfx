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
using System.Net;
using System.Net.Http;

namespace NFX.Web.IO.FileSystem.DropBox.BL
{
    public class DropBoxWebLoadException : HttpRequestException
    {
        #region Methods 

        public HttpStatusCode Code { get; private set;}

        public DropBoxWebLoadException(HttpStatusCode statusCode, Exception exception) : base(exception.Message, exception)
        {
            Code = statusCode;
        }

        #endregion
    }
  
    public class DropBoxWebUploadException : HttpRequestException
    {
        #region Methods

        public HttpStatusCode Code { get; private set;}

        public DropBoxWebUploadException(HttpStatusCode statusCode, Exception exception) : base(exception.Message, exception)
        {
            Code = statusCode;
        }

        public DropBoxWebUploadException(Exception exception) : base(exception.Message, exception) { }

        #endregion
    }

    public class DropBoxWebExecuteException : HttpRequestException
    {
        #region Methods
        
        public HttpStatusCode Code { get; private set; }

        public DropBoxWebExecuteException(HttpStatusCode statusCode, Exception exception)
            : base(exception.Message, exception)
        {
            Code = statusCode;
        }

        public DropBoxWebExecuteException(Exception exception) : base(exception.Message, exception) { }

        #endregion
    }
}