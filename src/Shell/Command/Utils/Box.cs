using Shell.Command;
using Shell;

/// <summary>
///     `Box` - класс, инкапсулирующий исполнение.
/// </summary>
public class Box
{
    public Task<int> Process { get; private set; }
    public Result<string>? Effect => ResultFactory.Join(_commands.Select(x => x.ShellSideEffect).ToList());

    private List<Command> _commands;

    /// <summary>
    ///     `Box` - класс, инкапсулирующий исполнение.
    ///     Создаёт объект класса Box.
    /// </summary>
    public Box(Task<int> process, Command orig)
    {
        Process = process;
        _commands = new List<Command>() { orig };
    }
    
    /// <summary>
    ///     `Box` - класс, инкапсулирующий исполнение.
    ///     Создаёт объект класса Box.
    /// </summary>
    public Box(List<Task<int>> processes, List<Command> cmnds)
    {
        Process = Task.WhenAll(processes.ToArray()).ContinueWith(x => x.Result.FirstOrDefault(x => x != 0, 0));
        _commands = cmnds;
    }
}


