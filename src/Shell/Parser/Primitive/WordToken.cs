namespace Shell.Parser.Primitive;

using System.Collections.Generic;
using Shell.Expression;

/// <summary>
///     WordToken = Word | WordToken WordToken
///     <br>
///     Например: echo или 'e'ch"o"
/// </summary>
public class WordToken : Token
{
    public string Value { get; }

    public WordToken(string value)
    {
        Original = value;
        Value = value;
        Type = TokenType.T_WORD;
    }

    public override Token? Join(Stack<Token> x)
    {
        if (x.Count == 0)
        {
            return null;
        }

        if (x.Peek().Type == TokenType.T_WORD)
        {
            var t = (WordToken)x.Pop();
            return new WordToken(t.Value + Value) { Original = t.Original + this.Original };
        }

        if (x.Peek().Type == TokenType.T_TMP)
        {
            var t = (TemplateToken)x.Pop();
            return t.Absorb(this);
        }

        // TODO: smth

        return null;
    }

    public override Result<Expression> Render()
    {
        return ResultFactory.CreateError<Expression>("Syntax error!");
    }

#if DEBUG
    public override string ToDebugString()
    {
        return $"[WRD: '{Value}']";
    }
#endif
}
