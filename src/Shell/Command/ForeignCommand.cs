namespace Shell.Command;
using Shell.Enviroment;
using Shell.Command.Utils;

using System.Diagnostics;

public class ForeignCommand : Command
{
    private bool _hasStarted = false;
    private bool _shouldFlash = false;
    private bool _stdInClosed = false;
    private readonly object _stdInWriteLock = new object();
    private Process? _cmnd = null;

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

    public override Task<int> Run(string[] args, Task<int>? e = null, bool last = false)
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

        cmnd.StartInfo.RedirectStandardInput = e != null;
        cmnd.StartInfo.RedirectStandardOutput = !last;

        cmnd.OutputDataReceived += (o, e) => StdOut.Write(e.Data);
        cmnd.EnableRaisingEvents = true;
        cmnd.Exited += (o, e) => FireFinished(cmnd.ExitCode);

        cmnd.Start();
        _cmnd = cmnd;
        if (!last)
        {
            cmnd.BeginOutputReadLine();
        }
        lock (_stdInWriteLock)
        {
            _stdin.Flush();
            _stdin.BaseStream.Position = 0;
            StreamReader sr = new StreamReader(_stdin.BaseStream);
            if (e != null)
            {
                cmnd.StandardInput.Write(sr.ReadToEnd());
            }
            _hasStarted = true;
        }

        if (_shouldFlash)
        {
            closeInput();
        }

        var cmndAwaiter = cmnd.WaitForExitAsync();

        return cmndAwaiter.ContinueWith((x) => cmnd.ExitCode);
    }

    public override void WriteToCommand(string? msg)
    {
        lock (_stdInWriteLock)
        {
            if (!_stdInClosed)
            {
                if (_hasStarted)
                {
                    _cmnd?.StandardInput.Write(msg);
                }
                else
                {
                    _stdin.Write(msg);
                }
            }
        }
    }

    private void closeInput()
    {
        lock (_stdInWriteLock)
        {
            _stdInClosed = true;
            _cmnd?.StandardInput.Flush();
            _cmnd?.StandardInput.Close();
        }
    }

    public override void CloseStdIn()
    {
        if (_hasStarted)
        {
            Thread.Sleep(100);
            closeInput();
        }
        else
        {
            _shouldFlash = true;
        }
    }
}
