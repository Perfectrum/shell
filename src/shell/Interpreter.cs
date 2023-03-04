namespace shell;

using shell.enviroment;
using shell.parser;

public class Interpreter
{
    private ShellEnvironment _env = new ShellEnvironment();
    private ShellParser _parser = new ShellParser();

    public Result<string> ProcessRequest(string? request)
    {   
        return _parser.Parse(request, _env).Then((x) => x.Run(_env));
    }
}