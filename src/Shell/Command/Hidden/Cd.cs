using Shell.Enviroment;

namespace Shell.Command.Hidden;


/// <summary>
///     Класс exit команды,
///     команда отвечает за смену рабочей директории.
/// </summary>
public class CdCommand : Command
{
    /// <summary>
    ///     Создаёт объект класса cd команды,
    ///     команда отвечает за смену рабочей директории.
    /// </summary>
    public CdCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }
    protected override int Go(string[] args)
    {
        base.ByCallThisFunctionIConfirmThatThisFunctionChangeBashStateAndThisIsUnsafeActualy();
        ShellSideEffect = ResultFactory.CreateError<string>("Not implemented");
        return 0;
    }
}
