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
using NFX.IO.FileSystem;
using NFX.Web.IO.FileSystem.DropBox.FileSystemObject;

namespace NFX.NUnit.IO.FileSystem.DropBox
{
    public static class TestDataHelper
    {
        public static string Auth = @"	
                                root
	                            {
		                                ResponseTypeCode = code
			                            client_id        = hr5nccgto8ymnej
			                            client_secret    = 5vju4pmuluyawop
			                            redirect_id      = ''
			                            timeout_ms       = 30000
			                            access_token     = i0ZB-cXHs9AAAAAAAAAAmgtePbh6KJ9KYljrmr_UgQbYoqctifbHcq-CebKNsqJv
			                            access_type      = ''
			                            access_uid       = '' 
	                            }";

        public static DropBoxFileSystem FileSystem = new DropBoxFileSystem("DropBoxFileSystem", Auth.AsLaconicConfig());
        public static DropBoxFileSystemSession SystemSession = FileSystem.StartSession() as DropBoxFileSystemSession;

        public static void InitTestData()
        {
            DeleteAllData();

            FileSystemDirectory folder = new FileSystemDirectory(SystemSession, "/", "", null);
            folder.CreateDirectory("Folder1");
            folder.CreateDirectory("Folder2");
            folder.CreateDirectory("Folder3");
            folder.CreateFile("file1.txt");
            folder.CreateFile("file2.txt");
            folder.CreateFile("file3.txt");

            FileSystemDirectory folder1 = new FileSystemDirectory(SystemSession, "/", "Folder1", null);
            folder1.CreateFile("f1.txt");
            FileSystemDirectory folder2 = new FileSystemDirectory(SystemSession, "/", "Folder2", null);
            folder2.CreateFile("f2.txt");
            FileSystemDirectory folder3 = new FileSystemDirectory(SystemSession, "/", "Folder3", null);
            folder3.CreateFile("f3.txt");
        }

        public static void DeleteAllData()
        {
            FileSystemDirectory folder = new FileSystemDirectory(SystemSession, "/", "", null);
            IEnumerable<string> files = folder.FileNames;
            foreach (string file in files)
            {
                FileSystemFile fileo = new FileSystemFile(SystemSession, "/", file, null);
                fileo.Delete();
            }
            IEnumerable<string> folders = folder.SubDirectoryNames;
            foreach (string f in folders)
            {
                FileSystemDirectory fld = new FileSystemDirectory(SystemSession, "/", f, null);
                fld.Delete();
            }
        }

        public static FileSystemDirectory GenerateRootFolder()
        {
            return new FileSystemDirectory(SystemSession, "/", "", null);
        }

        public static FileSystemDirectory GenerateFolder(string name)
        {
            if(name.IsNullOrEmpty()) throw new ArgumentNullException("name");

            return new FileSystemDirectory(SystemSession, "/", name, null);
        }
    }
}
