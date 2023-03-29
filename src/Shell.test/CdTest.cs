namespace Shell.Parser.Test;

using Shell.Command.Hidden;
using Shell.Command;
using Shell.Enviroment;

#if DEBUG || TEST

[Collection("Parser tests")]
public class CdTest
{
    [Fact]
    public void Point()
    {
        var stream = new MemoryStream();
        var streamWriter = new StreamWriter(stream);
        var cd = new CdCommand(new StreamReader(new MemoryStream()),
            streamWriter, new ShellEnvironment());
        var args = new string[]{"./"};
        var pathExpected = cd.Env["PWD"];
        cd.Run(args).Wait();
        Assert.Equal(pathExpected, cd.Env["PWD"]);
    }
    
    [Fact]
    public void Slash()
    {
        var stream = new MemoryStream();
        var streamWriter = new StreamWriter(stream);
        var cd = new CdCommand(new StreamReader(new MemoryStream()),
            streamWriter, new ShellEnvironment());
        var pathExpected = "/";
        var args = new string[]{"/"};
        cd.Run(args).Wait();
        Assert.Equal(pathExpected, cd.Env["PWD"]);
    }
    
    [Fact]
    public void NoExists()
    {
        var stream = new MemoryStream();
        var streamWriter = new StreamWriter(stream);
        var cd = new CdCommand(new StreamReader(new MemoryStream()),
            streamWriter, new ShellEnvironment());
        var pathExpected = cd.Env["PWD"];
        var args = new string[]{"/noexists"};
        cd.Run(args).Wait();
        Assert.Equal(pathExpected, cd.Env["PWD"]);
    }
    
    [Fact]
    public void UpDirectory()
    {
        var stream = new MemoryStream();
        var streamWriter = new StreamWriter(stream);
        var cd = new CdCommand(new StreamReader(new MemoryStream()),
            streamWriter, new ShellEnvironment());
        var pathExpected = Path.GetFullPath(Path.Combine(cd.Env["PWD"], "./../")).TrimEnd('/');
        var args = new string[]{"./../"};
        cd.Run(args).Wait();
        Assert.Equal(pathExpected, cd.Env["PWD"]);
    }
}
#endif
