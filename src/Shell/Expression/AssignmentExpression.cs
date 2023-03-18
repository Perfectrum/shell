namespace Shell.Expression;

using System.Collections;
using Shell.Enviroment;

public class Assignment
{
    public string Name { get; set; } = "";
    public string Value { get; set; } = "";
}

public class AssignmentExpression : Expression
{
    private List<Assignment> _assignments;

    public AssignmentExpression(List<Assignment> assignments)
    {
        _assignments = assignments;
        Type = ExpTypes.Assignment;
    }

    public override Result<Box> Run(ShellEnvironment env)
    {
        foreach (var p in _assignments)
        {
            env[p.Name] = p.Value;
        }
        return ResultFactory.CreateEmpty<Box>();
    }
}
