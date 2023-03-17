namespace Shell.Enviroment;

using System.Collections;
using System.Collections.Generic;

public class ShellEnvironment : IEnumerable<KeyValuePair<string, string>>
{
    private IDictionary<string, string> _data;
    private ShellEnvironment? _parent;

    public ShellEnvironment()
    {
        _parent = null;
        _data = new Dictionary<string, string>();
        // Hashtable
        var x = System.Environment.GetEnvironmentVariables();
        foreach (DictionaryEntry i in x)
        {
            if (i.Key is string && i.Value is string)
            {
                _data[(string)i.Key] = (string)i.Value;
            }
        }
    }

    private ShellEnvironment(ShellEnvironment parent)
    {
        _parent = parent;
        _data = new Dictionary<string, string>();
    }

    public ShellEnvironment CreateView()
    {
        return new ShellEnvironment(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        foreach (var item in _data)
        {
            yield return item;
        }
        if (_parent != null)
        {
            foreach (var item in _parent)
            {
                yield return item;
            }
        }
    }

    public string this[string name]
    {
        get
        {
            if (!_data.ContainsKey(name))
            {
                return _parent?[name] ?? "";
            }
            return _data[name];
        }
        set { _data[name] = value; }
    }
}
