using System.Collections;

namespace shell.enviroment;

public class ShellEnvironment
{
    private IDictionary<string, string> _data;
    private ShellEnvironment? _parent;

    public ShellEnvironment()
    {
        _parent = null;
        _data = new Dictionary<string, string>();
        // Hashtable
        var x = Environment.GetEnvironmentVariables();
        foreach (DictionaryEntry i in x) {
            if (i.Key is string && i.Value is string) {
                _data[(string)i.Key] = (string)i.Value;
            }
        }

        #if DEBUG
        Console.WriteLine($"Loaded {_data.Count} variables!");
        #endif
    }

    private ShellEnvironment(ShellEnvironment parent) {
        _parent = parent;
        _data = new Dictionary<string, string>();
    }

    public ShellEnvironment CreateView() {
        return new ShellEnvironment(this);
    }

    public string this[string name] {
        get {
            if (!_data.ContainsKey(name)) {
                return _parent?[name] ?? "";
            }
            return _data[name];
        }
        set {
            _data[name] = value;
        }
    }
}