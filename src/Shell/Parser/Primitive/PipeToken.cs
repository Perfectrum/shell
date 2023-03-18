namespace Shell.Parser.Primitive;

using Shell.Expression;
using System.Collections.Generic;
using System.Linq;

/// <summary>
///     AssingmentToken = PipeChunkToken CommandToken
///                      / PipeToken PipeToken
///     <br>
///     Например echo "fsdfds" | cat 
/// </summary>
public class PipeToken : Token
{

    public List<CommandToken> Commands { get; set; }

    public PipeToken(PipeChunkToken left, CommandToken right)
    {
        Original = left.Original + right.Original;
        if (left.Value != null)
        {
            Commands = new List<CommandToken>
            {
                left.Value,
                right
            };
        }
        else
        {
            Commands = left.Pipe?.Commands ?? new List<CommandToken>();
            Commands.Add(right);
        }
        Type = TokenType.T_PIPE;
    }

    public override Token? Join(Stack<Token> x)
    {
        if (x.Count == 0)
        {
            return null;
        }

        if (x.Peek().Type == TokenType.T_PIPE)
        {
            var t = (PipeToken)x.Pop();
            t.Commands.AddRange(this.Commands);
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
        return ResultFactory.Traverse(Commands.Select(x => x.Render()).ToList()).Map<Expression>(e => new PipeExpression(e));
    }

#if DEBUG
    public override string ToDebugString()
    {
        return $"[PIPE: {string.Join(" | ", Commands.Select(x => x.ToDebugString()))}";
    }
#endif
}
