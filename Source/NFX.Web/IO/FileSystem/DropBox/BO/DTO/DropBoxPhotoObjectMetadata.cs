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
using NFX.Serialization.JSON;

namespace NFX.Web.IO.FileSystem.DropBox.BO.DTO
{
    public class DropBoxPhotoObjectMetadata
    {
        private readonly JSONDataMap _dataMap;

        public DropBoxPhotoObjectMetadata(JSONDataMap dataMap)
        {
            if (dataMap == null) throw new ArgumentNullException("dataMap");

            _dataMap = dataMap;
        }

        public double[] LatLong { get { return GetLatLongValues(); } }

        public string TimeTaken { get { return _dataMap["time_taken"].ToString(); } }

        #region Private Methods

        private double[] GetLatLongValues()
        {
            JSONDataArray values = (JSONDataArray) _dataMap["lat_long"];
            return new [] {(double) values[0], (double) values[1]};
        }

        #endregion
    }
}
