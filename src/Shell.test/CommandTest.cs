namespace Shell.Command.Test;

using Shell.Enviroment;
using Shell.Parser;
using Shell.Command;
using Shell.Command.Integrated;
using Shell.Command.Hidden;
using System.IO;
using System;


#if DEBUG || TEST

[Collection("Command tests")]
public class CommandTest
{
    [Fact]
    public void EchoTest()
    {
        string[] args1 = {"Из чёрной резины сделана власть",
                          "abc", "some_string"};
        string[] args2 = {"What's wrong with being",
                            "a fortnite character"};
        string[] args3 = { "Spaces   test", "     1  2 " };

        string[] args4 = { "AAAaaaaBBBBCCCcddd", "AAAaaaaBBBBCCCcddd",
                            "AAAaaaaBBBBCCCcddd" };

        string[] args5 = { "12345",
                           "56789" };

        testEchoWithArgs(args1);
        testEchoWithArgs(args2);
        testEchoWithArgs(args3);
        testEchoWithArgs(args4);
        testEchoWithArgs(args5);
    }

    private string extractOutput(Command command,
                string[] args, MemoryStream stream)
    {
        command.Run(args).Wait();
        command.StdOut.Original?.Flush();
        stream.Position = 0;
        return (new StreamReader(stream)).ReadToEnd();
    }

    private void testEchoWithArgs(string[] args)
    {
        var stream = new MemoryStream();
        var streamWriter = new StreamWriter(stream);
        var echo = new EchoCommand(new StreamReader(new MemoryStream()),
                                   streamWriter, new ShellEnvironment());
        string expected = String.Join(' ', args);
        Assert.StartsWith(expected, extractOutput(echo, args, stream));
    }

    [Fact]
    public void CatTest()
    {
        string[] lines1 = {"Poopy-di scoop", "Scoop-diddy-whoop",
                            "Whoop-di-scoop-di-poop"};
        string[] lines2 = {"If I need some racks, I'ma flip me some packs",
                            "I talk like I want and she don't say nothin' back"};
        string[] lines3 = {"ABCDEFGHIJKabcdefghi",
                            "ABCDEFGHIJKabcdefghi",
                            "ABCDEFGHIJKabcdefghi",
                            "ABCDEFGHIJKabcdefghi"};
        string[] lines4 = Enumerable.Repeat("abcde fgh abcde fgh", 300).ToArray();                
        testCatWithText(lines1);
        testCatWithText(lines2);
        testCatWithText(lines3);
        testCatWithText(lines4);
    }

    private void testCatWithText(string[] lines)
    {
        string path = Path.GetTempFileName();
        string text = string.Join('\r', lines);
        using (StreamWriter file = new(path))
        {
            file.WriteLine(text);
            file.Close();
        }

        var stream = new MemoryStream();
        var streamWriter = new StreamWriter(stream);
        var cat = new CatCommand(new StreamReader(new MemoryStream()),
                                   streamWriter, new ShellEnvironment());
        string output = extractOutput(cat, new string[] { path }, stream);

        foreach (var line in lines)
        {
            Assert.Contains(line, output);
        }
    }

    /* Тест зависит от платформы
    [Fact]
    public void WcTest()
    {
        string[] lines1 = {"Poopy-di scoop", "Scoop-diddy-whoop",
                            "Whoop-di-scoop-di-poop"};
        string[] lines2 = {"If I need some racks, I'ma flip me some packs",
                            "I talk like I want and she don't say nothin' back"};
        //
        //    Преподсчитанные значения, записанные
        //    без пробелов.
        //
        string expected1 = "3457";
        string expected2 = "22197";
        testWcWithText(lines1, expected1);
        testWcWithText(lines2, expected2);
    }
    */
    private void testWcWithText(string[] lines,
                                string expected)
    {
        string path = Path.GetTempFileName();
        string text = string.Join('\r', lines);
        using (StreamWriter file = new(path))
        {
            file.WriteLine(text);
            file.Close();
        }

        var stream = new MemoryStream();
        var streamWriter = new StreamWriter(stream);
        var wc = new WcCommand(new StreamReader(new MemoryStream()),
                                streamWriter, new ShellEnvironment());

        Assert.StartsWith(expected, RemoveSpaces(
            extractOutput(wc, new string[] { path }, stream)));
    }


    [Fact]
    public void grepTestOnlyPattern()
    {
        string[] lines1 = {"Poopy-di scoop", "Scoop-diddy-whoop",
                            "Whoop-di-scoop-di-poop"};
        string[] args1 = {"Poopy"};

        string[] lines2 = {"If I need some racks, I'ma flip me some packs",
                            "I talk like I want and she don't say nothin' back"};
        string[] args2 = {"talk"};

        string[] lines3 = {"ABCDEFGHIJKabcdefghi",
                            "ABCDEFGHIJKabcdefghi",
                            "ABCDEFGHIJKabcdefghi",
                            "ABCDEFGHIJKabcdefghi"};
        string[] args3 = {"l"};

        string[] lines4 = {"ABCDEFGHIJKabcdefghi",
                            "abcdefghi",
                            "lmnnp",
                            "qrst"};                
        string[] args4 = {"l"};
        
        testGrepWithText(lines1, args1, "Poopy-di scoop");
        testGrepWithText(lines2, args2, "I talk like I want");
        testGrepWithText(lines3, args3, "");
        testGrepWithText(lines4, args4, "lmnnp");
    }

