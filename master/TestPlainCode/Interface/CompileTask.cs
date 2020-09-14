using System;
using System.Collections.Generic;

public interface ICompileTask
{
    string ExportDirectory { get; set; }
    string OutName { get; set; }
    string Extension { get; }

    void AddCS(string fileName);
    void AddCS(byte[] csFileData);
    void AddCSCode(string csCode);

    void AddSource(string fileName);
    void AddSource(byte[] data,string markName);

    void AddReference(string dllName);

    bool Compile();
    CompileResult CompileAsync();
}
