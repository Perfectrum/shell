namespace shell.expression;

using shell.enviroment;
using System.Collections;

public enum ExpTypes { Assignment, Command, Pipe }

public abstract class Expression {
    public ExpTypes Type { get; set; }

    abstract public Result<string> Run(ShellEnvironment env);
}

