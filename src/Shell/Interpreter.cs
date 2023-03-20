namespace Shell;

using Shell.Enviroment;
using Shell.Parser;
using Shell.Command;
using Shell.Command.Integrated;
using Shell.Command.Hidden;

/// <summary>
/// Класс интерпретатора.
/// </summary>
/// <description>
/// Инициализирует окружение и парсер. Принимает пользовательский ввод, передает его интерпретатору и возвращает объект
/// результата (Result) с соответствующим результатом.
/// </description>

public class Interpreter
{
    private ShellEnvironment _env = new ShellEnvironment();
    private ShellParser _parser = new ShellParser();

    public Interpreter()
    {
        CommandResolver.RegisterBuiltIn("echo", (r, w, e) => new EchoCommand(r, w, e));
        CommandResolver.RegisterBuiltIn("wc", (r, w, e) => new WcCommand(r, w, e));
        CommandResolver.RegisterBuiltIn("cat", (r, w, e) => new CatCommand(r, w, e));

        CommandResolver.RegisterBuiltIn("exit", (r, w, e) => new ExitCommand(r, w, e));
        CommandResolver.RegisterBuiltIn("pwd", (r, w, e) => new PwdCommand(r, w, e));
        
        CommandResolver.RegisterBuiltIn("grep", (r, w, e) => new GrepCommand(r, w, e));
    }

    /// <summary>
    /// Обработка пользовательского ввода.
    /// </summary>
    /// <param name="request">Пользовательский ввод в виде строки.</param>
    /// <returns>Объект класса Result - результат обработки команды.</returns>
    public Result<bool> ProcessRequest(string? request)
    {
        return _parser
            .Parse(request, _env)
            .Then((x) => x.Run(_env))
            .Then(
                (x) =>
                {
                    try
                    {
                        x.Process.Wait();
                        var code = x.Process.Result;
                        _env["?"] = code.ToString();
                        if (x.Effect != null)
                        {
                            return x.Effect.Map(x =>
                            {
                                Console.WriteLine(x);
                                return false;
                            });
                        }
                        return ResultFactory.CreateResult<bool>(true);
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Console.WriteLine(ex.ToString());
#endif
                        return ResultFactory.CreateResult<bool>(false);
                    }
                }
            );
    }
}
