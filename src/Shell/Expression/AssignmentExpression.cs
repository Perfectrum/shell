namespace Shell.Expression;


using Shell.Enviroment;

/// <summary>
///     Класс Assignment инкапсулирует аргументы
///     присваивания.
/// </summary>
public class Assignment
{
    public string Name { get; set; } = "";
    public string Value { get; set; } = "";
}

/// <summary>
///     Класс AssignmentExpression отражает выражение, соответствующее
///     некоторому набору присваиваний.
/// </summary>
public class AssignmentExpression : Expression
{
    private List<Assignment> _assignments;

    /// <summary>   
    ///     Класс AssignmentExpression отражает выражение, соответствующее
    ///     некоторому набору присваиваний.
    /// </summary>
    public AssignmentExpression(List<Assignment> assignments)
    {
        _assignments = assignments;
        Type = ExpTypes.Assignment;
    }

    /// <summary>
    ///     Конструирует объект для исполнения, соответствующий присваиваниям,
    ///     обёрнутый в объект-результат.
    /// </summary>
    public override Result<Box> Run(ShellEnvironment env)
    {
        foreach (var p in _assignments)
        {
            env[p.Name] = p.Value;
        }
        return ResultFactory.CreateEmpty<Box>();
    }
}
