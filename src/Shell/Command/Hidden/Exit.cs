using Shell.Enviroment;

namespace Shell.Command.Hidden;

public class ExitCommand : Command
{
    public ExitCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }
    protected override int Go(string[] args)
    {
        this.ByCallThisFunctionIConfirmThatThisFunctionChangeBashStateAndThisIsUnsafeActualy();
        ShellSideEffect = ResultFactory.CreateTerminate<string>("Exit command called!");
        return 0;
    }
}
