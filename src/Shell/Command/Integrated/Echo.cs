namespace Shell.Command.Integrated;

using Shell.Enviroment;

/// <summary>
///     Класс echo команды,
///     команда отвечает за вывод строк или ввода.
/// </summary>
public class EchoCommand : Command
{
    /// <summary>
    ///     Создаёт объект класса echo команды,
    ///     команда отвечает за вывод строк или ввода.
    /// </summary>
    public EchoCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }

    protected override int Go(string[] args)
    {
        StdOut.WriteLine(string.Join(" ", args));
        return 0;
    }
}
