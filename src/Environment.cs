namespace shell;

public interface IEnvironment
{
    public string Get(string key);
    public void Set(string key, string value);
}