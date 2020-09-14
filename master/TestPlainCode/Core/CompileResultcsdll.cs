using System;
using System.Collections.Generic;

internal class CompileResultcsdll : CompileResult
{
    internal bool isdone;
    internal bool hasError;
    internal string message;
    internal float progress;

    public override bool IsDone { get { return isdone; } }
    public override bool HasError { get { return hasError; } }
    public override string Message { get { return message; } }
    public override float Progress { get { return progress; } }


    internal CompileResultcsdll()
    {
        isdone = false;
        hasError = false;
        message = string.Empty;
        progress = 0;
    }


}
