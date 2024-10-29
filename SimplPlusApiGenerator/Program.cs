using SPlusUtilities;
using System;
using System.IO;
using System.IO.Compression;

namespace SimplPlusApiGenerator
{

    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length < 1 || args.Length > 2)
            {
                Console.WriteLine("Usage: SimplPlusApiGenerator <clz path> [<SIMPL Directory>]");
                return -1;
            }
            var clzPath = args[0];
            var simplPath = "C:\\Program Files (x86)\\Crestron\\Simpl";
            if (args.Length > 1)
            {
                simplPath = args[1];
            }
            var splusUtilitiesPath = Path.Combine(simplPath, "SPlusUtilities.dll");
            if (!Path.IsPathRooted(clzPath))
            {
                var currentPath = AppDomain.CurrentDomain.BaseDirectory;
                clzPath = Path.Combine(currentPath, clzPath);
            }
            if (!File.Exists(clzPath))
            {
                Console.WriteLine("File not found");
                return -1;
            }
            if (!Directory.Exists(simplPath))
            {
                Console.WriteLine("SIMPL Directory not found");
                return -1;
            }
            if (!File.Exists(splusUtilitiesPath))
            {
                Console.WriteLine("SPlusUtilities.dll not found");
                return -1;
            }
            var clzDirectory = Path.GetDirectoryName(clzPath);
            var projectName = Path.GetFileNameWithoutExtension(clzPath);
            var splsWorkDirectory = Path.Combine(clzDirectory, "SPlsWork");
            if (!Directory.Exists(splsWorkDirectory)) { Directory.CreateDirectory(splsWorkDirectory); }
            using (var clzContents = ZipFile.OpenRead(clzPath))
            {
                foreach (var entry in clzContents.Entries)
                {
                    var entryDestinationPath = Path.Combine(splsWorkDirectory, entry.Name);
                    try { entry.ExtractToFile(entryDestinationPath, true); }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return -1;
                    }
                }
            }
            var projectDllPath = Path.Combine(splsWorkDirectory, $"{projectName}.dll");
            var apiPath = Path.Combine(splsWorkDirectory, $"{projectName}.api");
            try
            {
                var SPlusHeader2 = new SPlusHeader2(splusUtilitiesPath);
                SPlusHeader2.CreateSPlusHeader(projectDllPath, apiPath);
                var b = SPlusHeader2.GetOutputList();
                var a = SPlusHeader2.GetErrorList();
                if (a.Count == 0) { return 0; }
                Console.WriteLine("API Creation Failed");
                foreach (var error in a) { Console.WriteLine(error); }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
            return -1;
        }
    }
}