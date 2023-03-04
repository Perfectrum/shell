namespace shell;

abstract class App
{
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
                    ui.PrintText(res.GetValueOrDefault(""));
                    break;
                case Status.Error:
                    ui.PrintError(res.Message);
                    break;
                case Status.Exit:
                    return;
            }
        }
    }
}