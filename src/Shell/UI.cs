
using System.Text;

namespace Shell;

using System;

public class UI
{
    private readonly string prompt = "$ ";

    public enum State
    {
        Normal,
        Backslash,
        DoubleQuote,
        SingleQuote
    }

    public State CheckLineBreak(string request)
    {
        Stack<State> s = new Stack<State>();

        foreach (char ch in request)
        {
            switch (s.Any())
            {
                case false:
                    s.Push(State.Normal);
                    break;
            }

            switch (ch)
            {
                case '\\':
                    switch (s.Peek())
                    {
                        case State.Backslash:
                            s.Pop();
                            break;
                        case State.SingleQuote:
                            break;
                        default:
                            s.Push(State.Backslash);
                            break;
                    }
                    break;
                case '"':
                    switch (s.Peek())
                    {
                        case State.Backslash:
                            s.Pop();
                            break;
                        case State.Normal:
                            s.Pop();
                            s.Push(State.DoubleQuote);
                            break;
                        case State.DoubleQuote:
                            s.Pop();
                            break;
                    }

                    break;
                case '\'':
                    switch (s.Peek())
                    {
                        case State.Backslash:
                            s.Pop();
                            break;
                        case State.Normal:
                            s.Pop();
                            s.Push(State.SingleQuote);
                            break;
                        case State.SingleQuote:
                            s.Pop();
                            break;
                    }
                    break;
                default:
                    switch (s.Peek())
                    {
                        case State.Backslash:
                            s.Pop();
                            s.Push(State.Normal);
                            break;
                    }
                    break;
            }
        }

        return s.Any() ? s.Peek() : State.Normal;
    }

    public string? GetCommand()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(prompt);

        string? request = Console.ReadLine();
        State state = CheckLineBreak(request ?? "");

        if (state == State.Normal)
        {
            return request;
        }

        while (state != State.Normal)
        {
            if (state == State.Backslash)
            {
                request = (request?.Substring(0, request.Length - 1) ?? "");
            }
            else
            {
                request += "\n";
            }
            request += Console.ReadLine();
            state = CheckLineBreak(request);

        }

        return request;
    }

    public void PrintText(string message)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(message);
    }

    public void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
    }
}
