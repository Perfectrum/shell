namespace Shell.Parser.Primitive;

using Shell.Expression;
using System.Collections.Generic;

public class ErrorToken : Token
{
    public ErrorToken()
    {
        Original = "";
        Type = TokenType.T_ERR;
    }

    public override Token? Join(Stack<Token> x)
    {
        return null;
    }

    public override Result<Expression> Render()
    {
        return ResultFactory.CreateError<Expression>("Syntax error!");
    }

#if DEBUG
    public override string ToDebugString()
    {
        return $"[ERR: '{Original}']";
    }
#endif
}
