namespace Shell.Parser.Primitive;

using Shell.Expression;
using System.Collections.Generic;
using System.Linq;

/// <summary>
///     Пара из названия переменной и значения для присваивания.
/// </summary>
public class VarPair
{
    public string Name { get; set; } = "";
    public string Value { get; set; } = "";
}

/// <summary>
///     AssingmentToken = AssingmentChunkToken CommandToken
///                      / AssingmentToken AssingmentToken.
///     <br>
///     Например a=423 или a=423 b=x.
/// </summary>
public class AssingmentToken : Token
{
    /// <summary>
    ///     Массив присваиваний.
    /// </summary>
    public List<VarPair> Values { get; }

    /// <summary>
    ///     AssingmentToken = AssingmentChunkToken CommandToken
    ///                      / AssingmentToken AssingmentToken.
    ///     Создаёт объект класса AssingmentToken.
    ///     <br>
    ///     Например a=423 или a=423 b=x.
    /// </summary>
    public AssingmentToken(string name, string value)
    {
        Original = $"{name}={value}";
        Values = new List<VarPair>
        {
            new VarPair() { Name = name, Value = value }
        };
        Type = TokenType.T_ASS;
    }

    /// <summary>
    ///     AssingmentToken = AssingmentChunkToken CommandToken
    ///                      / AssingmentToken AssingmentToken.
    ///     Создаёт объект класса AssingmentToken.
    ///     <br>
    ///     Например a=423 или a=423 b=x.
    /// </summary>
    public AssingmentToken(AssingmentChunkToken left, CommandToken right)
    {
        Original = left.Original + right.Original;
        var name = left.Value;
        var value = right.Name;
        Values = new List<VarPair>
        {
            new VarPair() { Name = name, Value = value }
        };
        Type = TokenType.T_ASS;
    }

    public override Token? Join(Stack<Token> x)
    {
        if (x.Count == 0)
        {
            return null;
        }

        if (x.Peek().Type == TokenType.T_ASS)
        {
            var t = (AssingmentToken)x.Pop();
            t.Original += Original;
            t.Values.AddRange(this.Values);
            return t;
        }

        if (x.Peek().Type == TokenType.T_CMD)
        {
            var cmd = new CommandToken(
                string.Join(" ", Values.Select(x => x.Name + '=' + x.Value).ToArray())
            );
            cmd.Original += Original;
            return cmd;
        }

        if (x.Peek().Type == TokenType.T_TMP)
        {
            var t = (TemplateToken)x.Pop();
            return t.Absorb(this);
        }

        return null;
    }

    public override Result<Expression> Render()
    {
        return ResultFactory.CreateResult<Expression>(
            new AssignmentExpression(
                Values.Select(x => new Assignment() { Name = x.Name, Value = x.Value }).ToList()
            )
        );
    }

#if DEBUG
    public override string ToDebugString()
    {
        return $"[ASS {string.Join(", ", Values.Select(x => $"'{x.Name}'='{x.Value}'"))}]";
    }
#endif
}
