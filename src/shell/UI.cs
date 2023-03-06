namespace shell;

using System;

public class UI
{
    private readonly string prompt = "$ ";

    public string? GetCommand()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(prompt);

        var request = Console.ReadLine();

        if (request!.Length == 0)
        {
            return "";
        }

        switch (request.Last())
        {
            case '\\':
                while (request.EndsWith('\\'))
                {
                    request = request.Remove(request.Length-1) + Console.ReadLine();
                }
                break;
            case '"':
                request += Console.ReadLine();
                while (!request.EndsWith('"'))
                {
                    request += Console.ReadLine();
                }
                break;
            case '\'':
                request += Console.ReadLine();
                while (!request.EndsWith('\''))
                {
                    request += Console.ReadLine();
                }
                break;
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