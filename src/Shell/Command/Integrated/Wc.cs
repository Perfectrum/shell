namespace Shell.Command.Integrated;

using Shell.Enviroment;

class WsCommand : Command
{
    public WsCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }

    protected override int Go(string[] args)
    {
        StdOut.WriteLine("Not implemented!");
        return -1;
    }
}
