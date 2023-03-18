namespace Shell.Parser.Primitive;

using Shell.Expression;
using Shell;
using System.Collections.Generic;

/// <summary>
///     AssingmentChunkToken = WordToken PalkaToken 
///     <br>
///     Например a=
/// </summary>
public class PipeChunkToken : Token
{
    /// <summary>
    ///     Значение переменной для присваивания
    /// </summary>
    public CommandToken? Value { get; } = null;
    public PipeToken? Pipe { get; } = null;

    public PipeChunkToken(CommandToken value)
    {
        Original = $"{value.Original}|";
        Value = value;
        Type = TokenType.T_PIPE_PRED;
    }

    public PipeChunkToken(PipeToken value)
    {
        Original = $"{value.Original}|";
        Pipe = value;
        Type = TokenType.T_PIPE_PRED;
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
        return $"[PIPE_CHUNK: {Value?.ToDebugString() ?? Pipe?.ToDebugString()}|]";
    }
#endif
}
