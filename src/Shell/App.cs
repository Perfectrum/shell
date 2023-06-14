namespace Shell;

/// <summary>
///     Класс приложения.
/// </summary>
/// <description>
/// Инициализирует UI и Interpreter, а за тем запускает цикл, в котором UI считывает команду
/// и передает ее интерпретатору. Выход из цикла (и, соответственно, приложения) происходит когда обработка введенной
/// пользователем команды завершается ошибкой (Status.Error) или сигналом выхода (Status.Exit).
/// </description>
abstract class App
{
    /// <summary>
    ///     Запускает работу приложения: инициализирует UI и Interpreter, 
    ///     а затем запускает цикл, в котором UI считывает команду
    ///     и передает ее интерпретатору. Выход из цикла (и, соответственно, приложения)
    ///     происходит когда обработка введенной
    ///     пользователем команды завершается ошибкой (Status.Error) 
    ///     или сигналом выхода (Status.Exit).
    /// </summary>
    public static void Main()
    {
        UI ui = new UI();
        var interpreter = new Interpreter();

        while (true)
        {
            var res = interpreter.ProcessRequest(ui.GetCommand());

            switch (res.State)
            {
                case Status.Ok:
                    break;
                case Status.Error:
                    ui.PrintError(res.Message);
                    break;
                case Status.Exit:
                    Console.WriteLine();
                    return;
            }
        }
    }
}
