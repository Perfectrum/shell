namespace shell;

public enum Status { Ok, Error, Exit };

public struct Result
{
    public Result(Status state, string message)
    {
        State = state;
        Message = message;
    }

    public readonly Status State;
    
    public readonly string Message;
}