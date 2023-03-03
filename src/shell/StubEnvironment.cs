using System.Collections;

namespace shell;

public class StubEnvironment : IEnvironment
{

    IDictionary _data = Environment.GetEnvironmentVariables();

    public StubEnvironment()
    {
        Console.WriteLine(String.Format("Loaded " + _data.Count + " environment variables"));
    }

public string Get(string key)
{
    string? val = (string?)_data[key];
    if (val == null)
    {
        return "";
    }
    return (string)_data[key]!;
}

    public void Set(string key, string value)
    {
        _data.Add(key, value);
    }
}