namespace Shell.Parser.Primitive;

using Shell.Expression;
using System.Collections.Generic;

public enum TokenType
{
    T_WORD,
    T_WS,
    T_ASS,
    T_ASS_PRED,
    T_CMD,
    T_EQ,
    T_TMP,
    T_COR,
    T_ERR
}

abstract public class Token
{
    public TokenType Type { get; protected set; }
    public string Original { get; set; } = "";
    abstract public Token? Join(Stack<Token> x);
    abstract public Result<Expression> Render();

#if DEBUG
    abstract public string ToDebugString();
#endif
}
