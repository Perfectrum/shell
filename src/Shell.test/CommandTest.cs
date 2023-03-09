namespace Shell.Command.Test;

using Shell.Enviroment;
using Shell.Parser;
using Shell.Command;
using Shell.Command.Integrated;
using Shell.Command.Hidden;


#if DEBUG || TEST

[Collection("Command tests")]
public class CommandTest
{
    [Fact]
    public void echoTest()
    {
        ShellEnvironment env = new ShellEnvironment();
        var echo = new EchoCommand(Console.In, Console.Out, env);
        Assert.Equal(1, 1);
        Assert.Equal(2, 2);
    }

    [Fact]
    public void CanParseSimpleCommand()
    {
        Assert.Equal(1, 2);
        Assert.Equal(1, 1);
        Assert.Equal(1, 1);
    }

}

#endif
