using System;
using System.Collections.Generic;

public abstract class CompileResult
{
    public abstract bool IsDone { get; }
    public abstract bool HasError { get; }
    public abstract string Message { get; }
    public abstract float Progress { get; }
}
