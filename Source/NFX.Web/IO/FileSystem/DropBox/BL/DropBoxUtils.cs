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
using System.Linq;
using System.Text;

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
        #region Private Fields

        private static readonly char[] NotAvailableSimbols = {'*', '|', '\\', ':', '"', '<', '>', '?' };

        #endregion

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

        public static string Combine(params string[] parts)
        {
            if (parts == null || parts.Length == 0)
                return string.Empty;

            StringBuilder pathBuilder = new StringBuilder();

            for (int idx = 0; idx < parts.Length; idx++)
            {
                string part = parts[idx];
                if (part.IsNullOrEmpty())
                    continue;

                if (part.Length > 0 && pathBuilder.Length > 0 && pathBuilder[pathBuilder.Length - 1] != '/')
                    pathBuilder.Append("/");

                for (int i = 0; i < part.Length; i++)
                {
                    char charOfPart = part[i];
                    if (pathBuilder.Length == 0 && charOfPart != '/')
                        pathBuilder.Append(string.Format("{0}{1}", '/', charOfPart));
                    else if (pathBuilder.Length > 0 && charOfPart == '/' &&
                            (pathBuilder[pathBuilder.Length - 1] != '/' && i != part.Length - 1))
                        pathBuilder.Append(charOfPart);
                    else if (pathBuilder.Length > 0 && charOfPart != '/')
                        pathBuilder.Append(charOfPart);
                }
            }

            if (pathBuilder.Length > 0 && pathBuilder[pathBuilder.Length - 1] == '/')
                return pathBuilder.Remove(pathBuilder.Length - 1, 1).ToString();

            return pathBuilder.ToString();
        }

        public static bool IsValid(string path)
        {
            if (!path.IsNullOrEmpty())
            {
                return path.All(ch => !NotAvailableSimbols.Contains(ch));
            }
            return false;
        }

        public static bool IsValid(string[] partsOfPath)
        {
            if (partsOfPath != null && partsOfPath.Length > 0)
            {
                return partsOfPath.All(IsValid);
            }
            return false;
        }

        #endregion
    }
}
