using Shell.Enviroment;

namespace Shell.Command.Hidden;

public class CdCommand : InternalCommand
{
    public override Result<string> Process(string[] args, ShellEnvironment env)
    {
        return ResultFactory.CreateError<string>("Not implemented");
    }
}
