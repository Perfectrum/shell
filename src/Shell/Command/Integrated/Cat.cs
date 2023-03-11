namespace Shell.Command.Integrated;

using Shell.Enviroment;

public class CatCommand : Command
{
    public CatCommand(TextReader i, TextWriter o, ShellEnvironment e)
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
