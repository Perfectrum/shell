namespace shell;

public interface IInterpreter
{
    public Result ProcessRequest(string request);
}