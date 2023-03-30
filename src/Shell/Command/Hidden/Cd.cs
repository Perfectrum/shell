using Shell.Enviroment;

namespace Shell.Command.Hidden;

public class CdCommand : Command
{
    public CdCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }
    /// <summary>
    /// Change working directory.
    /// </summary>
    /// <param name="args">argument list.</param>
    /// <returns>return code.</returns>
    protected override int Go(string[] args)
    {
        base.ByCallThisFunctionIConfirmThatThisFunctionChangeBashStateAndThisIsUnsafeActualy();
        var arg = args.Length == 0 ? "." : args[0];
        var path = arg[0] != '/' ? Path.GetFullPath(Path.Combine(Env["PWD"] + "/", arg)).TrimEnd('/') : arg;
        var returnCode = 0;
        if (Directory.Exists(path)) 
        {
            Env["PWD"] = path;
        }
        else
        {
            ShellSideEffect = ResultFactory.CreateError<string>("bash: cd: No such file or directory");
            returnCode = 1;
        }
        return returnCode;
    }
}
