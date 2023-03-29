namespace Shell.Command;

using Shell;
using Shell.Enviroment;
using Shell.Command.Utils;

public class CommandOutputEvent : EventArgs
{
    public string? Text { get; private set; }

    public CommandOutputEvent(string text)
    {
        Text = text;
    }

    public CommandOutputEvent(TextRecivedEvent e)
    {
        Text = e.Text;
    }
}

public class CommandFinishedEvent : EventArgs
{
    public int Code { get; private set; }

    public CommandFinishedEvent(int code)
    {
        Code = code;
    }
}

public abstract class Command
{
    protected StreamWriter _stdin;
    private bool _unsafeMode = false;
    private Result<string>? _sideEffect = null;
    public Result<string>? ShellSideEffect
    {
        get => _sideEffect;
        protected set
        {
            if (_unsafeMode)
            {
                _sideEffect = value;
            }
        }
    }
    public delegate void CommandOutputHandler(CommandOutputEvent e);
    public delegate void CommandFinishedHandler(CommandFinishedEvent e);
    public event CommandOutputHandler? OutputDataRecived;
    public event CommandFinishedHandler? CommandFinished;
    public TextReader StdIn { get; protected set; }
    public EventTextWriter StdOut { get; protected set; }

    public ShellEnvironment Env { get; protected set; }

    public int? Code { get; protected set; }

    public Command(TextReader cin, TextWriter cout, ShellEnvironment env) : this(cin, new EventTextWriter(cout), env) { }

    public Command(TextReader cin, EventTextWriter cout, ShellEnvironment env)
    {
        StdIn = cin;
        StdOut = cout;
        StdOut.OnTextRecived += (e) => OutputDataRecived?.Invoke(new CommandOutputEvent(e));
        Env = env;
        _stdin = new StreamWriter(new MemoryStream());
    }

    protected abstract int Go(string[] args);


    public virtual Task<int> Run(string[] args, Task<int>? previous = null, bool last = false)
    {
        var job = previous;
        if (job == null)
        {
            job = Task.Run(() => this.Go(args));
        }
        else
        {
            job.ContinueWith((e) =>
            {
                this._stdin.Flush();
                this._stdin.BaseStream.Position = 0;
                this.StdIn = new StreamReader(_stdin.BaseStream);
                return this.Go(args);
            });
        }
        return job.ContinueWith((x) =>
        {
            CommandFinished?.Invoke(new CommandFinishedEvent(x.Result));
            return x.Result;
        });
    }

    protected void FireFinished(int code)
    {
        CommandFinished?.Invoke(new CommandFinishedEvent(code));
    }

    public virtual void CloseStdIn() { }

    public virtual void WriteToCommand(string? msg)
    {
        if (msg == null)
        {
            _stdin.Flush();
            return;
        }
        _stdin.Write(msg);
    }

    protected void ByCallThisFunctionIConfirmThatThisFunctionChangeBashStateAndThisIsUnsafeActualy()
    {
        _unsafeMode = true;
        Env = Env.GetUnsafeShellEnv();
    }
}
