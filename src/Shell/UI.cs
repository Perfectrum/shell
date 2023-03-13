namespace Shell;

using System;

/// <summary>
///  Класс UI отвечает за заимодействие с пользователем: вывод приветствия (prompt) и считывание команды.
/// </summary>
public class UI
{
    private readonly string prompt = "$ ";
    
    /// <summary>
    /// Список состояний, используемый для проверки переносов строки в пользовательском вводе.
    /// Строка переносится, если в команде присутствуют незакрытые кавычки или если в конце команды стоит обратный слеш.
    /// </summary>
    public enum State
    {
        Normal,
        Backslash,
        DoubleQuote,
        SingleQuote
    }
    
    /// <summary>
    /// Метод проверяет, закончена ли введенная пользователем команда, или она должна быть продолжена на следующей
    /// строке. Используется методом GetCommand для накопления многострочного ввода. 
    /// </summary>
    /// <param name="request">Введенная пользователем стркоа</param>
    /// <returns>Состояние введенной команды:
    /// <list type="bullet"></list>
    /// <item>State.Normal, если ввод завершен;</item>
    /// <item>State.Backslash, если в конце команды был обратный слеш, который соответствует переносу строки;</item>
    /// <item>State.SingleQuote, если в команде присутствует незакрытая одинарная кавычка;</item>
    /// <item>State.DoubleQuote, если в команде присутствует незакрытая двойная кавычка.</item>
    /// </returns>
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
    
    /// <summary>
    /// Метод, принимающий пользовательский ввод, пока он не будет завершен.
    /// </summary>
    /// <returns>Возвращает введенную пользователем команду в виде строки.</returns>
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

    /// <summary>
    /// Метод для вывода на экран текста ошибки. Текст ошибки подсвечивается специальным цветом.
    /// </summary>
    /// <param name="message">Текст ошибки.</param>
    public void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
    }
}
