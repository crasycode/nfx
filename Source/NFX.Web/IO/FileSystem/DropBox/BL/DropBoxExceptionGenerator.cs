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
using System.Text;
using NFX.Web.IO.FileSystem.DropBox.BO;

namespace NFX.Web.IO.FileSystem.DropBox.BL
{
    public static class DropBoxExceptionGenerator
    {
        #region Public Static Methods

        public static NFXException Throw(HttpStatusCode code, Exception originalException, DropBoxRequest request)
        {
            StringBuilder additionallyMessage = new StringBuilder();
            additionallyMessage.Append(GetDescriptionByCode(code));
            additionallyMessage.AppendFormat(" Url: {0}; MethodName: {1}."
                                             , request.ReturnAsHttpsRequestMessage().RequestUri
                                             , request.MethodName);
            if (originalException is HttpRequestException)
                originalException = new HttpRequestException(originalException.Message, originalException);

            return new NFXException(additionallyMessage.ToString(), originalException);
        }

        public static NFXException Throw(Exception exception)
        {
            return new NFXException(exception.Message, exception);
        }

        private static string GetDescriptionByCode(HttpStatusCode code)
        {
            if ((int)code == 429)
                return "Too many requests.";

            if ((int)code == 507)
                return "User is over Dropbox storage quota.";

            switch (code)
            {
                case HttpStatusCode.BadRequest:
                    return "One or more parameters were invalid.";
                case HttpStatusCode.Unauthorized:
                    return "Bad or expired token.";
                case HttpStatusCode.Forbidden:
                    return "Bad OAuth request or an invalid copy operation was attempted " +
                           "(e.g. there is already a file at the given destination, or trying to copy a shared folder)";
                case HttpStatusCode.NotFound:
                    return "Unable to find at that path.";
                case HttpStatusCode.MethodNotAllowed:
                    return "Request method not expected (generally should be GET or POST).";
                case HttpStatusCode.ServiceUnavailable:
                    return "Transient server error.";
                case HttpStatusCode.Conflict:
                    return "There was a conflict when creating or processing of the object Dropbox.";
                case HttpStatusCode.UnsupportedMediaType:
                    return "The image is invalid and cannot be converted to a thumbnail.";
                case HttpStatusCode.LengthRequired:
                    return "Missing Content-Length header (this endpoint doesn't support HTTP chunked transfer encoding).";
                case HttpStatusCode.NotModified:
                    return "The folder contents have not changed (relies on hash parameter).";
                case HttpStatusCode.NotAcceptable:
                    return "There are too many file entries to return. The limit is currently 10,000 files and folders";

            }

            return "DropBox server error. Check DropboxOps.";
        }

        #endregion
    }
}
