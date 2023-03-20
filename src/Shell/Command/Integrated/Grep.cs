using Shell.Enviroment;
using System.CommandLine;
using CliCommand = System.CommandLine.Command;

namespace Shell.Command.Integrated;

public class GrepCommand : Command
{
    public GrepCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }

    protected override int Go(string[] args)
    {
        var patternArgument = new Argument<string>(name: "pattern");
        var wordOption = new Option<bool>(name: "-w");
        var caseOption = new Option<bool>(name: "-i");
        var numOption = new Option<int>(name: "-A");

        var rootCommand = new RootCommand
        {
            patternArgument,
            wordOption,
            caseOption,
            numOption
        };

        rootCommand.SetHandler((
            wordOptionValue,
            caseOptionValue,
            numOptionValue,
            patternArgumentValue) => {
                Console.WriteLine($"pattern: {patternArgumentValue}; w = {wordOptionValue}; i = {caseOptionValue}; A = {numOptionValue}");
            }, wordOption, wordOption, numOption, patternArgument);

        return rootCommand.InvokeAsync(args).Result;
    }
}