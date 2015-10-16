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

using System.Linq;
using NFX.Serialization.JSON;
using NFX.Web.IO.FileSystem.DropBox.BL;

namespace NFX.Web.IO.FileSystem.DropBox.BO.DTO
{
    public class DropBoxObjectMetadata : DropBoxObject
    {
        public DropBoxObjectMetadata(JSONDataMap dataMap) : base(dataMap)
        {
            HasMetadata = dataMap != null;
        }

        #region Public Properties

        public bool HasMetadata { get; private set; }

        public string Size { get { return _metadata["size"].ToString(); } }
        
        public ulong Bytes { get { return _metadata["bytes"].AsULong(); } }

        public bool ThumbnailExists { get { return _metadata["thumb_exists"].AsBool(); } }

        public string ModifiedDate { get { return _metadata["modified"].AsString(); } }

        public string CreatedDate { get { return _metadata["client_mtime"].AsString(); } }

        public string Path { get { return _metadata["path"].AsString(); } }

        public string Name { get { return DropBoxPathUtils.GetNameFromPath(Path); } }

        public bool IsDir { get { return _metadata["is_dir"].AsBool(); } }

        public string Icon { get { return _metadata["icon"].AsString(); } }

        public long Revision { get { return _metadata["revision"].AsLong(); } }

        public string Rev { get { return _metadata["rev"].AsString(); } }

        public string Hash { get { return _metadata["hash"].AsString(); } }

        public DropBoxContentObjectMetadata[] Сhildren { get { return GetContents(); } }

        #endregion

        #region Private Methods

        private DropBoxContentObjectMetadata[] GetContents()
        {
            JSONDataArray contents = (JSONDataArray) _metadata["contents"];
            if (contents == null || contents.Count == 0)
                return new DropBoxContentObjectMetadata[] {};

            return contents.Select(c => new DropBoxContentObjectMetadata((JSONDataMap) c))
                           .ToArray();
        }

        #endregion
    }
}
