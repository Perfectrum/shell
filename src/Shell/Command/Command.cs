namespace Shell.Command;

using Shell;
using Shell.Enviroment;

public abstract class Command
{
    public TextReader StdIn { get; protected set; }
    public TextWriter StdOut { get; protected set; }

    public ShellEnvironment Env { get; protected set; }

    public int? Code { get; protected set; }

    public Command(TextReader cin, TextWriter cout, ShellEnvironment env)
    {
        StdIn = cin;
        StdOut = cout;
        Env = env;
    }

    protected abstract int Go(string[] args);

    public virtual Task<int> Run(string[] args)
    {
        return Task.Run(() => this.Go(args));
    }
}

public abstract class InternalCommand
{
    public abstract Result<string> Process(string[] args, ShellEnvironment env);
}
