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

        testEchoWithArgs(args1);
        testEchoWithArgs(args2);
        testEchoWithArgs(args3);
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
        testCatWithText(lines1);
        testCatWithText(lines2);
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

    [Fact]
    public void WcTest()
    {
        string[] lines1 = {"Poopy-di scoop", "Scoop-diddy-whoop",
                            "Whoop-di-scoop-di-poop"};
        string[] lines2 = {"If I need some racks, I'ma flip me some packs",
                            "I talk like I want and she don't say nothin' back"};
        /** 
            Преподсчитанные значения, записанные
            без пробелов.
        */
        string expected1 = "3456";
        string expected2 = "22196";
        testWcWithText(lines1, expected1);
        testWcWithText(lines2, expected2);
    }
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

    private string RemoveSpaces(string input)
    {
        return new string(input.ToCharArray()
            .Where(c => !Char.IsWhiteSpace(c))
            .ToArray());
    }
    
    [Fact]
    public void LsTest()
    {
        // TODO: создать директорию, наполнить её файлами
        string[] files = {"Poopy", "di", "scoop", "Scoop", "diddy-whoop", "Whoop-di-scoop-di-poop"};
        Array.Sort(files);
        string dir = "test_dir";

        createTestDir(dir, files);
        string absolutePath = Directory.GetCurrentDirectory();

        // TODO: проверить что в директории с файлами есть эти файлы
        string[] args = { dir };
        testLsWithFiles(args, files, absolutePath);
        
        // TODO: проверить что вызов на произвольном файле из созданных не падает и выдаёт само имя файла
        string[] pathToExistingFile = { dir +  files[2] /* склеить директорию с именем файла через системный разделитель */};
        string[] existingFile = { files[2] };
        testLsWithFiles(pathToExistingFile, existingFile, absolutePath);
        
        // TODO: проверить что вызов на произвольном файле из не созданных падает и выдаёт ошибку
        string[] pathToNotExistingFile = { dir +  "not_existing_file" /* склеить директорию с именем файла через системный разделитель */};
        string[] notExistingFile = { "not_existing_file" };
        
        testLsWithFiles(pathToNotExistingFile, notExistingFile, absolutePath);
        
        // TODO: поменять рабочую директорию, зайти в созданную директорию и проверить работоспособность ls без аргументов и аргументом .
        string[] arr = new string[] {};
        testLsWithFiles(arr, files, Path.Join(absolutePath, dir));
        
        string[] dot = {"."};
        testLsWithFiles(dot, files, Path.Join(absolutePath, dir));
        
        // TODO: вернуться назад и удалить созданную директорию
        removeTestDir(Path.Join(absolutePath, dir));
    }

    private void createTestDir(string dir, string[] files)
    {
        DirectoryInfo createdDir = Directory.CreateDirectory(dir);
        foreach (string file in files)
        {
            var filePath = Path.Join(createdDir.FullName, file);
            File.Create(filePath);
        }
    }

    private void testLsWithFiles(string[] args , string[] files, string startDir = "")
    {
        // somehow change current working directory
        ShellEnvironment shellEnvironment = new ShellEnvironment();
        shellEnvironment["PWD"] = startDir;
        Directory.SetCurrentDirectory(startDir);
        
        // creating and running the command
        var stream = new MemoryStream();
        var streamWriter = new StreamWriter(stream);
        
        var ls = new LsCommand(new StreamReader(new MemoryStream()),
            streamWriter, shellEnvironment);

        string expectedString = String.Join(" ", files);
        Assert.Equal(expectedString, RemoveSpaces(
            extractOutput(ls, args, stream)));
    }

    private void removeTestDir(string dir)
    {
        Directory.Delete(dir);
    }
}

#endif
