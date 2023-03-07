using Shell.Enviroment;

namespace Shell.Command.Hidden;

public class PwdCommand : InternalCommand
{
    public override Result<string> Process(string[] args, ShellEnvironment env)
    {
        return ResultFactory.CreateResult<string>(env["PWD"]);
    }
}
