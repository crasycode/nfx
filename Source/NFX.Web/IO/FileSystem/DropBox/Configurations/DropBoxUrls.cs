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

namespace NFX.Web.IO.FileSystem.DropBox.Configurations
{
    public sealed class DropBoxUrls
    {
        #region Private Fields

        private const string Get = "https://content.dropboxapi.com/1/files/auto/{0}";
        private const string Put="https://content.dropboxapi.com/1/files_put/auto/{0}?param=val";
        private const string ChunkPut="https://content.dropboxapi.com/1/chunked_upload?param=val";
        private const string ChunkCommit="https://content.dropboxapi.com/1/commit_chunked_upload/auto/{0}";
        private const string Copy= "https://api.dropboxapi.com/1/fileops/copy";
        private const string CreateFolder= "https://api.dropboxapi.com/1/fileops/create_folder";
        private const string Delete= "https://api.dropboxapi.com/1/fileops/delete";
        private const string Move= "https://api.dropboxapi.com/1/fileops/move";
        private const string DeletePermanently= "https://api.dropbox.com/1/fileops/permanently_delete";
		private const string GetMetadata= "https://api.dropboxapi.com/1/metadata/auto/{0}";
        private const string GetMetadataLink= "https://api.dropbox.com/1/metadata/link";
        private const string Delta= "https://api.dropboxapi.com/1/delta";
        private const string GetServerCursor= "https://api.dropboxapi.com/1/delta/latest_cursor";
        private const string WaitOnChanges= "https://notify.dropboxapi.com/1/longpoll_delta";
        private const string GetRevisions= "https://api.dropboxapi.com/1/revisions/auto/{0}";
        private const string Restore= "https://api.dropboxapi.com/1/restore/auto/{0}";
        private const string Search= "https://api.dropboxapi.com/1/search/auto/{0}";
        private const string Shears= "https://api.dropboxapi.com/1/shares/auto/{0}";
        private const string GetMedia= "https://api.dropboxapi.com/1/media/auto/{0}";
        private const string GetCopyRef= "https://api.dropboxapi.com/1/copy_ref/auto/{0}";
        private const string GetThumbnails= "https://content.dropboxapi.com/1/thumbnails/auto/{0}";
        private const string GetPreviews= "https://content.dropboxapi.com/1/previews/auto/{0}";
        private const string SharedFolders= "https://api.dropboxapi.com/1/shared_folders/{0}";
        private const string Save= "https://api.dropboxapi.com/1/save_url/auto/{0}";
        private const string GetSaveUrlStatus= "https://api.dropboxapi.com/1/save_url_job/{0}";
		private const string GetAccount= "https://api.dropboxapi.com/1/account/info";
        private const string DisableToken= "https://api.dropboxapi.com/1/disable_access_token";
        private const string GetCode = "https://www.dropbox.com/1/oauth2/authorize";
        private const string GetToken= "https://api.dropboxapi.com/1/oauth2/token";

        #endregion

        #region Public Properties

        public static string CopyUrl { get { return Copy ; } }

        public static string PutUrl { get { return Put ; } }

        public static string ChunkPutUrl { get { return ChunkPut; } }

        public static string ChunkCommitUrl { get { return ChunkCommit; } }

        public static string CreateFolderUrl { get { return CreateFolder; } }

        public static string DeleteUrl { get { return Delete; } }

        public static string MoveUrl { get { return Move; } }

        public static string DeletePermanentlyUrl { get { return DeletePermanently; } }

        public static string GetUrl { get { return Get; } }

        public static string GetMetadataUrl { get { return GetMetadata; } }

        public static string GetMetadataLinkUrl { get { return GetMetadataLink; } }

        public static string DeltaUrl { get { return Delta; } }

        public static string GetServerCursoUrlr { get { return GetServerCursor; } }

        public static string WaitOnChangesUrl { get { return WaitOnChanges; } }

        public static string GetRevisionsUrl { get { return GetRevisions; } }

        public static string RestoreUrl { get { return Restore; } }

        public static string SearchUrl { get { return Search; } }

        public static string ShearsUrl { get { return Shears; } }

        public static string GetMediaUrl { get { return GetMedia; } }

        public static string GetCopyRefUrl { get { return GetCopyRef; } }

        public static string GetThumbnailsUrl { get { return GetThumbnails; } }

        public static string GetPreviewsUrl { get { return GetPreviews; } }

        public static string SharedFoldersUrl { get { return SharedFolders; } }

        public static string SaveUrl { get { return Save; } }

        public static string GetSaveUrlStatusUrl { get { return GetSaveUrlStatus; } }

        public static string GetAccountUrl { get { return GetAccount; } }

        public static string GetCodeUrl { get { return GetCode; } }

        public static string GetTokenUrl { get { return GetToken; } }

        public static string DisableTokenUrl { get { return DisableToken; } }

        #endregion
    }
}