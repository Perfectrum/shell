namespace Shell.Command;

using Shell.Enviroment;
using System.IO;

using Shell;


/// <summary>
/// Метод создания объектов-команд. 
/// </summary>
public delegate Command CmndCreator(TextReader r, TextWriter w, ShellEnvironment e);


/// <summary>
/// Класс CommandResolver хранит набор известных (стандартных) команд.
/// </summary>
public static class CommandResolver
{

    private static Dictionary<string, CmndCreator> _builtInCommands =
        new Dictionary<string, CmndCreator>();

    
    /// <summary>
    /// Сохранить создателя команд
    /// </summary>
    public static void RegisterBuiltIn(string name, CmndCreator creator)
    {
        _builtInCommands[name] = creator;
    }

    /// <summary>
    /// Ищет команду, результат поиска передаёт в обертку Result.
    /// </summary>
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
