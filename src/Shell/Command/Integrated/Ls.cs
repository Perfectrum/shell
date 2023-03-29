using Shell.Enviroment;

namespace Shell.Command.Integrated;

public class LsCommand : Command
{
    public LsCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }
    protected override int Go(string[] args)
    {
        base.ByCallThisFunctionIConfirmThatThisFunctionChangeBashStateAndThisIsUnsafeActualy();
        ShellSideEffect = ResultFactory.CreateError<string>("Not implemented");
        return 0;
    }
}
