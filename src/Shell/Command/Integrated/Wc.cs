namespace Shell.Command.Integrated;

using Shell.Enviroment;

class WcCommand : Command
{
    public WcCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }

    protected override int Go(string[] args)
    {
 foreach (var arg in args)
        {
            try 
            {
                var content = File.ReadLines(arg);
                var byteContent = File.ReadAllBytes(arg);

                int linesCount = 0;
                int bytesCount = byteContent.Length;                
                int wordsCount = 0;
                foreach (var line in content) 
                {
                    linesCount += 1;
                    wordsCount += line.Split().Length;
                }

                StdOut.WriteLine(linesCount.ToString() + " " +
                                 wordsCount.ToString() + " " +
                                 bytesCount.ToString() + " " + arg);
            }
            catch (FileNotFoundException)
            {
                StdOut.WriteLine("cat: " + arg + " No such file or directory");
            }
        }
        return 0;
    }
}
