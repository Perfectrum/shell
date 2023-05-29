namespace Shell.Expression;

using System.Collections.Generic;
using Shell.Command;
using Shell.Enviroment;
using Shell;


/// <summary>
///     Класс PipeExpression отражает выражение, соответствующее
///     пайплайну.
/// </summary>
public class PipeExpression : Expression
{
    List<CommandExpression> _commands;

    
    /// <summary>
    ///     Класс PipeExpression отражает выражение, соответствующее
    ///     пайплайну.
    ///     Создаёт объект класса PipeExpression.
    /// </summary>
    public PipeExpression(CommandExpression left, CommandExpression right)
    {
        _commands = new List<CommandExpression>() { left, right };
    }

    /// <summary>
    ///     Класс PipeExpression отражает выражение, соответствующее
    ///     пайплайну.
    ///     Создаёт объект класса PipeExpression.
    /// </summary>
    public PipeExpression(List<Expression> commands)
    {
        _commands = commands.Select(x => (CommandExpression)x).ToList();
    }

    private ShellEnvironment createEnv(CommandExpression cmnd, ShellEnvironment env)
    {

        var e = env.CreateView();

        foreach (var v in cmnd.Vars)
        {
            e[v.Name] = v.Value;
        }

        return e;
    }

    /// <summary>
    ///     Конструирует объект для исполнения, соответствующий пайплайну,
    ///     команд, обёрнутый в объект-результат.
    /// </summary>
    public override Result<Box> Run(ShellEnvironment env)
    {
        List<Result<CmndCreator>> creators = _commands.Select(x => CommandResolver.FindCommand(x.Command, env)).ToList();

        return ResultFactory.Traverse(creators).Map((list) =>
        {

            List<Task<int>> processes = new List<Task<int>>();
            List<Command> commands = new List<Command>();

            CmndCreator first = list.First();

            Command prev = first.Invoke(Console.In, new StreamWriter(new MemoryStream()), env);
            commands.Add(prev);

            for (int i = 1; i < list.Count; ++i)
            {
                CmndCreator currCreator = list[i];
                Command currInstance = currCreator.Invoke(
                    new StreamReader(new MemoryStream()),
                    ((i != (list.Count - 1)) ? (new StreamWriter(new MemoryStream())) : Console.Out),
                    createEnv(_commands[i], env)
                );

                prev.OutputDataRecived += (t) => currInstance.WriteToCommand(t.Text);
                prev.CommandFinished += (c) => currInstance.CloseStdIn();

                commands.Add(currInstance);

                prev = currInstance;
            }

            Task<int>? prevProc = null;
            for (int i = 0; i < commands.Count; ++i)
            {
                Command cmnd = commands[i];
                Task<int> currProc = cmnd.Run(_commands[i].Args.ToArray(), prevProc, i == (list.Count - 1));

                processes.Add(currProc);
                prevProc = currProc;
            }

            return new Box(processes, commands);
        });
    }
}
