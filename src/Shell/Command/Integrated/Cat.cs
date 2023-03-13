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
                using (StreamReader sr = new StreamReader(arg))
                {
                    string? line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        StdOut.WriteLine(line);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                StdOut.WriteLine("cat: " + arg + " No such file or directory");
                return -1;
            }
        }
        return 0;
    }
}
