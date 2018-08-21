﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Xunit;
using XWidget.Utilities;

namespace XWidget.Cryptography.Test {
    public class FileInfoExtensionTest {
        [Fact(DisplayName = "FileInfoExtension.ToHashString")]
        public void ToHashString() {
            var path = Guid.NewGuid().ToString() + ".txt";
            using (var stream = File.CreateText(path)) {
                stream.Write("1234");
            }

            Assert.Equal("81dc9bdb52d04dc20036dbd8313ed055", new FileInfo(path).ToHashString<MD5>(false));

            System.IO.File.Delete(path);
        }

        [Fact(DisplayName = "FileInfoExtension.ToHash")]
        public void ToHash() {
            var path = Guid.NewGuid().ToString() + ".txt";
            using (var stream = File.CreateText(path)) {
                stream.Write("1234");
            }

            Assert.Equal(ByteUtility.FromHex("81dc9bdb52d04dc20036dbd8313ed055"),
                new FileInfo(path).ToHash<MD5>());

            System.IO.File.Delete(path);
        }
    }
}
