namespace Shell;

public enum Status
{
    Ok,
    Empty,
    Error,
    Exit
};

/// <summary>
/// Обёртка над результатами отработки классов, 
/// реализует [монаду](https://ru.wikipedia.org/wiki/Монада_(программирование)).
/// Помимо результатов передаёт состояния и ошибки.
/// </summary>
public class Result<T>
{
    /// <summary>
    /// Обёртка над результатами отработки классов, 
    /// реализует [монаду](https://ru.wikipedia.org/wiki/Монада_(программирование)).
    /// Помимо результатов передаёт состояния и ошибки.
    /// Создаёт объект класса результата.
    /// </summary>
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

    /// <summary>
    /// bind монады результата.
    /// https://en.wikipedia.org/wiki/Monad_(functional_programming)
    /// </summary>
    public Result<U> Then<U>(MonadHandler<U> f)
    {
        if (State == Status.Ok && Value != null)
        {
            return f(Value);
        }
        return new Result<U>(State, Message, default(U));
    }

    public delegate U FunctorHandler<U>(T x);

    /// <summary>
    /// map монады результата
    /// https://en.wikipedia.org/wiki/Monad_(functional_programming)
    /// </summary>
    public Result<U> Map<U>(FunctorHandler<U> f)
    {
        if (State == Status.Ok && Value != null)
        {
            return new Result<U>(State, Message, f(Value));
        }
        return new Result<U>(State, Message, default(U));
    }
}

/// <summary>
/// Класс ResultFactory инкапсулирует создание объектов-результатов.
/// </summary>
public static class ResultFactory
{
    /// <summary>
    /// Создать обертку результат над переданным значением.
    /// </summary>
    public static Result<U> CreateResult<U>(U v)
    {
        return new Result<U>(Status.Ok, "", v);
    }

    /// <summary>
    /// Создать обертку результат над переданным значением
    /// - строкой ошибки.
    /// </summary>
    public static Result<U> CreateError<U>(string msg)
    {
        return new Result<U>(Status.Error, msg, default(U));
    }

    /// <summary>
    /// Создать обертку результат c информацией
    /// о выходе из программы, и переданной строкой. 
    /// </summary>
    public static Result<U> CreateTerminate<U>(string msg)
    {
        return new Result<U>(Status.Exit, msg, default(U));
    }

    /// <summary>
    /// Создать обертку результат c дефолтным значением типа
    /// и пустым сообщением.
    /// </summary>
    public static Result<U> CreateEmpty<U>()
    {
        return new Result<U>(Status.Empty, "", default(U));
    }

    /// <summary>
    /// Traverse для списка результатов.
    /// https://runebook.dev/ru/docs/haskell/libraries/base-4.15.0.0/data-traversable
    /// </summary>
    public static Result<List<T>> Traverse<T>(List<Result<T>> list)
    {
        List<T> res = new List<T>();
        foreach (var r in list)
        {
            if (r.State != Status.Ok)
            {
                return new Result<List<T>>(r.State, r.Message, default(List<T>));
            }
            res.Add(r.Value!);
        }

        return ResultFactory.CreateResult<List<T>>(res);
    }

    /// <summary>
    /// Соединить все результаты bindом монады. 
    /// </summary>
    public static Result<T>? Join<T>(List<Result<T>?> list)
    {
        List<Result<T>> pure = new List<Result<T>>();
        foreach (var e in list)
        {
            if (e != null)
            {
                pure.Add(e);
            }
        }
        if (pure.Count == 0)
        {
            return null;
        }

        var x = pure.First();
        for (int i = 1; i < pure.Count; ++i)
        {
            x = x.Then(a => pure[i]);
        }

        return x;
    }
}
