namespace Shell.Expression;

using Shell.Command;
using Shell.Enviroment;

public class CommandExpression : Expression
{
    private string _command;
    public string Command => _command;
    private List<string> _args;
    public List<string> Args => _args;
    private List<Assignment> _vars;

    public List<Assignment> Vars => _vars;

    public CommandExpression(List<Assignment> vars, string command, List<string> args)
    {
        _command = command;
        _args = args;
        _vars = vars;

        Type = ExpTypes.Assignment;
    }

    public override Result<Box> Run(ShellEnvironment env)
    {
        var e = env.CreateView();
        foreach (var i in _vars)
        {
            e[i.Name] = i.Value;
        }

        return CommandResolver.FindCommand(_command, env).Map((instantate) =>
        {
            var cmnd = instantate.Invoke(Console.In, Console.Out, env);
            return new Box(cmnd.Run(_args.ToArray(), null, true), cmnd);
        });
    }
}
