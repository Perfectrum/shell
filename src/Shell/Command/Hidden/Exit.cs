using Shell.Enviroment;

namespace Shell.Command.Hidden;

public class ExitCommand : InternalCommand
{
    public override Result<string> Process(string[] args, ShellEnvironment env)
    {
        return ResultFactory.CreateTerminate<string>("Exit command called!");
    }
}
