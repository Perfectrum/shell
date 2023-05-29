namespace Shell.Command.Integrated;

using Shell.Enviroment;

/// <summary>
///     Класс wc команды,
///     команда отвечает за подсчет количества слов,
///     символов и байтов в конкатеции содержимых переданных файлов.
/// </summary>
public class WcCommand : Command
{
    /// <summary>
    ///     Создаёт объект класса wc команды,
    ///     команда отвечает за подсчет количества слов,
    ///     символов и байтов в конкатеции содержимых переданных файлов.
    /// </summary>
    public WcCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }

    protected override int Go(string[] args)
    {
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