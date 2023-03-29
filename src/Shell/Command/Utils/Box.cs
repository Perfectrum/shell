using Shell.Command;
using Shell;

public class Box
{
    public Task<int> Process { get; private set; }
    public Result<string>? Effect => ResultFactory.Join(_commands.Select(x => x.ShellSideEffect).ToList());

    private List<Command> _commands;

    public Box(Task<int> process, Command orig)
    {
        Process = process;
        _commands = new List<Command>() { orig };
    }

    public Box(List<Task<int>> processes, List<Command> cmnds)
    {
        Process = Task.WhenAll(processes.ToArray()).ContinueWith(x => x.Result.FirstOrDefault(x => x != 0, 0));
        _commands = cmnds;
    }
}


