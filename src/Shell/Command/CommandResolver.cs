namespace Shell.Command;

using Shell.Enviroment;
using System.IO;

using Shell;


public delegate Command CmndCreator(TextReader r, TextWriter w, ShellEnvironment e);
public static class CommandResolver
{

    private static Dictionary<string, CmndCreator> _builtInCommands =
        new Dictionary<string, CmndCreator>();

    public static void RegisterBuiltIn(string name, CmndCreator creator)
    {
        _builtInCommands[name] = creator;
    }

    public static Result<CmndCreator> FindCommand(string name, ShellEnvironment env)
    {
        if (_builtInCommands.ContainsKey(name))
        {
            return ResultFactory.CreateResult<CmndCreator>(_builtInCommands[name]);
        }

        var path = $"{env["PWD"]}:{env["PATH"]}";
        var paths = path.Split(':');

        foreach (var p in paths)
        {
            var file = Path.Combine(p, name);
            if (File.Exists(file))
            {
                return ResultFactory.CreateResult<CmndCreator>((r, w, e) =>
                {
                    return new ForeignCommand(
                        new FileInfo(file).FullName,
                        r, w, e
                    );
                });
            }
        }

        return ResultFactory.CreateError<CmndCreator>($"Command '{name}' not found!");

    }
}
