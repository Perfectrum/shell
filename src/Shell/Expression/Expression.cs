namespace Shell.Expression;

using Shell.Enviroment;
using System.Collections;

public enum ExpTypes
{
    Assignment,
    Command,
    Pipe
}

/// <summary>   
///     Класс Expression - результат после парсинга,
///     описывает набор действий, которые нужно исполнить.
/// </summary>
public abstract class Expression
{
    public ExpTypes Type { get; set; }

    /// <summary>   
    ///     Конструирует соответствующий инкапсулируемой семантике
    ///     выражения объект для исполнения, обернутый в объект результат.
    /// </summary>
    abstract public Result<Box> Run(ShellEnvironment env);
}
