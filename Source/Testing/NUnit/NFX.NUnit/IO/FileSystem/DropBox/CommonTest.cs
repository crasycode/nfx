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
        public void DB_CreateAndWriteToFile()
        {
//            FileSystemDirectory folder = TestDataHelper.GenerateRootFolder();
//            FileSystemFile file = folder.CreateFile("notempty.txt");
//            Assert.IsNotNull(file);
//            Assert.IsTrue(file.Name == "notempty.txt");
//            Assert.IsTrue(file.ParentPath == "/");
//            Assert.IsTrue(file.Path == "/notempty.txt");
//
//            const string testValue = "test value";
//            byte[] buff = Encoding.UTF8.GetBytes(testValue);
//            file.FileStream.Write(buff, 0, buff.Length);
//            file.FileStream.Flush();
//
//            FileSystemFile file2 = folder.GetFile("notempty.txt");
//            Assert.IsTrue(file2.FileStream.Length == buff.Length);
//            byte[] buff2 = new byte[buff.Length];
//            file2.FileStream.Write(buff2, 0, (int)file2.FileStream.Length);
//            string textFromFile = Encoding.UTF8.GetString(buff2);
//
//            Assert.IsTrue(testValue == textFromFile);
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
