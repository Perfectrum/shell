namespace Shell.Command;

using Shell.Enviroment;
using System.IO;

using Shell;

public static class CommandResolver
{
    public delegate Command BuiltInCmndCreator(TextReader r, TextWriter w, ShellEnvironment e);
    public delegate InternalCommand InternalCmndCreator();

    private static Dictionary<string, BuiltInCmndCreator> _builtInCommands =
        new Dictionary<string, BuiltInCmndCreator>();
    private static Dictionary<string, InternalCmndCreator> _internalCommands =
        new Dictionary<string, InternalCmndCreator>();

    public static void RegisterBuiltIn(string name, BuiltInCmndCreator creator)
    {
        _builtInCommands[name] = creator;
    }

    public static void RegisterInternal(string name, InternalCmndCreator creator)
    {
        _internalCommands[name] = creator;
    }

    public static Result<Task<int>> StartCommand(string name, string[] args, ShellEnvironment env)
    {
        if (_internalCommands.ContainsKey(name))
        {
#if DEBUG
            Console.WriteLine($"- [INTERNAL START] ---> {name}");
#endif
            return _internalCommands[name]
                .Invoke()
                .Process(args, env)
                .Map<Task<int>>(
                    (s) =>
                        Task.Run(() =>
                        {
                            Console.WriteLine(s);
                            return 0;
                        })
                );
        }

        if (_builtInCommands.ContainsKey(name))
        {
#if DEBUG
            Console.WriteLine($"- [BUILTIN START] ---> {name}");
#endif
            return ResultFactory.CreateResult<Task<int>>(
                _builtInCommands[name].Invoke(Console.In, Console.Out, env).Run(args)
            );
        }

        var path = $"{env["PWD"]}:{env["PATH"]}";
        var paths = path.Split(':');

        foreach (var p in paths)
        {
            var file = Path.Combine(p, name);
            if (File.Exists(file))
            {
#if DEBUG
                Console.WriteLine($"- [FOREIGN START] ---> {new FileInfo(file).FullName}");
#endif
                var cmnd = new ForeignCommand(
                    new FileInfo(file).FullName,
                    Console.In,
                    Console.Out,
                    env
                );
                return ResultFactory.CreateResult<Task<int>>(cmnd.Run(args));
            }
        }

        return ResultFactory.CreateError<Task<int>>($"Command '{name}' not found!");
    }
}
