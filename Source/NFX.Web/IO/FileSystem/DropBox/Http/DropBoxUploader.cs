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
using System.Net.Http.Headers;
using System.Threading;
using NFX.Serialization.JSON;
using NFX.Web.IO.FileSystem.DropBox.BL;
using NFX.Web.IO.FileSystem.DropBox.BO;

namespace NFX.Web.IO.FileSystem.DropBox.Http
{
    internal static class DropBoxHttpUploader
    {
        #region Private Fields

        private const int DataReadFromHDChunkSize = 4000; // 4 Kb;
        private const int DataReadMemoryChunkSize = 4194304; // 4 Mb;
        private const int DefaultNumberOfAttempts = 5;
        private const int ThreadWaiteOnNextAttemptTime = 3000; // 3 sec

        #endregion

        #region Public Methods

        public static JSONDataMap Upload(DropBoxRequest request, int numberOfAttempts,
            CancellationToken token)
        {
            numberOfAttempts = numberOfAttempts <= 0 ? DefaultNumberOfAttempts : numberOfAttempts;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                    request.AuthorizationToken);
                httpClient.Timeout = new TimeSpan(0, 0, 0, request.RequestTimeout);
                HttpRequestMessage message = request.ReturnAsHttpsRequestMessage();
                do
                {
                    try
                    {
                        HttpResponseMessage response = httpClient.PutAsync(message.RequestUri, 
                                                       request.StreamContent, token).Result;
                        response.EnsureSuccessStatusCode();
                        return response.Content.DeserializeToJsonDataMap();
                    }
                    catch
                    {
                        if (numberOfAttempts == 0)
                            throw;
                        --numberOfAttempts;
                    }
                    Thread.Sleep(ThreadWaiteOnNextAttemptTime);

                } while (numberOfAttempts > 0);

            }
            return null;
        }

        public static JSONDataMap ChunkUpload(DropBoxRequest request, string sourceFile
                                             , int numberOfAttempts, CancellationToken token)
        {
            JSONDataMap chunkUploadResult = null;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization =
                                                 new AuthenticationHeaderValue("Bearer", request.AuthorizationToken);
                httpClient.Timeout = new TimeSpan(0, 0, 0, request.RequestTimeout);
                MemoryStream stream = ReadFile(sourceFile);
                if (stream.Length == 0)
                    return null;

                    byte[] buffer = new byte[DataReadMemoryChunkSize];
                    int readByteCount;
                    int totalCountBytes = 0;
                    stream.Seek(0, SeekOrigin.Begin);

                    while ((readByteCount = stream.Read(buffer, 0, DataReadMemoryChunkSize)) > 0)
                    {
                        totalCountBytes += readByteCount;
                        request.ChageParameter("offset", totalCountBytes.ToString());
                        if (chunkUploadResult == null)
                        {
                            chunkUploadResult = SendFile(httpClient, buffer, request.ReturnAsHttpsRequestMessage(),
                                                         numberOfAttempts, token);
                        }
                        else
                        {

                            chunkUploadResult = SendFile(httpClient, buffer, request.ReturnAsHttpsRequestMessage(),
                                                         numberOfAttempts, token);
                        }
                    }
            }
            return chunkUploadResult;
        }

        #endregion

        #region Private Methods

        private static JSONDataMap SendFile(HttpClient httpClient, byte[] bytes, HttpRequestMessage message,
                                                      int numberOfAttempts, CancellationToken token)
        {
            if (bytes.Length == 0) return null;
            numberOfAttempts = numberOfAttempts <= 0 ? DefaultNumberOfAttempts : numberOfAttempts;

            do
            {
                try
                {
                    StreamContent content = new StreamContent(new MemoryStream(bytes));
                    var response = httpClient.PutAsync(message.RequestUri, content, token).Result;
                    response.EnsureSuccessStatusCode();
                    return response.Content.DeserializeToJsonDataMap();
                }
                catch
                {
                    if (numberOfAttempts == 0)
                        throw;
                    --numberOfAttempts;
                }
                Thread.Sleep(ThreadWaiteOnNextAttemptTime);

            } while (numberOfAttempts > 0);

            return null;
        }

        private static MemoryStream ReadFile(string sourceFile)
        {
            if(!File.Exists(sourceFile))
                throw new FileNotFoundException("File not found", sourceFile);

            MemoryStream stream = new MemoryStream();
            using (FileStream file = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
            {
                file.Seek(0, SeekOrigin.Begin);

                byte[] buffer = new byte[DataReadFromHDChunkSize];
                int countReadBytes;
                do
                {
                    countReadBytes = file.Read(buffer, 0, buffer.Length);
                    stream.Write(buffer, 0, countReadBytes);
                } while (countReadBytes > 0);
            }
            return stream;
        }   

        #endregion
    }
}