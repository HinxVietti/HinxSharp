#define roslyn
using System;
using System.IO;
using System.Threading;

namespace TestPlatform
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //FolderBrowserDialog fdlg = new FolderBrowserDialog();
            //fdlg.RootFolder = Environment.SpecialFolder.MyComputer;
            //fdlg.ShowDialog();

            //string root = fdlg.SelectedPath;
            string root = @"F:\Gits\Github\hinxsharp\master\TestPlainCode";
            var files = Directory.GetFiles(root, "*", SearchOption.AllDirectories);


            //SaveFileDialog sdlg = new SaveFileDialog();
            //sdlg.Filter = "|*.dll";
            //sdlg.ShowDialog();
            //string path = sdlg.FileName;

            var saveInfo = new FileInfo("output/_ome.dll");
            string path = saveInfo.FullName;
            //provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();
            //CodeDomProvider objCodeCompiler = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();
#if roslyn
            ICompileTask com = CompilerUtil.GetDllCompilerRoslyn();
#else
            var com = new CompileTaskCSdll();
#endif
            com.ExportDirectory = Path.GetDirectoryName(path);
            com.OutName = Path.GetFileNameWithoutExtension(path);
            Console.WriteLine(com.OutputName);

            //com.AddReference(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.CSharp.dll");
            com.AddReference("Microsoft.CSharp.dll");
            com.AddReference("System.dll");
            com.AddReference("System.Core.dll");
            com.AddReference("System.Data.dll");
            com.AddReference("System.Data.DataSetExtensions.dll");
            com.AddReference("System.Xml.Linq.dll");
            //com.AddReference(@"F:\Gits\Github\hinxsharp\master\packages\Microsoft.CodeDom.Providers.DotNetCompilerPlatform.3.6.0\lib\net45\Microsoft.CodeDom.Providers.DotNetCompilerPlatform.dll");

            foreach (var file in files)
            {
                if (file.EndsWith(".cs"))
                    com.AddCS(file);
                else if (file.EndsWith(".dll"))
                { }//.pdb
                else if (file.EndsWith(".pdb"))
                { }
                else if (file.EndsWith(".exe"))
                { }
                else
                    com.AddSource(file);
            }
            var ao = com.CompileAsync();
            while (!ao.IsDone)
            {
                Console.Write(".");
                Thread.Sleep(1000);
            }
            Console.WriteLine();
            Console.WriteLine(ao.Message);
            Console.WriteLine(ao.IsDone);
            Console.ReadKey();
        }
    }
}
//HinxCompiler.dll
