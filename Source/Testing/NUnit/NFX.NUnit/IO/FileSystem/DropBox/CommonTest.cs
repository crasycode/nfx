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


using System.Collections.Generic;
using System.IO;
using System.Text;
using NFX.IO.FileSystem;
using NUnit.Framework;

namespace NFX.NUnit.IO.FileSystem.DropBox
{
    [TestFixture]
    public class CommonTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            TestDataHelper.InitTestData();
        }

        [Test]
        public void DB_GetFileNames()
        {
            FileSystemDirectory folder = TestDataHelper.GenerateRootFolder();
            List<string> fileNames = (List<string>)folder.FileNames;
            Assert.IsNotEmpty(fileNames);
            Assert.IsTrue(fileNames.Contains("file1.txt"));
            Assert.IsTrue(fileNames.Contains("file2.txt"));
            Assert.IsTrue(fileNames.Contains("file3.txt"));
        }

        [Test]
        public void DB_GetFileNamesRecursive()
        {
            FileSystemDirectory folder = TestDataHelper.GenerateRootFolder();
            List<string> fileNames = (List<string>)folder.RecursiveFileNames;
            Assert.IsNotEmpty(fileNames);
            Assert.IsTrue(fileNames.Contains("file1.txt"));
            Assert.IsTrue(fileNames.Contains("file2.txt"));
            Assert.IsTrue(fileNames.Contains("file3.txt"));
            Assert.IsTrue(fileNames.Contains("f1.txt"));
            Assert.IsTrue(fileNames.Contains("f2.txt"));
            Assert.IsTrue(fileNames.Contains("f3.txt"));
        }

        [Test]
        public void DB_CreateFolder()
        {
            FileSystemDirectory folder = TestDataHelper.GenerateRootFolder();
            FileSystemDirectory createdFolder = folder.CreateDirectory("NFXFolder");
            Assert.NotNull(createdFolder);
            Assert.IsTrue(createdFolder.Name == "NFXFolder");
        }

        [Test]
        public void DB_CreateSubFolder()
        {
            FileSystemDirectory folder = TestDataHelper.GenerateFolder("NFXFolder");
            FileSystemDirectory createdFolder = folder.CreateDirectory("SubNFXFolder");
            Assert.NotNull(createdFolder);
            Assert.IsTrue(createdFolder.Name == "SubNFXFolder");
            Assert.IsTrue(createdFolder.ParentPath == "/NFXFolder");
            Assert.IsTrue(createdFolder.Path == "/NFXFolder/SubNFXFolder");
        }

        [Test]
        public void DB_CreateEmptyFile()
        {
            FileSystemDirectory folder = TestDataHelper.GenerateRootFolder();
            FileSystemFile file = folder.CreateFile("empty.txt");
            Assert.IsNotNull(file);
            Assert.IsTrue(file.Name == "empty.txt");
            Assert.IsTrue(file.ParentPath == "/");
            Assert.IsTrue(file.Path == "/empty.txt");
        }

        [Test]
        public void DB_CreateAndWriteToLocalFile()
        {
            FileSystemDirectory folder = TestDataHelper.GenerateRootFolder();
            string p = Path.GetFullPath(@"IO\FileSystem\DropBox\TestData\Adventures-of-Tom-Sawyer.pdf");
            folder.CreateFile("blabla.pdf", p);
            FileSystemFile loaded = folder.GetFile("blabla.pdf");
            Assert.IsNotNull(loaded);
            Assert.IsTrue(loaded.Name == "blabla.pdf");
            Assert.IsTrue(loaded.Path == "/blabla.pdf");
            Assert.IsTrue(loaded.FileStream.Length > 0);

            string path = Path.GetFullPath(@"IO\FileSystem\DropBox\TestData\loaded.pdf");
            using (FileStream st = File.Create(path))
            {
                byte[] buffer = new byte[loaded.FileStream.Length];
                int readC = loaded.FileStream.Read(buffer, 0, buffer.Length);
                st.Write(buffer, 0, readC);
                st.Flush();
            }
            Assert.IsTrue(File.Exists(path));
        }

        [Test]
        public void DB_ChangeFile()
        {
            const string defaultValue = "this is text before changes"; // from file
            string testValue = " - 1111";
            const string newValue = "this is text before changes - 1111";

            FileSystemDirectory folder = TestDataHelper.GenerateRootFolder();
            string p = Path.GetFullPath(@"IO\FileSystem\DropBox\TestData\test_change.txt");
            folder.CreateFile("test_change.txt", p);
            FileSystemFile loaded = folder.GetFile("test_change.txt");
            Assert.IsNotNull(loaded);
            Assert.IsNotNull(loaded.Path == "/test_change.txt");

            byte[] utf8Value = Encoding.UTF8.GetBytes(defaultValue + testValue);
            loaded.FileStream.Position = 0;
            loaded.FileStream.Write(utf8Value, 0, utf8Value.Length);
            loaded.FileStream.Flush();

            FileSystemFile changedFile = folder.GetFile("test_change.txt");
            Assert.IsNotNull(changedFile);
            Assert.IsNotNull(changedFile.Path == "/test_change.txt");
            string content2 = changedFile.ReadAllText();

            Assert.IsTrue(newValue == content2);
        }

        [Test]
        public void DB_CreateDateTimeFile()
        {
            FileSystemDirectory folder = TestDataHelper.GenerateRootFolder();
            FileSystemFile file = folder.CreateFile("date.txt");
            Assert.IsNotNull(file);
            Assert.IsTrue(file.CreationTimestamp.HasValue);
        }

        [TestFixtureTearDown]
        public void Complete()
        {
            TestDataHelper.DeleteAllData();
        }
    }
}
