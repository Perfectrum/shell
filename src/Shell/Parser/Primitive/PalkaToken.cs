namespace Shell.Parser.Primitive;

using Shell.Expression;
using System.Collections.Generic;

/// <summary>
///     PalkaToken = '|'.
/// </summary>
public class PalkaToken : Token
{
    /// <summary>
    ///     PalkaToken = '|'.
    ///     Создаёт объект класса PalkaToken.
    /// </summary>
    public PalkaToken()
    {
        Original = "|";
        Type = TokenType.T_PLK;
    }

    public override Token? Join(Stack<Token> x)
    {
        if (x.Count == 0)
        {
            return null;
        }

        if (x.Peek().Type == TokenType.T_ASS_PRED)
        {
            var t = (AssingmentChunkToken)x.Pop();
            return new PipeChunkToken(new CommandToken($"{t.Value}=|")) { Original = t.Original + this.Original };
        }

        if (x.Peek().Type == TokenType.T_WORD)
        {
            var t = (WordToken)x.Pop();
            return new PipeChunkToken(new CommandToken(t.Value)) { Original = t.Original + this.Original };
        }

        if (x.Peek().Type == TokenType.T_CMD)
        {
            var t = (CommandToken)x.Pop();
            return new PipeChunkToken(t) { Original = t.Original + this.Original };
        }

        if (x.Peek().Type == TokenType.T_ASS)
        {
            var t = (AssingmentToken)x.Pop();
            return new CommandToken(t, "") { Original = t.Original + this.Original };
        }

        if (x.Peek().Type == TokenType.T_PIPE)
        {
            var t = (PipeToken)x.Pop();
            return new PipeChunkToken(t) { Original = t.Original + this.Original };
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
        return "[PALKA: '|']";
    }
#endif
}
