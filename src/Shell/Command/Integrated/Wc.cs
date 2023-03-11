namespace Shell.Command.Integrated;

using Shell.Enviroment;

public class WcCommand : Command
{
    public WcCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }

    public override int Go(string[] args)
    {
<<<<<<< HEAD
        int returnCode = 0;
=======
>>>>>>> 15ad7acafcbad003e5516455dcaea28e09c69a11
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
<<<<<<< HEAD
=======

>>>>>>> 15ad7acafcbad003e5516455dcaea28e09c69a11
                StdOut.WriteLine(linesCount.ToString() + " " +
                                 wordsCount.ToString() + " " +
                                 bytesCount.ToString() + " " + arg);
            }
            catch (FileNotFoundException)
            {
                StdOut.WriteLine("wc: " + arg + " No such file or directory");
<<<<<<< HEAD
                returnCode = -1;
            }
        }
        return returnCode;
=======
                return -1;
            }
        }
        return 0;
>>>>>>> 15ad7acafcbad003e5516455dcaea28e09c69a11
    }
}
