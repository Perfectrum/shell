namespace shell;

abstract class App
{
    public static void Main()
    {
        UI ui = new UI();
        IInterpreter interpreter = new StubInterpreter();

        while (true)
        {
            Result res = interpreter.ProcessRequest(ui.GetCommand());

            switch (res.State)
            {
                case Status.Ok:
                    ui.PrintText(res.Message);
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