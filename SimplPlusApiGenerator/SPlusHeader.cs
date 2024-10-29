using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SimplPlusApiGenerator
{
    internal class SPlusHeader2
    {
        private readonly Type _splusHeader;
        private readonly Assembly _splusUtilitiesAssembly;

        internal SPlusHeader2(string splusUtilitiesPath)
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            if (!File.Exists(splusUtilitiesPath))
            {
                throw new Exception("SPlusUtilities.dll not found");
            }
            _splusUtilitiesAssembly = Assembly.LoadFrom(splusUtilitiesPath);
            _splusHeader = _splusUtilitiesAssembly.GetType("SPlusUtilities.SPlusHeader")
                ?? throw new Exception("SPlusUtilities.SPlusHeader not found");
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Console.WriteLine(args.Name);
            if (args.Name.Contains("SPlusUtilities"))
            {
                return _splusUtilitiesAssembly;
            }
            return null;
        }

        internal void CreateSPlusHeader(string dllPath, string hPath)
        {
            var createSPlusHeaderMethod = _splusHeader.GetMethod("CreateSPlusHeader")
                ?? throw new Exception("CreateSPlusHeader not found");
                createSPlusHeaderMethod.Invoke(_splusHeader, new object[] { dllPath, hPath });
        }

        internal List<string> GetErrorList()
        {
            var getErrorListMethod = _splusHeader.GetMethod("GetErrorList")
                ?? throw new Exception("GetErrorList not found");
            var errorList = new List<string>();
                errorList = (List<string>)getErrorListMethod.Invoke(_splusHeader, new object[0]);
            return errorList;
        }

        internal List<string> GetOutputList() {
            var getOutputListMethod = _splusHeader.GetMethod("GetOutputList")
                ?? throw new Exception("GetOutputList not found");
            return (List<string>)getOutputListMethod.Invoke(_splusHeader, new object[0]);
        }
    }
}
