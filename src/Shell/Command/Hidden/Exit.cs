using Shell.Enviroment;

namespace Shell.Command.Hidden;

/// <summary>
///     Класс exit команды,
///     команда отвечает за выход из приложения.
/// </summary>
public class ExitCommand : Command
{
    /// <summary>
    ///     Создаёт объект класса exit команды,
    ///     команда отвечает за выход из приложения.
    /// </summary>
    public ExitCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }
    protected override int Go(string[] args)
    {
        this.ByCallThisFunctionIConfirmThatThisFunctionChangeBashStateAndThisIsUnsafeActualy();
        ShellSideEffect = ResultFactory.CreateTerminate<string>("Exit command called!");
        return 0;
    }
}
