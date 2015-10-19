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
using System.Threading;
using NFX.Serialization.JSON;
using NFX.Web.IO.FileSystem.DropBox.BL;
using NFX.Web.IO.FileSystem.DropBox.BO;

namespace NFX.Web.IO.FileSystem.DropBox.Http
{
    internal static class DropBoxRequestExecuter
    {
        #region Public Methods

        public static JSONDataMap Execute(DropBoxRequest request, int numberOfAttempts, CancellationToken token = new CancellationToken())
        {
            try
            {
                return DropBoxHttpExecuter.ExecuteAsync(request, numberOfAttempts, token);
            }
            catch (Exception ex)
            {
                DropBoxWebExecuteException executeException = ex as DropBoxWebExecuteException;
                if (executeException != null)
                    throw DropBoxExceptionGenerator.Throw(executeException.Code, ex, request);

                throw DropBoxExceptionGenerator.Throw(ex);
            }
        }

        public static JSONDataMap ExecuteUpload(DropBoxRequest request, int numberOfAttempts, CancellationToken token = new CancellationToken())
        {
            try
            {
                return DropBoxHttpUploader.Upload(request, numberOfAttempts, token);
            }
            catch (Exception ex)
            {
                DropBoxWebUploadException uploadError = ex as DropBoxWebUploadException;
                if (uploadError != null)
                    throw DropBoxExceptionGenerator.Throw(uploadError.Code, ex, request);

                throw DropBoxExceptionGenerator.Throw(ex);
            }
        }

        public static JSONDataMap ExecuteChunkUpload(DropBoxRequest request, int numberOfAttempts
                                                     , CancellationToken token = new CancellationToken())
        {
            try
            {
                return DropBoxHttpUploader.ChunkUpload(request, numberOfAttempts, token);
            }
            catch (Exception ex)
            {
                DropBoxWebUploadException uploadError = ex as DropBoxWebUploadException;
                if (uploadError != null)
                    throw DropBoxExceptionGenerator.Throw(uploadError.Code, ex, request);

                throw DropBoxExceptionGenerator.Throw(ex);
            }
        }

        public static DropBoxFile ExecuteDownload(DropBoxRequest request, int numberOfAttempts, CancellationToken token = new CancellationToken())
        {
            try
            {
                MemoryStream content = DropBoxHttpLoader.Load(request, numberOfAttempts, token);
                DropBoxFile dropBoxFile = new DropBoxFile(null, content);
                return dropBoxFile;
            }
            catch (Exception ex)
            {
                DropBoxWebLoadException loadError = ex as DropBoxWebLoadException;
                if (loadError != null)
                    throw DropBoxExceptionGenerator.Throw(loadError.Code, ex, request);

                throw DropBoxExceptionGenerator.Throw(ex);
            }
        }

        #endregion
    }
}