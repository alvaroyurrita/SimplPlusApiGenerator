using JetBrains.Annotations;
using NUnit.Framework;
using SimplPlusApiGenerator;
using System;
using System.IO;

namespace SimplPlusApiGenerator.Tests;
[TestFixture]
[TestSubject(typeof(Program))]
public class ProgramTest
{

    [Test]
    public void TestApiCreation()
    {
        var currentPath = AppDomain.CurrentDomain.BaseDirectory;
        var sampleClzPath = Path.Combine(currentPath, "SampleSimplSharpLibrary.clz");
        Program.CreateApi(new[] { sampleClzPath });
    }
}