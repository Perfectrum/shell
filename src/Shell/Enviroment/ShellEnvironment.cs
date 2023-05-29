namespace Shell.Enviroment;

using System.Collections;
using System.Collections.Generic;

/// <summary>
///     ShellEnvironment - класс,
///     инкапсулирующий значения переменных 
///     в языке приложения. 
/// </summary>

public class ShellEnvironment : IEnumerable<KeyValuePair<string, string>>
{
    private IDictionary<string, string> _data;
    private ShellEnvironment? _parent;

    /// <summary>
    ///     ShellEnvironment - класс,
    ///     инкапсулирующий значения переменных 
    ///     в языке приложения. 
    ///     Создаёт объект класса ShellEnvironment.
    /// </summary>
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

    /// <summary>
    ///     Создаёт окружение-view,
    ///     то есть - копию окружения.
    /// </summary>
    public ShellEnvironment CreateView()
    {
        return new ShellEnvironment(this);
    }

    /// <summary>
    ///     Получить enumerator коллекции.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    ///     Возвращает самое внешнее
    ///     - основное окружение программы. 
    /// </summary>
    public ShellEnvironment GetUnsafeShellEnv()
    {
        if (_parent == null)
        {
            return this;
        }
        return _parent.GetUnsafeShellEnv();
    }

    /// <summary>
    ///     Получить enumerator коллекции.
    /// </summary>
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

    /// <summary>
    ///     Получить значение переменной.
    /// </summary>
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
