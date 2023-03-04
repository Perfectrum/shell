namespace shell.expression;

using System.Collections;
using shell.enviroment;

public class PipeExpression : Expression {
    List<Expression> _commands;
    public PipeExpression(Expression left, Expression right) {
        _commands = new List<Expression>() { left, right };
    }

    public PipeExpression(List<Expression> commands) {
        _commands = commands;
    }

    public override Result<string> Run(ShellEnvironment env) {
        return ResultFactory.CreateError<string>("Not implemented!");
    }
}