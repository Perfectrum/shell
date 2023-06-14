namespace Shell.Parser.Primitive;

using Shell.Expression;
using System.Collections.Generic;

/// <summary>
///     CommandToken = AssingmentToken CommandToken
///                      | CommandToken CommandToken
///                      | WordToken WsToken  .
///     <br>
///     Например echo или echo a b или a=3 echo a b.
/// </summary>
public class CommandToken : Token
{
    /// <summary>
    ///     Имя команды.
    /// </summary>
    public string Name { get; }
    /// <summary>
    ///     Присваивания переменных для этой команды.
    /// </summary>
    public AssingmentToken? Assignments { get; private set; }
    /// <summary>
    ///     Аргументы команды.
    /// </summary>
    public List<string> Arguments { get; }

    /// <summary>
    ///     CommandToken = AssingmentToken CommandToken
    ///                      | CommandToken CommandToken
    ///                      | WordToken WsToken.
    ///     Создаёт объект класса CommandToken.
    ///     <br>
    ///     Например echo или echo a b или a=3 echo a b.
    /// </summary>
    public CommandToken(string value)
    {
        Original = value;
        Assignments = null;
        Arguments = new List<string>();
        Name = value;
        Type = TokenType.T_CMD;
    }

    public CommandToken(AssingmentToken ass, string name)
    {
        Original = ass.Original + name;
        Assignments = ass;
        Name = name;
        Arguments = new List<string>();
        Type = TokenType.T_CMD;
    }

    public override Token? Join(Stack<Token> x)
    {
        if (x.Count == 0)
        {
            return null;
        }

        if (x.Peek().Type == TokenType.T_ASS_PRED)
        {
            if (Assignments == null && Arguments.Count == 0)
            {
                var t = (AssingmentChunkToken)x.Pop();
                var token = new AssingmentToken(t, this);
                token.Original = t.Original + Original;
                return token;
            }
        }

        if (x.Peek().Type == TokenType.T_ASS)
        {
            var t = (AssingmentToken)x.Pop();
            Assignments = t;
            Original = t.Original + Original;
            return this;
        }

        if (x.Peek().Type == TokenType.T_CMD)
        {
            var t = (CommandToken)x.Pop();
            t.Arguments.Add(Name);
            t.Arguments.AddRange(Arguments);
            t.Original += Original;
            return t;
        }

        if (x.Peek().Type == TokenType.T_PIPE_PRED)
        {
            var t = (PipeChunkToken)x.Pop();
            return new PipeToken(t, this);
        }

        if (x.Peek().Type == TokenType.T_PIPE)
        {
            var t = (PipeToken)x.Pop();
            t.Commands.Last().Arguments.Add(Name);
            t.Commands.Last().Arguments.AddRange(Arguments);
            t.Original += Original;
            return t;
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
            new CommandExpression(
                Assignments?.Values
                    .Select(x => new Assignment { Name = x.Name, Value = x.Value })
                    .ToList() ?? new List<Assignment>(),
                Name,
                Arguments
            )
        );
    }

#if DEBUG
    public override string ToDebugString()
    {
        return $"[CMD: {Assignments?.ToDebugString() ?? ""} '{Name}' {string.Join(", ", Arguments)}]";
    }
#endif
}
