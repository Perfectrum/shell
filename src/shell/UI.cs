namespace shell;

using System;

public class UI
{
    private readonly string prompt = "$ ";

    public string? GetCommand()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(prompt);

        string? request = Console.ReadLine();
        while (request != null && request.EndsWith('\\'))
        {
            request = request.Remove(request.Length-1) + Console.ReadLine();
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