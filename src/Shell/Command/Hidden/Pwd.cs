using Shell.Enviroment;

namespace Shell.Command.Hidden;

public class PwdCommand : Command
{
    public PwdCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }
    protected override int Go(string[] args)
    {
        base.ByCallThisFunctionIConfirmThatThisFunctionChangeBashStateAndThisIsUnsafeActualy();
        ShellSideEffect = ResultFactory.CreateResult<string>(Env["PWD"]);
        return 0;
    }
}
