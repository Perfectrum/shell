
namespace Shell.Interpeter.Test;

using Shell.Enviroment;
using Shell.Parser;
using Shell.Command;
using Shell.Command.Integrated;
using Shell.Command.Hidden;
using System.IO;
using System;


#if DEBUG || TEST

public class InterpeterTest {
    [Fact]
    public void InterpeterTest1()
    {
        var interpreter = new Interpreter();
        var res = interpreter.ProcessRequest("echo 2141242");
        Assert.Equal(Status.Ok, res.State);
        
        res = interpreter.ProcessRequest("x=15");
        Assert.Equal(Status.Empty, res.State);

        res = interpreter.ProcessRequest("cat dontexist");
        Assert.Equal(Status.Ok, res.State);

        res = interpreter.ProcessRequest("Command that don't exits");
        Assert.Equal(Status.Error, res.State);

        res = interpreter.ProcessRequest("CatCommand that don't exits");
        Assert.Equal(Status.Error, res.State);

        res = interpreter.ProcessRequest("exit");
        Assert.Equal(Status.Exit, res.State);
    }

}

#endif