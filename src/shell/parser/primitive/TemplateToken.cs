namespace shell.parser.primitive;

using shell.expression;
using System.Collections.Generic;
using System.Linq;

public class TemplateToken : Token {

    public List<string> Pattern { get; private set; }
    public List<string> VarNames { get; }
    public TemplateToken(string name) {
        Pattern = new List<string>() { "", "" };
        VarNames = new List<string>() { name };
        Type = TokenType.T_TMP;
        Original = $"${name}";
    }
    public override Token? Join(Stack<Token> x) {
        if (x.Count == 0) {
            return null;
        }

        if (x.Peek().Type == TokenType.T_TMP) {
            var temp = (TemplateToken)x.Pop();
            temp.Original += Original;
            temp.VarNames.AddRange(VarNames);
            
            var first = Pattern[0];
            Pattern.RemoveAt(0);

            temp.Pattern[temp.Pattern.Count - 1] += first;
            temp.Pattern.AddRange(Pattern);
            return temp;
        }
        
        var t = x.Pop();
        Original = t.Original + Original;
        Pattern[0] = t.Original + Pattern[0];

        return this;
    }

    public Token Absorb(Token x) {
        Original += x.Original;
        Pattern[Pattern.Count - 1] += x.Original;
        return this;
    }

    public string Resolve(List<string> vars) {
        vars.Add("");
        return Enumerable.Zip(Pattern, vars).Aggregate("", (a, x) => a + x.First + x.Second);
    }

     public override Result<Expression> Render() {
        return ResultFactory.CreateError<Expression>("Sytax error!");
    }

    #if DEBUG
    public override string ToDebugString() {
        return $"[TMP: '{string.Join("{$$$}", Pattern)}' {string.Join(", ", VarNames.Select(x => $"${x}"))} ]";
    }
    #endif
}
