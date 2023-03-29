using Shell.Enviroment;

namespace Shell.Command.Integrated;

public class LsCommand : Command
{
    public LsCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e)
    {
    }

    protected override int Go(string[] args)
    {
        int returnCode = 0;
        var path = Env["PWD"];
        string shift = "./";

        if (args.Length > 0)
            shift = args[0];
        path = Path.GetFullPath(Path.Combine(path, shift));

        if (File.Exists(path))
        {
            StdOut.WriteLine(String.Join(" ", Path.GetFileName(path)));
            return 0;
        }

        if (Directory.Exists(path))
        {
            try
            {
                List<string> entries = Directory.GetFileSystemEntries(path).ToList();
                StdOut.WriteLine(String.Join(" ", entries.ConvertAll(Path.GetFileName)));
                return 0;
            }
            catch (DirectoryNotFoundException)
            {
                returnCode = -1;
            }
        }
        StdOut.WriteLine("ls: cannot access '" + path + "' No such file or directory");
        return returnCode;
    }
}
