namespace Shell;

public enum Status
{
    Ok,
    Empty,
    Error,
    Exit
};

public class Result<T>
{
    public Result(Status state, string message, T? value)
    {
        State = state;
        Message = message;
        Value = value;
    }

    public Status State { get; }

    public string Message { get; }

    public T? Value { get; }

    public T GetValueOrDefault(T d)
    {
        return Value ?? d;
    }

    public delegate Result<U> MonadHandler<U>(T x);

    public Result<U> Then<U>(MonadHandler<U> f)
    {
        if (State == Status.Ok && Value != null)
        {
            return f(Value);
        }
        return new Result<U>(State, Message, default(U));
    }

    public delegate U FunctorHandler<U>(T x);

    public Result<U> Map<U>(FunctorHandler<U> f)
    {
        if (State == Status.Ok && Value != null)
        {
            return new Result<U>(State, Message, f(Value));
        }
        return new Result<U>(State, Message, default(U));
    }
}

public static class ResultFactory
{
    public static Result<U> CreateResult<U>(U v)
    {
        return new Result<U>(Status.Ok, "", v);
    }

    public static Result<U> CreateError<U>(string msg)
    {
        return new Result<U>(Status.Error, msg, default(U));
    }

    public static Result<U> CreateTerminate<U>(string msg)
    {
        return new Result<U>(Status.Exit, msg, default(U));
    }

    public static Result<U> CreateEmpty<U>()
    {
        return new Result<U>(Status.Empty, "", default(U));
    }
}
