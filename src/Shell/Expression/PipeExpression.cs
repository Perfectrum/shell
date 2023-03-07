namespace Shell.Expression;

using System.Collections.Generic;
using Shell.Enviroment;
using Shell;

public class PipeExpression : Expression
{
    List<Expression> _commands;

    public PipeExpression(Expression left, Expression right)
    {
        _commands = new List<Expression>() { left, right };
    }

    public PipeExpression(List<Expression> commands)
    {
        _commands = commands;
    }

    public override Result<Task<int>> Run(ShellEnvironment env)
    {
        return ResultFactory.CreateError<Task<int>>("Not implemented!");
    }
}
