namespace Shell.Command;
using Shell.Enviroment;
using Shell.Command.Utils;

using System.Diagnostics;

public class ForeignCommand : Command
{
    private string _commandName;

    public ForeignCommand(string name, TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e)
    {
        _commandName = name;
    }

    protected override int Go(string[] a)
    {
        return 0;
    }

    public override Task<int> Run(string[] args)
    {
        Process cmnd = new Process();
        cmnd.StartInfo.FileName = _commandName;

        foreach (var a in args)
        {
            cmnd.StartInfo.ArgumentList.Add(a);
        }
        foreach (var item in Env)
        {
            cmnd.StartInfo.EnvironmentVariables[item.Key] = item.Value;
        }

        cmnd.StartInfo.UseShellExecute = false;

        cmnd.StartInfo.RedirectStandardInput = false;
        cmnd.StartInfo.RedirectStandardOutput = false;

        cmnd.Start();

        var cmndAwaiter = cmnd.WaitForExitAsync();

        /*
        StreamPiper in2in = new StreamPiper(StdIn, cmnd.StandardInput, cmndAwaiter);
        StreamPiper out2out = new StreamPiper(cmnd.StandardOutput, StdOut, cmndAwaiter);


        var inAwaiter = in2in.Pipe(false, true, true);
        var outAwaiter =  out2out.Pipe(true, false);

        */
        return cmndAwaiter.ContinueWith((x) => cmnd.ExitCode);
    }
}