    [Fact]
    public void grepTestRegisterFree()
    {
        string[] lines1 = {"Poopy-di scoop", "Scoop-diddy-whoop",
                            "Whoop-di-scoop-di-poop"};
        string[] args1 = {"-i", "Poopy"};

        string[] lines2 = {"If I need some racks, I'ma flip me some packs",
                            "I talk like I want and she don't say nothin' back"};
        string[] args2 = {"-i", "TALK"};

        string[] lines3 = {"ABCDEFGHIJKabcdefghi",
                            "ABCDEFGHIJKabcdefghi",
                            "ABCDEFGHIJKabcdefghi",
                            "ABCDEFGHIJKabcdefghi"};
        string[] args3 = {"-i", "l"};

        string[] lines4 = {"ABCDEFGHIJKabcdefghi",
                            "abcdefghi",
                            "lmnnp",
                            "qrst"};                
        string[] args4 = {"-i", "L"};
        
        testGrepWithText(lines1, args1, "Poopy-di scoop");
        testGrepWithText(lines2, args2, "I talk like I want");
        testGrepWithText(lines3, args3, "");
        testGrepWithText(lines4, args4, "lmnnp");
    }

    [Fact]
    public void grepTestWithCounter()
    {
        string [] lines1 = {"Poopy-di scoop", "Scoop-diddy-whoop",
                            "Whoop-di-scoop-di-poop"};
        string[] args1 = {"-A 1", "di"};

        string[] lines2 = {"If I need some racks, I'ma flip me some packs",
                            "I talk like I want and she don't say nothin' back"};
        string[] args2 = {"-A 0", "need"};

        string[] lines3 = {"ABCDEFGHIJKabcdefghi",
                            "ABCDEFGHIJKabcdefghi",
                            "ABCDEFGHIJKabcdefghi",
                            "ABCDEFGHIJKabcdefghi"};
        string[] args3 = {"-A 10", "l"};

        string[] lines4 = {"ABCDEFGHIJKabcdefghi",
                            "abcdefghi",
                            "lmnnp",
                            "qrst"};                
        string[] args4 = {"-A 4", "a"};
        
        testGrepWithText(lines1, args1, "Whoop-");
        testGrepWithText(lines2, args2, "");
        testGrepWithText(lines3, args3, "");
        testGrepWithText(lines4, args4, "qrst");
    }


    [Fact]
    public void grepTestWithWord()
    {
        string [] lines1 = {"Poopy-di scoop", "Scoop-diddy-whoop",
                            "Whoop-di-scoop-di-poop"};
        string[] args1 = {"-w", "di"};

        string[] lines2 = {"If I need some racks, I'ma flip me some packs",
                            "I talk like I want and she don't say nothin' back"};
        string[] args2 = {"-w", "talk"};

        string[] lines3 = {"ABCDEFGHIJKabcdefghi",
                            "ABCDEFGHIJKabcdefghi",
                            "ABCDEFGHIJKabcdefghi",
                            "ABCDEFGHIJKabcdefghi"};
        string[] args3 = {"-w", "ABDCDE"};

        string[] lines4 = {"ABCDEFGHIJKabcdefghi",
                            "abcdefghi",
                            " lmnnp ",
                            "qrst"};                
        string[] args4 = {"-w", "lmnnp"};
        
        testGrepWithText(lines1, args1, "");
        testGrepWithText(lines2, args2, "I talk like I");
        testGrepWithText(lines3, args3, "");
        testGrepWithText(lines4, args4, "lmnnp");
    }

    private void testGrepWithText(string[] lines,
                           string [] args,
                           string expected)
    {
        string path = Path.GetTempFileName();
        string text = string.Join('\r', lines);
        using (StreamWriter file = new(path))
        {
            file.WriteLine(text);
            file.Close();
        }

        var stream = new MemoryStream();
        var streamWriter = new StreamWriter(stream);
        var grepCommand = new GrepCommand(new StreamReader(new MemoryStream()),
                                streamWriter, new ShellEnvironment());
        List<string> ArgsAndPath = new List<string>(args);
        ArgsAndPath.Add(path);
        /* Ищет левое в правом! */
        Assert.Contains(expected, extractOutput(grepCommand, ArgsAndPath.ToArray(), stream));
    }

    private string RemoveSpaces(string input)
    {
        return new string(input.ToCharArray()
            .Where(c => !Char.IsWhiteSpace(c))
            .ToArray());
    }

}

#endif
