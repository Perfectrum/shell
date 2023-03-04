namespace shell.parser.primitive;

using shell.expression;
using System.Collections.Generic;

public class EqToken : Token {

    public EqToken() {
        Original = "=";
        Type = TokenType.T_EQ;
    }
    public override Token? Join(Stack<Token> x) {
        if (x.Count == 0) {
            var w = new WordToken("=") { Original = this.Original };
            return w;
        }

        if (x.Peek().Type == TokenType.T_WORD) {
            var t = (WordToken)x.Pop();
            return new AssingmentChunkToken(t.Value) { Original = t.Original + this.Original };
        }

        if (x.Peek().Type == TokenType.T_TMP) {
            var t = (TemplateToken)x.Pop();
            return t.Absorb(this);
        }
        
        return new WordToken("=") { Original = this.Original };
    }

    public override Result<Expression> Render() {
        return ResultFactory.CreateError<Expression>("Sytax error!");
    }

    #if DEBUG
    public override string ToDebugString() {
        return "[EQ: '=']";
    }
    #endif
}
