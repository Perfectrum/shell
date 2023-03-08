namespace Shell.Command.Integrated;

using Shell.Enviroment;

class CatCommand : Command
{
    public CatCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }

    protected override int Go(string[] args) 
    {   
        foreach (var arg in args)
        {
            try
            {
                var content = File.ReadAllLines(arg);
                StdOut.WriteLine(string.Join("\n", content));
            }
            catch (FileNotFoundException) {
                StdOut.WriteLine("cat: " + arg + " No such file or directory");
            }
        }
        return 0;
    }
}
