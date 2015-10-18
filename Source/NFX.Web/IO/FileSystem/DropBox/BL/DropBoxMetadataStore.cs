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
using System.Linq;
using System.Threading;
using NFX.Environment;
using NFX.Serialization.JSON;
using NFX.Web.IO.FileSystem.DropBox.BO;
using NFX.Web.IO.FileSystem.DropBox.BO.DTO;
using NFX.Web.IO.FileSystem.DropBox.Configurations;
using NFX.Web.IO.FileSystem.DropBox.Http;

namespace NFX.Web.IO.FileSystem.DropBox.BL
{
    public class DropBoxMetadataStore
    {
        #region Private Fields

        private readonly DropBoxMetadataRequestProvider _metadataRequestProvider;

        #endregion

        #region .ctor

        internal DropBoxMetadataStore(IConfigSectionNode node = null)
        {
            _metadataRequestProvider = new DropBoxMetadataRequestProvider(node);
        }

        internal DropBoxMetadataStore(DropBoxFileSystemSessionConnectParams sessionParam)
        {
            _metadataRequestProvider = new DropBoxMetadataRequestProvider(sessionParam);
        }

        #endregion

        #region Public Methods

        internal DropBoxObjectMetadata GetObjectMetadata(string path, bool withContent, int numberOfAttempts, 
                                                         CancellationToken token = new CancellationToken())
        {
            return GetMetadata(path, withContent, numberOfAttempts, token);
        }

        internal bool IsExist(string path, int numberOfAttempts,
                                                 CancellationToken token = new CancellationToken())
        {
            try
            {
                GetMetadata(path, false, numberOfAttempts, token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal IEnumerable<string> GetObjectNames(DropBoxObjectType objectType, string path, bool recursive
                                                    , int numberOfAttempts, CancellationToken token = new CancellationToken())
        {
            bool isDirectory = objectType == DropBoxObjectType.Directory;
            List<string> names = new List<string>();
            DropBoxObjectMetadata objects = GetMetadata(path, true, numberOfAttempts, token);

            if (objects.Сhildren != null && objects.Сhildren.Length != 0)
            {
                names.AddRange(objects.Сhildren.Where(f => f.IsDir == isDirectory)
                                               .Select(obj => obj.Name)
                                               .ToList());
                if (recursive)
                    foreach (DropBoxContentObjectMetadata folderMetadata in objects.Сhildren.Where(f => f.IsDir))
                    {
                        names.AddRange(GetObjectNames(objectType, folderMetadata.Path, true, numberOfAttempts, token));
                    }
            }

            return names;
        }

        internal ulong GetFileSize(string path, int numberOfAttempts, CancellationToken token = new CancellationToken())
        {
            DropBoxObjectMetadata metadata = GetMetadata(path, false, numberOfAttempts, token);
            return metadata.Bytes;
        }

        internal ulong GetFolderSize(string path, int numberOfAttempts, CancellationToken token = new CancellationToken())
        {
            DropBoxObjectMetadata objects = GetMetadata(path, true, numberOfAttempts, token);
            ulong commponSize = 0;

            if (objects.Сhildren != null && objects.Сhildren.Length != 0)
            {
                commponSize = objects.Сhildren.Where(obj => !obj.IsDir)
                                             .Aggregate(commponSize, (current, folder) => current + folder.Bytes);
                foreach (DropBoxContentObjectMetadata folderMetadata in objects.Сhildren.Where(f => f.IsDir))
                {
                    commponSize += GetFolderSize(folderMetadata.Path, numberOfAttempts, token);
                }
            }

            return commponSize;
        }

        internal DateTime? GetItemCreationTimestamp(string path, int numberOfAttempts, CancellationToken token = new CancellationToken())
        {
            DropBoxObjectMetadata metadata = GetMetadata(path, false, numberOfAttempts, token);
            DateTime? time = DateTimeUtils.GetUTCDateTimeFromString(metadata.CreatedDate);
            return time;
        }

        internal DateTime? GetItemModificationTimestamp(string path, int numberOfAttempts, CancellationToken token = new CancellationToken())
        {
            DropBoxObjectMetadata metadata = GetMetadata(path, false, numberOfAttempts, token);
            if (metadata.ModifiedDate.IsNullOrEmpty())
                return null;

            return  DateTimeUtils.GetDateTimeFromString(metadata.ModifiedDate);
        }

        #endregion

        #region Private Methods - API

        private DropBoxObjectMetadata GetMetadata(string path, bool withContent, int numberOfAttempts,
                                                       CancellationToken token = default(CancellationToken))
        {
            DropBoxRequest request = _metadataRequestProvider.CreateMetadataRequest(path, list: withContent.ToString());
            JSONDataMap dataMap = RequestExecuter.Execute(request, numberOfAttempts, token);
            return new DropBoxObjectMetadata(dataMap);
        }

        #endregion
    }
}