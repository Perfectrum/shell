namespace shell;

public class StubInterpreter : IInterpreter
{
    private IEnvironment _env = new StubEnvironment();

    public Result ProcessRequest(string request)
    {
        if (request == "exit")
        {
            return new Result(Status.Exit, "");
        } 
        if (request.StartsWith("echo $"))
        {
            return new Result(Status.Ok, _env.Get(request.Substring(6, request.Length-6)));
        }
        return new Result(Status.Error, "Unknown command");
    }
}