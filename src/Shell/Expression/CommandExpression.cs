namespace Shell.Expression;

using Shell.Command;
using Shell.Enviroment;

public class CommandExpression : Expression
{
    private string _command;
    private List<string> _args;
    private List<Assignment> _vars;

    public CommandExpression(List<Assignment> vars, string command, List<string> args)
    {
        _command = command;
        _args = args;
        _vars = vars;

        Type = ExpTypes.Assignment;
    }

    public override Result<Task<int>> Run(ShellEnvironment env)
    {
        var e = env.CreateView();
        foreach (var i in _vars)
        {
            e[i.Name] = i.Value;
        }
        return CommandResolver.StartCommand(_command, _args.ToArray(), env);
    }
}
