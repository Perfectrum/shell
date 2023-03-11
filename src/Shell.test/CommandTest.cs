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
        string [] args1 = {"Из чёрной резины сделана власть",
                          "abc", "some_string"};
        string [] args2 = {"What's wrong with being",
                            "a fortnite character"};
        string [] args3 = {"Spaces   test", "     1  2 "};

        testEchoWithArgs(args1);
        testEchoWithArgs(args2);
        testEchoWithArgs(args3);
    }

    private string extractOutput(Command command,
                string[] args, MemoryStream stream) {
        command.Go(args);  
        command.StdOut.Flush();
        stream.Position = 0;
        return (new StreamReader(stream)).ReadToEnd();
    }

    private void testEchoWithArgs(string [] args) {
        var stream = new MemoryStream();
        var streamWriter = new StreamWriter(stream);
        var echo = new EchoCommand(new StreamReader(new MemoryStream()),
                                   streamWriter, new ShellEnvironment());
        string expected = String.Join(' ', args);
        Assert.True(extractOutput(echo, args, stream).StartsWith(expected));
    }

    [Fact]
    public void CatTest()
    {
        string [] lines1 = {"Poopy-di scoop", "Scoop-diddy-whoop",
                            "Whoop-di-scoop-di-poop"};
        string [] lines2 = {"If I need some racks, I'ma flip me some packs",
                            "I talk like I want and she don't say nothin' back"};
        testCatWithText(lines1);
        testCatWithText(lines2);
    }

    private void testCatWithText(string [] lines) {
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
        string output = extractOutput(cat, new string [] {path}, stream);
        
        foreach (var line in lines)
        {
            Assert.True(output.Contains(line));
        }
    }

    [Fact]
    public void WcTest()
    {
        string [] lines1 = {"Poopy-di scoop", "Scoop-diddy-whoop",
                            "Whoop-di-scoop-di-poop"};
        string [] lines2 = {"If I need some racks, I'ma flip me some packs",
                            "I talk like I want and she don't say nothin' back"};
        /** 
            Преподсчитанные значения, записанные
            без пробелов.
        */
        string expected1 = "3457"; 
        string expected2 = "22197"; 
        testWcWithText(lines1, expected1);
        testWcWithText(lines2, expected2);
    }
    private void testWcWithText(string [] lines,
                                string expected) {
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
        
        Assert.True(RemoveSpaces(
            extractOutput(wc, new string [] {path}, stream)).StartsWith(expected));
    }

 private string RemoveSpaces(string input)
 {
    return new string(input.ToCharArray()
        .Where(c => !Char.IsWhiteSpace(c))
        .ToArray());
 }

}

#endif
