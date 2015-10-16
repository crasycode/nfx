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
using System.Globalization;

namespace NFX.Web.IO.FileSystem.DropBox.BL
{
    public static class DateTimeUtils
    {
        #region Methods

        public static DateTime? GetDateTimeFromString(string dateTimeStr)
        {
            return dateTimeStr == null ? DateTime.MinValue : DateTime.Parse(dateTimeStr);
        }

        public static DateTime? GetUTCDateTimeFromString(string dateTimeStr)
        {
            string str = dateTimeStr;
            if (str == null)
                return null;
            if (str.EndsWith(" +0000")) str = str.Substring(0, str.Length - 6);
            if (!str.EndsWith(" UTC")) str += " UTC";
            return DateTime.ParseExact(str, "ddd, d MMM yyyy HH:mm:ss UTC", CultureInfo.InvariantCulture);
        }

        public static string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString("ddd, d MMM yyyy HH:mm:ss UTC");
        }

        #endregion
    }

    public static class DropBoxPathUtils
    {
        #region Methods

        public static string GetNameFromPath(string path)
        {
            if (path.IsNullOrEmpty()) return String.Empty;

            if (!path.Contains("/")) return path;

            return path.Remove(0, path.LastIndexOf('/') + 1);
        }

        public static string GetParentPathFromPath(string path)
        {
            if (path.IsNullOrEmpty()) return String.Empty;

            if(!path.Contains("/")) return "";

            int lastPosition = path.LastIndexOf('/');
            if(lastPosition == 0) return "/";

            return path.Remove(lastPosition, path.Length - lastPosition);
        }

        public static string CompinePath(params string[] parts)
        {
            if(parts == null || parts.Length == 0)
                return string.Empty;

            string fullPath = string.Join("/", parts).Replace("//", "/");

            if (fullPath.StartsWith("/"))
                return fullPath;
            return fullPath.Insert(0, "/");
        }

        #endregion
    }
}
