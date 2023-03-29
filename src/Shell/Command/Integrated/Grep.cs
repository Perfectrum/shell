using Shell.Enviroment;
using System.CommandLine;
using System.Text.RegularExpressions;

namespace Shell.Command.Integrated;

public class GrepCommand : Command
{
    public GrepCommand(TextReader i, TextWriter o, ShellEnvironment e)
        : base(i, o, e) { }

    protected override int Go(string[] args)
    {
        var patternArgument = new Argument<string>(name: "pattern");
        var inputArgument = new Argument<string>(name: "input");
        var wordOption = new Option<bool>(name: "-w");
        var caseOption = new Option<bool>(name: "-i");
        var numOption = new Option<int>(name: "-A");

        var rootCommand = new RootCommand
        {
            patternArgument,
            inputArgument,
            wordOption,
            caseOption,
            numOption
        };

        rootCommand.SetHandler( (
            wordOptionValue,
            caseOptionValue,
            numOptionValue,
            patternArgumentValue,
            inputArgumentValue) => {
                if ( !File.Exists(inputArgumentValue) )
                {
                    Console.WriteLine($"File {inputArgumentValue} not found.");
                    return;
                }

                var ignoreCase = caseOptionValue ? RegexOptions.IgnoreCase : RegexOptions.None;
                var count = 0;
                
                if (wordOptionValue)  {  patternArgumentValue = ' ' + patternArgumentValue + ' ';  }
                    
                patternArgumentValue = ".*" + patternArgumentValue + ".*";
                
#if DEBUG
                Console.WriteLine($"GREP DEBUG: regex = {patternArgumentValue}; -w = {wordOptionValue}; -i = {caseOptionValue}; -A = {numOptionValue}");
#endif
                //FIXME: StdIn.ReadLine()
                foreach (var line in File.ReadLines(inputArgumentValue))
                {
                    if (count > 0)
                    {
                        StdOut.WriteLine(line);
                        count--;
                        continue;
                    }
                    
                    if (Regex.Matches(line, @patternArgumentValue, ignoreCase).Count > 0)
                    {
                        count = numOptionValue;
                        StdOut.WriteLine(line);
                    }
                }
        }, wordOption, caseOption, numOption, patternArgument, inputArgument);

        return rootCommand.Invoke(args);
    }
}