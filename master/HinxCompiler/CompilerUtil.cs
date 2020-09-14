using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

public class CompilerUtil
{

    public static ICompileTask GetDllCompiler()
    {
        return new CompileTaskCSdll();
    }


    public static ICompileTask GetDllCompilerRoslyn()
    {
        return GetDllCompiler(new CSharpCodeProvider());
    }

    /// <summary>
    /// Create sample as RoslynOptions
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public static ICompileTask GetDllCompilerRoslyn(IProviderOptions options)
    {
        return GetDllCompiler(new CSharpCodeProvider(options));
    }


    public static ICompileTask GetDllCompiler(CodeDomProvider customprovider)
    {
        return new CompileTaskCSdll(customprovider);
    }



}


public class RoslynOptions : IProviderOptions
{
    public string CompilerVersion => "0.1";

    public bool WarnAsError { get; set; }

    public bool UseAspNetSettings => false;

    public IDictionary<string, string> AllOptions { get; } = new Dictionary<string, string>();

    public string CompilerFullPath { get; set; }

    public int CompilerServerTimeToLive { get; set; }
}