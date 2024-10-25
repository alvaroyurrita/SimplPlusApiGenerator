﻿using NUnit.Framework;
using SimplPlusApiGenerator;
using System;
using System.IO;

namespace SimplPlusApiGenerator.Tests
{
    [TestFixture()]
    public class ProgramTests
    {
        string currentPath = AppDomain.CurrentDomain.BaseDirectory;
        [SetUp()]
        public void Setup()
        {
            var splsWorkDirectory = Path.Combine(currentPath, "SPlsWork");
            if (Directory.Exists(splsWorkDirectory)) { Directory.Delete(splsWorkDirectory, true); }
        }
        [Test()]
        public void CreateApiTest()
        {
            var sampleClzPath = Path.Combine(currentPath, "SampleSimplSharpLibrary.clz");
            Program.Main(new[] { sampleClzPath });

            var apiExists = File.Exists(Path.Combine(currentPath, "SPlsWork", "SampleSimplSharpLibrary.api"));
            var expectedApiContents = File.ReadAllText(Path.Combine(currentPath, "ExpectedApiContents.txt"));
            var actualApiContents = File.ReadAllText(Path.Combine(currentPath, "SPlsWork", "SampleSimplSharpLibrary.api"));
            Assert.That(apiExists);
            Assert.That(actualApiContents, Is.EqualTo(expectedApiContents));
        }
        [Test()]
        public void CreateApiTest_InvalidPath()
        {
            var invalidClzPath = Path.Combine(currentPath, "InvalidPath.clz");
            var result = Program.Main(new[] { invalidClzPath });
            Assert.That(result, Is.EqualTo(-1));
        }
        [Test()]
        public void CreateApiTest_PartialPath()
        {
            var sampleClzPath = Path.Combine("SampleSimplSharpLibrary.clz");
            Program.Main(new[] { sampleClzPath });

            var apiExists = File.Exists(Path.Combine(currentPath, "SPlsWork", "SampleSimplSharpLibrary.api"));
            var expectedApiContents = File.ReadAllText(Path.Combine(currentPath, "ExpectedApiContents.txt"));
            var actualApiContents = File.ReadAllText(Path.Combine(currentPath, "SPlsWork", "SampleSimplSharpLibrary.api"));
            Assert.That(apiExists);
            Assert.That(actualApiContents, Is.EqualTo(expectedApiContents));
        }
        [Test()]
        public void CreateApiTest_NoArgumenth()
        {
            var result = Program.Main(new string[0] );
            Assert.That(result, Is.EqualTo(-1));
        }
        [Test()]
        public void CreateApiTest_BadClz()
        {
            var invalidClzPath = Path.Combine(currentPath, "BadClz.clz");
            var result = Program.Main(new[] { invalidClzPath });
            Assert.That(result, Is.EqualTo(-1));
        }
        [Test()]
        public void CreateApiTest_BadClz2()
        {
            var invalidClzPath = Path.Combine(currentPath, "BadClz2.clz");
            var result = Program.Main(new[] { invalidClzPath });
            Assert.That(result, Is.EqualTo(-1));
        }
    }
}