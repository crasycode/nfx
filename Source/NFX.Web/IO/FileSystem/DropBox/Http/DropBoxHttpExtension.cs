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
using System.Net.Http;
using System.Threading;
using NFX.Serialization.JSON;
using NFX.Web.IO.FileSystem.DropBox.BO;

namespace NFX.Web.IO.FileSystem.DropBox.Http
{
    public static class DropBoxHttpExtension
    {
        #region Methods

        public static JSONDataMap RetryExecute(this HttpClient client, DropBoxRequest request
                                               , int numberOfAttempts, CancellationToken token
                                              , Func<HttpClient, DropBoxRequest, int, CancellationToken, JSONDataMap> action)
        {
            numberOfAttempts = numberOfAttempts <= 0 ? DropBoxHttpRequestSettings.DefaultNumberOfAttempts : numberOfAttempts;
            do
            {
                try
                {
                    return action(client, request, numberOfAttempts, token);
                }
                catch
                {
                    --numberOfAttempts;
                    if (numberOfAttempts == 0)
                        throw;
                }
                Thread.Sleep(DropBoxHttpRequestSettings.ThreadWaiteOnNextAttemptTime);

            } while (numberOfAttempts > 0);

            return null;
        }

        public static HttpResponseMessage RetryExecute(this HttpClient client, DropBoxRequest request
                                       , int numberOfAttempts, CancellationToken token
                                      , Func<HttpClient, DropBoxRequest, int, CancellationToken, HttpResponseMessage> action)
        {
            numberOfAttempts = numberOfAttempts <= 0 ? DropBoxHttpRequestSettings.DefaultNumberOfAttempts : numberOfAttempts;
            do
            {
                try
                {
                    return action(client, request, numberOfAttempts, token);
                }
                catch
                {
                    --numberOfAttempts;
                    if (numberOfAttempts == 0)
                        throw;
                }
                Thread.Sleep(DropBoxHttpRequestSettings.ThreadWaiteOnNextAttemptTime);

            } while (numberOfAttempts > 0);

            return null;
        }

        #endregion
    }
}
