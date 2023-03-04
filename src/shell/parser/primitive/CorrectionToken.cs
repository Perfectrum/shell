namespace shell.parser.primitive;

using shell.expression;
using System.Collections.Generic;

public class CorrectionToken : Token {

    public CorrectionToken(string val) {
        Original = val;
        Type = TokenType.T_COR;
    }
    public override Token? Join(Stack<Token> x) {
        if (x.Count == 0) {
            return null;
        }

        var a = x.Pop();
        a.Original += Original;
        return a;
    }

    public override Result<Expression> Render() {
        return ResultFactory.CreateError<Expression>("Syntax error!");
    }

    #if DEBUG
    public override string ToDebugString() {
        return $"[COR: '{Original}']";
    }
    #endif
}
