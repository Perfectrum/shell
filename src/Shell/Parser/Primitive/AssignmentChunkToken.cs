namespace Shell.Parser.Primitive;

using Shell.Expression;
using Shell;
using System.Collections.Generic;

/// <summary>
///     AssingmentChunkToken = WordToken EqToken.
///     <br>
///     Например a=.
/// </summary>
public class AssingmentChunkToken : Token
{
    /// <summary>
    ///     Значение переменной для присваивания.
    /// </summary>
    public string Value { get; }

    /// <summary>
    ///     AssingmentChunkToken = WordToken EqToken.
    ///     Создаёт объект класса AssingmentChunkToken.
    ///     <br>
    ///     Например a=.
    /// </summary>
    public AssingmentChunkToken(string value)
    {
        Original = $"{value}=";
        Value = value;
        Type = TokenType.T_ASS_PRED;
    }

    public override Token? Join(Stack<Token> x)
    {
        if (x.Count == 0)
        {
            return null;
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
        return ResultFactory.CreateError<Expression>("Syntax error!");
    }

#if DEBUG
    public override string ToDebugString()
    {
        return $"[ASS_CHUNK: {Value}=]";
    }
#endif
}
