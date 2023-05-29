namespace Shell.Parser;

using System;
using System.Collections.Generic;
using Shell.Parser.Primitive;
using Shell.Expression;
using Shell;
using Shell.Enviroment;

/// <summary>
///     Enum, который перечисляет состояния парсера.
/// <summary>
enum ParseState
{
    SPACE,
    WORD,
    WEAK_Q,
    STRONG_Q,
    VARIABLE,
    QUOTE
}

/// <summary>
///     Класс, который выполняет безусловный (без контекста) парсинг строки.
/// </summary>
public class ParseAutomaton
{
    private ParseState state = ParseState.SPACE;
    private ParseState prev = ParseState.SPACE;
    private Stack<Token> stack = new Stack<Token>();

    private string buffer = "";
    private bool escaped = false;

    /// <summary>
    ///     Метод, который запускает парсинг.
    /// </summary>
    /// <param name="input">Входная строка.</param>
    /// <returns> Стэк с результатом парсинга.</returns>
    public Stack<Token> Run(string input)
    {
        foreach (var s in input.Trim())
        {
            if (s == '\\' && !escaped && state != ParseState.VARIABLE)
            {
                escaped = true;
                continue;
            }
            switch (state)
            {
                case ParseState.SPACE:
                    {
                        if (!Char.IsWhiteSpace(s))
                        {
                            prev = state;
                            if (s == '$')
                            {
                                state = ParseState.VARIABLE;
                            }
                            else if (s == '\'' && !escaped)
                            {
                                state = ParseState.STRONG_Q;
                            }
                            else if (s == '"' && !escaped)
                            {
                                state = ParseState.WEAK_Q;
                            }
                            else if (s == '=')
                            {
                                Reduce(new EqToken());
                            }
                            else if (s == '|')
                            {
                                Reduce(new PalkaToken());
                            }
                            else
                            {
                                buffer += s;
                                state = ParseState.WORD;
                            }
                        }
                        else
                        {
                            Reduce(new WsToken(s));
                        }
                        break;
                    }
                case ParseState.STRONG_Q:
                    {
                        if (s == '\'' && !escaped)
                        {
                            Reduce(new WordToken(buffer) { Original = $"'{buffer}'" });
                            buffer = "";
                            state = ParseState.SPACE;
                        }
                        else
                        {
                            buffer += s;
                        }
                        break;
                    }
                case ParseState.WORD:
                    {
                        if (Char.IsWhiteSpace(s))
                        {
                            Reduce(new WordToken(buffer));
                            Reduce(new WsToken());
                            buffer = "";
                            state = ParseState.SPACE;
                        }
                        else if (s == '=' && !escaped)
                        {
                            Reduce(new WordToken(buffer));
                            Reduce(new EqToken());
                            buffer = "";
                            state = ParseState.SPACE;
                        }
                        else if (s == '\'' && !escaped)
                        {
                            Reduce(new WordToken(buffer));
                            buffer = "";
                            state = ParseState.STRONG_Q;
                        }
                        else if (s == '"' && !escaped)
                        {
                            Reduce(new WordToken(buffer));
                            buffer = "";
                            state = ParseState.WEAK_Q;
                        }
                        else if (s == '$' && !escaped)
                        {
                            Reduce(new WordToken(buffer));
                            buffer = "";
                            state = ParseState.VARIABLE;
                        }
                        else
                        {
                            buffer += s;
                        }
                        break;
                    }
                case ParseState.WEAK_Q:
                    {
                        if (s == '"' && !escaped)
                        {
                            Reduce(new WordToken(buffer) { Original = $"{buffer}" });
                            buffer = "";
                            state = ParseState.SPACE;
                        }
                        else if (s == '$' && !escaped)
                        {
                            Reduce(new WordToken(buffer) { Original = $"{buffer}" });
                            buffer = "";
                            prev = ParseState.WEAK_Q;
                            state = ParseState.VARIABLE;
                        }
                        else
                        {
                            buffer += s;
                        }
                        break;
                    }
                case ParseState.VARIABLE:
                    {
                        if (Char.IsWhiteSpace(s))
                        {
                            Reduce(new TemplateToken(buffer));
                            Reduce(new WsToken());
                            buffer = "";
                            state = prev;
                        }
                        else if (s == '\'')
                        {
                            Reduce(new TemplateToken(buffer));
                            buffer = "";
                            state = ParseState.STRONG_Q;
                        }
                        else if (s == '"')
                        {
                            Reduce(new TemplateToken(buffer));
                            buffer = "";
                            if (prev == ParseState.WEAK_Q)
                            {
                                Reduce(new CorrectionToken("\'"));
                                state = ParseState.SPACE;
                            }
                            else
                            {
                                state = ParseState.WEAK_Q;
                            }
                        }
                        else if (s == '$')
                        {
                            Reduce(new TemplateToken(buffer));
                            buffer = "";
                        }
                        else if (!(Char.IsLetterOrDigit(s) || s == '_'))
                        {
                            Reduce(new TemplateToken(buffer));
                            buffer = "";
                            state = prev;
                        }
                        else
                        {
                            buffer += s;
                        }
                        break;
                    }
            }

            escaped = false;
        }

        if (buffer.Length > 0)
        {
            if (state == ParseState.WORD)
            {
                Reduce(new WordToken(buffer));
            }
            else if (state == ParseState.VARIABLE)
            {
                Reduce(new TemplateToken(buffer));
            }
            else
            {
                Reduce(new ErrorToken());
            }
        }

        Reduce(new EndToken());
        return stack;
    }


    private void Reduce(Token top)
    {
        Token prev = top;
        Token? next = top;
        while (next != null)
        {
            // Console.WriteLine(next.ToDebugString());
            prev = next;
            next = next.Join(stack);
        }
        stack.Push(prev);
    }
}

/// <summary>
///     Класс <c>ShellParser</c> представляет собой объект, 
///     который преобразует строку в представление,
///     готовое для исполнения интерпретатором.
/// </summary>
public class ShellParser
{

    /// <summary>
    ///    Преобразует входную строку в выражение, готовое к исполнению.
    /// </summary>
    /// <param name="input">Входная строка.</param>
    /// <param name="env">Контекст. Нужен для разрешения подстановок.</param>
    /// <returns>
    ///    <c>Expression</c> завернутый в монаду <c>Result</c>.
    /// </returns>
    public Result<Expression> Parse(string? input, ShellEnvironment env)
    {
        if (input == null)
        {
            return ResultFactory.CreateTerminate<Expression>("\n");
        }

        ParseAutomaton p = new ParseAutomaton();
        var stack = p.Run(input);

        if (stack.Count > 1)
        {
            return ResultFactory.CreateError<Expression>("Parse error!");
        }

        var result = stack.Pop();

#if DEBUG
        Console.WriteLine("=== PARSE DEBUG ========");
        Console.WriteLine(result.ToDebugString());
        Console.WriteLine("========================");
#endif

        if (result.Type == TokenType.T_TMP)
        {
#if DEBUG
            Console.WriteLine("=== VARS RESOLVING =====");
#endif

            var tmp = (TemplateToken)result;
            return this.Parse(
                tmp.Resolve(
                    tmp.VarNames
                        .Select(x =>
                        {
                            var r = env[x];
#if DEBUG
                            Console.WriteLine($"-- {x} = '{r}'");
#endif
                            return r;
                        })
                        .ToList()
                ),
                env
            );
        }

        return result.Render();
    }
}
