namespace Shell.Command.Integrated;

using Shell.Enviroment;

class EchoCommand : Command
{
    public EchoCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }

    protected override int Go(string[] args)
    {
        StdOut.WriteLine(string.Join(" ", args));
        return 0;
    }
}
