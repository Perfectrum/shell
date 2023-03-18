using Shell.Enviroment;

namespace Shell.Command.Hidden;

public class CdCommand : Command
{
    public CdCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }
    protected override int Go(string[] args)
    {
        base.ByCallThisFunctionIConfirmThatThisFunctionChangeBashStateAndThisIsUnsafeActualy();
        ShellSideEffect = ResultFactory.CreateError<string>("Not implemented");
        return 0;
    }
}
