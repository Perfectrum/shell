namespace Shell.Expression;

using Shell.Enviroment;
using System.Collections;

public enum ExpTypes
{
    Assignment,
    Command,
    Pipe
}

public abstract class Expression
{
    public ExpTypes Type { get; set; }

    abstract public Result<Box> Run(ShellEnvironment env);
}
