namespace Shell.Command.Integrated;

using Shell.Enviroment;

class WcCommand : Command
{
    public WcCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }

    protected override int Go(string[] args)
    {
        StdOut.WriteLine("Not implemented!");
        return -1;
    }
}
