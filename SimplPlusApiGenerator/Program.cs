using SPlusUtilities;
using System.IO.Compression;

namespace SimplPlusApiGenerator;

public class Program
{
    public static int Main(string[] args) { return CreateApi(args); }
    public static int CreateApi(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: SimplPlusApiGenerator <clz path>");
            return -1;
        }
        var clzPath = args[0];
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
        var clzDirectory = Path.GetDirectoryName(clzPath);
        var projectName = Path.GetFileNameWithoutExtension(clzPath);
        var splsWorkDirectory = Path.Combine(clzDirectory!, "SPlsWork");
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
        SPlusHeader.CreateSPlusHeader(projectDllPath, apiPath);
        var a = SPlusHeader.GetErrorList();
        if (a.Count == 0) { return 0; }
        Console.WriteLine("API Creation Failed");
        foreach (var error in a) { Console.WriteLine(error); }
        return -1;
    }
}