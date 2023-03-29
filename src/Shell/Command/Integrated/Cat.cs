namespace Shell.Command.Integrated;

using Shell.Enviroment;

public class CatCommand : Command
{
    public CatCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }

    protected override int Go(string[] args)
    {
        string? s = this.StdIn.ReadLine();
        int returnCode = 0;
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
                returnCode = -1;
            }
        }
        return returnCode;
    }
}
