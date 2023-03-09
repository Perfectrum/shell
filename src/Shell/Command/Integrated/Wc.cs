namespace Shell.Command.Integrated;

using Shell.Enviroment;

public class WcCommand : Command
{
    public WcCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }

    protected override int Go(string[] args)
    {
        StdOut.WriteLine("GOGOGO");

        int returnCode = 0;
        foreach (var arg in args)
        {
            try
            {
                int linesCount = 0;
                long bytesCount = new System.IO.FileInfo(arg).Length;
                int wordsCount = 0;

                using (StreamReader sr = new StreamReader(arg))
                {
                    string? line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        linesCount += 1;
                        wordsCount += line == "" ? 0 : line.Split().Length;
                    }
                }
                StdOut.WriteLine(linesCount.ToString() + " " +
                                 wordsCount.ToString() + " " +
                                 bytesCount.ToString() + " " + arg);
            }
            catch (FileNotFoundException)
            {
                StdOut.WriteLine("wc: " + arg + " No such file or directory");
                returnCode = -1;
            }
        }
        return returnCode;
    }
}
