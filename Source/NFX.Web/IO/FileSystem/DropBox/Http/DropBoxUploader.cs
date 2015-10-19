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
using System.Net.Http;
using System.Threading;
using NFX.Serialization.JSON;
using NFX.Web.IO.FileSystem.DropBox.BL;
using NFX.Web.IO.FileSystem.DropBox.BO;

namespace NFX.Web.IO.FileSystem.DropBox.Http
{
    internal static class DropBoxHttpUploader
    {
        #region Private Fields

        private static readonly Func<HttpClient, DropBoxRequest, int, CancellationToken, JSONDataMap> ExecuteUploadAction =
            delegate(HttpClient client, DropBoxRequest request, int numberOfAttempts, CancellationToken token)
            {
                HttpRequestMessage message = request.CreateHttpRequestMessage();
                HttpResponseMessage response = client.PutAsync(message.RequestUri,request.StreamContent, token).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.DeserializeToJsonDataMap();
            };

        #endregion

        #region Public Methods

        public static JSONDataMap Upload(DropBoxRequest request, int numberOfAttempts, CancellationToken token)
        {
            numberOfAttempts = numberOfAttempts <= 0 ? DropBoxHttpRequestSettings.DefaultNumberOfAttempts : numberOfAttempts;
            using (HttpClient httpClient = DropBoxHttpFactory.Create(request))
            {
                return httpClient.RetryExecute(request, numberOfAttempts, token, ExecuteUploadAction);
            }
        }

        public static JSONDataMap ChunkUpload(DropBoxRequest request, int numberOfAttempts, CancellationToken token)
        {
            using (HttpClient httpClient = DropBoxHttpFactory.Create(request))
            {
                byte[] buffer = new byte[DropBoxHttpRequestSettings.DataReadFromHDChunkSize];
                JSONDataMap chunkUploadResult = null;

                using (FileStream fileStream = new FileStream(request.ContentSourcePath, FileMode.Open, FileAccess.Read))
                {
                    int numberOfBytes;
                    while ((numberOfBytes = fileStream.Read(buffer, 0, DropBoxHttpRequestSettings.DataReadFromHDChunkSize)) > 0)
                    {
                        request.Content = new MemoryStream();
                        request.Content.Write(buffer, 0, numberOfBytes);

                        if (chunkUploadResult != null)
                            request.ChageParameter("offset", chunkUploadResult["offset"].ToString());

                        chunkUploadResult = SendFile(httpClient, request, numberOfAttempts, token);
                    }
                    return chunkUploadResult;
                }
            }
        }

        #endregion

        #region Private Methods

        private static JSONDataMap SendFile(HttpClient httpClient, DropBoxRequest request
                                            , int numberOfAttempts, CancellationToken token)
        {
            return httpClient.RetryExecute(request, numberOfAttempts, token, ExecuteUploadAction);
        }

        #endregion
    }
}