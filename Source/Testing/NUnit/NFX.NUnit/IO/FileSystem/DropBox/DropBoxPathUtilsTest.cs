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


using NFX.Web.IO.FileSystem.DropBox.BL;
using NUnit.Framework;

namespace NFX.NUnit.IO.FileSystem.DropBox
{
    [TestFixture]
    public class DropBoxPathUtilsTest
    {
        [Test]
        public void GetParentPathFromPathTest()
        {
            Assert.IsTrue(DropBoxPathUtils.GetParentPathFromPath("/123") == "/");
            Assert.IsTrue(DropBoxPathUtils.GetParentPathFromPath("/123/456") == "/123");
            Assert.IsTrue(DropBoxPathUtils.GetParentPathFromPath("/123/456/6.txt") == "/123/456");
            Assert.IsTrue(DropBoxPathUtils.GetParentPathFromPath("6.txt") == "");
            Assert.IsTrue(DropBoxPathUtils.GetParentPathFromPath("") == "");
            Assert.IsTrue(DropBoxPathUtils.GetParentPathFromPath(null) == "");
        }

        [Test]
        public void GetNameFromPathTest()
        {
            Assert.IsTrue(DropBoxPathUtils.GetNameFromPath("/123") == "123");
            Assert.IsTrue(DropBoxPathUtils.GetNameFromPath("/123/456") == "456");
            Assert.IsTrue(DropBoxPathUtils.GetNameFromPath("/123/456/6.txt") == "6.txt");
            Assert.IsTrue(DropBoxPathUtils.GetNameFromPath("/") == "");
            Assert.IsTrue(DropBoxPathUtils.GetNameFromPath("6.txt") == "6.txt");
            Assert.IsTrue(DropBoxPathUtils.GetNameFromPath("") == "");
            Assert.IsTrue(DropBoxPathUtils.GetNameFromPath(null) == "");
        }

        [Test]
        public void CompinePathTest()
        {
            Assert.IsTrue(DropBoxPathUtils.Combine("", "", "") == "");
            Assert.IsTrue(DropBoxPathUtils.Combine(null, null, null) == "");
            Assert.IsTrue(DropBoxPathUtils.Combine("") == "");
            Assert.IsTrue(DropBoxPathUtils.Combine("123", "456") == "/123/456");
            Assert.IsTrue(DropBoxPathUtils.Combine("", "456") == "/456");
            Assert.IsTrue(DropBoxPathUtils.Combine("/123", "/456") == "/123/456");
            Assert.IsTrue(DropBoxPathUtils.Combine("/123/", "/456/") == "/123/456");
            Assert.IsTrue(DropBoxPathUtils.Combine("123/", "456/") == "/123/456");
            Assert.IsTrue(DropBoxPathUtils.Combine("/", "123") == "/123");
            Assert.IsTrue(DropBoxPathUtils.Combine("/", "/123") == "/123");
            Assert.IsTrue(DropBoxPathUtils.Combine("/", "/123/") == "/123");
            Assert.IsTrue(DropBoxPathUtils.Combine("/", "123/") == "/123");
            Assert.IsTrue(DropBoxPathUtils.Combine("123", "") == "/123");
            Assert.IsTrue(DropBoxPathUtils.Combine("123", "/") == "/123");
            Assert.IsTrue(DropBoxPathUtils.Combine("123/", "") == "/123");
            Assert.IsTrue(DropBoxPathUtils.Combine("/123", "") == "/123");
            Assert.IsTrue(DropBoxPathUtils.Combine("/123/456", "") == "/123/456");
            Assert.IsTrue(DropBoxPathUtils.Combine("/123/456/", "") == "/123/456");
            Assert.IsTrue(DropBoxPathUtils.Combine("/123/456/", "789") == "/123/456/789");
            Assert.IsTrue(DropBoxPathUtils.Combine("/123/456/", "/789") == "/123/456/789");
            Assert.IsTrue(DropBoxPathUtils.Combine("/123/456/", "/789/") == "/123/456/789");
            Assert.IsTrue(DropBoxPathUtils.Combine("/123/456/", "789/") == "/123/456/789");
            Assert.IsTrue(DropBoxPathUtils.Combine("/123/456", "789/") == "/123/456/789");
            Assert.IsTrue(DropBoxPathUtils.Combine("/123/456", "/789/") == "/123/456/789");
            Assert.IsTrue(DropBoxPathUtils.Combine("123/456", "/789/") == "/123/456/789");
            Assert.IsTrue(DropBoxPathUtils.Combine("123/456/", "789/") == "/123/456/789");
            Assert.IsTrue(DropBoxPathUtils.Combine("123/456/", "/789") == "/123/456/789");
            Assert.IsTrue(DropBoxPathUtils.Combine("//123//456//", "//789//") == "/123/456/789");

            Assert.IsTrue(DropBoxPathUtils.Combine("//123/456/", "/789/") == "/123/456/789");
            Assert.IsTrue(DropBoxPathUtils.Combine("/123/456//", "789/") == "/123/456/789");
            Assert.IsTrue(DropBoxPathUtils.Combine("/123//456", "789/") == "/123/456/789");
            Assert.IsTrue(DropBoxPathUtils.Combine("///123//456", "789/") == "/123/456/789");
            Assert.IsTrue(DropBoxPathUtils.Combine("///123///456", "//789/") == "/123/456/789");
            Assert.IsTrue(DropBoxPathUtils.Combine("/////", "//789/") == "/789");
            Assert.IsTrue(DropBoxPathUtils.Combine("//1///", "//////") == "/1");
        }

        [Test]
        public void IsValidStringPath()
        {
            string valid = "/123/123";
            string notValid = "/12*3/123";
            string notValid2 = "/123/12>3";

            Assert.IsTrue(DropBoxPathUtils.IsValid(valid));
            Assert.IsFalse(DropBoxPathUtils.IsValid(notValid));
            Assert.IsFalse(DropBoxPathUtils.IsValid(notValid2));
        }

        [Test]
        public void IsValidParts()
        {
            string[] valid = {"/123/123", "65"};
            string[] notValid = {"/12*3/123", "65"};
            string[] notValid2 = {"/123/123", "6>5"};

            Assert.IsTrue(DropBoxPathUtils.IsValid(valid));
            Assert.IsFalse(DropBoxPathUtils.IsValid(notValid));
            Assert.IsFalse(DropBoxPathUtils.IsValid(notValid2));
        }
    }
}
