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


using NFX.Serialization.JSON;

namespace NFX.Web.IO.FileSystem.DropBox.BO.DTO
{
    public class DropBoxContentObjectMetadata : DropBoxObjectMetadata
    {
        private readonly JSONDataMap _photoMetadata;
        
        public string ClientMtime { get { return _metadata["client_mtime"].AsString(); } }

        public string MimeType { get { return _metadata["mime_type"].AsString(); } }

        public DropBoxPhotoObjectMetadata PhotosInfo { get { return new DropBoxPhotoObjectMetadata(_photoMetadata); } }

        public DropBoxContentObjectMetadata(JSONDataMap dataMap) : base(dataMap)
        {
            if (dataMap != null)
            {
                _photoMetadata = dataMap["photo_info"] as JSONDataMap;
                _metadata = dataMap;   
            }
        }

    }
}
