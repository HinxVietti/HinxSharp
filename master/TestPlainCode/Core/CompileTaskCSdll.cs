using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CompileTaskCSdll : ICompileTask
{
    public static Encoding Coding = Encoding.ASCII;

    private CompileResultcsdll result;
    CodeDomProvider provider = null;
    CompilerParameters parameters;
    string workDirectory;

    public string ExportDirectory { get; set; }
    public string OutName { get; set; }
    public string Extension => ".dll";
    public string OutputName { get { return Path.Combine(ExportDirectory, OutName + Extension); } }
    List<string> files;
    List<string> resources;

    public CompileTaskCSdll(CodeDomProvider cpo)
    {
        result = new CompileResultcsdll();
        provider = cpo;
        parameters = new CompilerParameters
        {
            GenerateExecutable = false,
            GenerateInMemory = false,
            TreatWarningsAsErrors = false
        };
        workDirectory = Path.Combine(Path.GetTempPath(), "hinx-sharp-compiler/");
        if (Directory.Exists(workDirectory))
            Directory.Delete(workDirectory, true);
        Directory.CreateDirectory(workDirectory);
        random = new Random((int)(DateTime.Now.Ticks % 9999));
        files = new List<string>();
        resources = new List<string>();
    }

    public CompileTaskCSdll()
    {
        result = new CompileResultcsdll();
        provider = CodeDomProvider.CreateProvider("CSharp");
        parameters = new CompilerParameters
        {
            GenerateExecutable = false,
            GenerateInMemory = false,
            TreatWarningsAsErrors = false
        };
        workDirectory = Path.Combine(Path.GetTempPath(), "hinx-sharp-compiler/");
        if (Directory.Exists(workDirectory))
            Directory.Delete(workDirectory, true);
        Directory.CreateDirectory(workDirectory);
        random = new Random((int)(DateTime.Now.Ticks % 9999));
        files = new List<string>();
        resources = new List<string>();
    }


    private Random random;
    public string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public void AddCS(string fileName)
    {
        string saveName = GetSaveName();
        File.Copy(fileName, saveName);
        files.Add(saveName);
    }

    public void AddCS(byte[] csFileData)
    {
        string saveName = GetSaveName();
        string source = Coding.GetString(csFileData);
        File.WriteAllText(saveName, source);
        files.Add(saveName);
    }

    private string GetSaveName()
    {
        string filename = string.Format("csfile-{0}.cs", RandomString(12));
        string saveName = string.Format("{0}\\{1}", workDirectory, filename);
        return saveName;
    }

    public void AddCSCode(string csCode)
    {
        string saveName = GetSaveName();
        File.WriteAllText(saveName, csCode);
        files.Add(saveName);
    }

    public void AddSource(string fileName)
    {
        parameters.EmbeddedResources.Add(fileName);
        //   resources.Add(fileName);
    }

    public void AddSource(byte[] data, string mark)
    {
        string saveName = workDirectory + mark;
        File.WriteAllBytes(saveName, data);
        parameters.EmbeddedResources.Add(saveName);
        //parameters.res
    }

    public bool Compile()
    {
        parameters.OutputAssembly = OutputName;
        try
        {
            var cr = provider.CompileAssemblyFromFile(parameters, files.ToArray());

            if (cr.Errors.Count > 0)
                return false;
            else
                return true;
        }
        catch
        {
            return false;
        }
    }

    public CompileResult CompileAsync()
    {
        var rss = new CompileResultcsdll();
        parameters.OutputAssembly = OutputName;
        Task.Run(() =>
        {
            try
            {
                rss.message = "wait";
                result = rss;
                var cr = provider.CompileAssemblyFromFile(parameters, files.ToArray());

                if (cr.Errors.Count > 0)
                {
                    StringBuilder errors = new StringBuilder();
                    errors.AppendLine(string.Format("Errors building {0} into {1}", OutputName, cr.PathToAssembly));

                    foreach (CompilerError ce in cr.Errors)
                        errors.AppendLine(string.Format("  {0}", ce.ToString()));
                    rss.message = errors.ToString();
                    rss.hasError = true;
                    rss.progress = 0;
                }
                else
                {
                    rss.hasError = false;
                    rss.message = "success";
                    rss.progress = 1;
                }

                rss.isdone = true;
            }
            catch (Exception e)
            {
                rss.hasError = true;
                rss.message = e.ToString();
                rss.isdone = true;
            }
        });
        return rss;
    }

    public void AddReference(string dllName)
    {
        parameters.ReferencedAssemblies.Add(dllName);
    }
}

