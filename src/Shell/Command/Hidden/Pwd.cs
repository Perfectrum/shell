using Shell.Enviroment;

namespace Shell.Command.Hidden;

/// <summary>
/// Класс pwd команды,
/// команда отвечает за вывод рабочей директории.
/// </summary>
public class PwdCommand : Command
{
    /// <summary>
    ///     Создаёт объект класса pwd команды
    ///     команда отвечает за вывод рабочей директории.
    /// </summary>
    public PwdCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }
    protected override int Go(string[] args)
    {
        base.ByCallThisFunctionIConfirmThatThisFunctionChangeBashStateAndThisIsUnsafeActualy();
        ShellSideEffect = ResultFactory.CreateResult<string>(Env["PWD"]);
        return 0;
    }
}
