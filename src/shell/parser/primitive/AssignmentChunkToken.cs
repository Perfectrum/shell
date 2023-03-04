namespace shell.parser.primitive;

using shell.expression;
using shell;
using System.Collections.Generic;

public class AssingmentChunkToken : Token {
    public string Value { get; }
    public AssingmentChunkToken(string value) {
        Original = $"{value}=";
        Value = value;
        Type = TokenType.T_ASS_PRED;
        Original = value;
    }
    public override Token? Join(Stack<Token> x) {

        if (x.Count == 0) {
            return null;
        }

        if (x.Peek().Type == TokenType.T_TMP) {
            var t = (TemplateToken)x.Pop();
            return t.Absorb(this);
        }

        return null;
    }

    public override Result<Expression> Render() {
        return ResultFactory.CreateError<Expression>("Syntax error!");
    }

    #if DEBUG
    public override string ToDebugString() {
        return $"[ASS_CHUNK: {Value}=]";
    }
    #endif
}
