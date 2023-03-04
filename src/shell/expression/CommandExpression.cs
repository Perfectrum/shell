namespace shell.expression;

using System.Collections;
using shell.enviroment;

public class CommandExpression : Expression {
    private string _command;
    private List<string> _args;
    private List<Assignment> _vars;
    public CommandExpression(List<Assignment> vars, string command, List<string> args) {
        _command = command;
        _args = args;
        _vars = vars;

        Type = ExpTypes.Assignment;
    }

    public override Result<string> Run(ShellEnvironment env) {
        var e = env.CreateView();
        foreach (var i in _vars) {
            e[i.Name] = i.Value;
        }
        if (_command == "echo") {
            return ResultFactory.CreateResult<string>(string.Join(" ", _args));
        }
        return ResultFactory.CreateError<string>($"'{_command}' Not implemented!");
    }
}

