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
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using NFX.Serialization.JSON;

namespace NFX.Web.IO.FileSystem.DropBox.BL
{
    public static class HttpContentExtension
    {
        #region Methods

        public static string HttpContentAsString(this HttpContent content)
        {
            return content.ReadAsStringAsync().Result;
        }

        public static byte[] HttpContentAsByteArray(this HttpContent content)
        {
            return content.ReadAsByteArrayAsync().Result;
        }

        public static Stream HttpContentAsStream(this HttpContent content)
        {
            return content.ReadAsStreamAsync().Result;
        }

        public static MemoryStream ReadToMemory(this HttpContent content)
        {
            if (content == null) return new MemoryStream(new byte[] {});

            using (Stream httpStream = content.ReadAsStreamAsync().Result)
            {
                MemoryStream nemoryStream = new MemoryStream();
                httpStream.CopyTo(nemoryStream);
                return nemoryStream;
            }
        }

        public static JSONDataMap DeserializeToJsonDataMap(this HttpContent content)
        {
            string contentValue = content.ReadAsStringAsync().Result;
            return (JSONDataMap)contentValue.JSONToDataObject();
        }

        #endregion
    }

    public static class FileExtension
    {
        #region Methods

        public static string GetWindowsDiscName(this FileInfo file)
        {
            int position = file.FullName.IndexOf(":", StringComparison.Ordinal);
            if (position < 0)
                return null;

            return file.FullName.Remove(position, file.FullName.Length - 1);
        }

        #endregion
    }

    public static class CollectionExtension
    {
        #region Methods

        public static bool IsNotEmpryOrNull<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) return false;

            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                if (e.MoveNext()) return true;
            }
            return false;
        }

        #endregion
    }
}
