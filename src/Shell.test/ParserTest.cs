namespace Shell.Parser.Test;

using Shell.Parser;
using Shell.Enviroment;
using Shell.Expression;
using System.Linq;

[Collection("Parser tests")]
public class ParserTest
{
    public ParseAutomaton Automaton { get; private set; }

    public ParserTest()
    {
        Automaton = new ParseAutomaton();
    }

    [Fact]
    public void CanParseSimpleCommand()
    {
        var s = Automaton.Run("echo hi");

        Assert.Single(s);
        Assert.Equal("[CMD:  'echo' hi]", s.FirstOrDefault()?.ToDebugString());
    }

    [Fact]
    public void CanParseOneAssignment()
    {
        var s = Automaton.Run("x=3");

        Assert.Single(s);
        Assert.Equal("[ASS 'x'='3']", s.FirstOrDefault()?.ToDebugString());
    }

    [Fact]
    public void CanParseManyAssignments()
    {
        var s = Automaton.Run("x=4 y=2 r=m r=f");

        Assert.Single(s);
        Assert.Equal(
            "[ASS 'x'='4', 'y'='2', 'r'='m', 'r'='f']",
            s.FirstOrDefault()?.ToDebugString()
        );
    }

    [Fact]
    public void CanParseCommandWithArgs()
    {
        var s = Automaton.Run("rm -rf");

        Assert.Single(s);
        Assert.Equal("[CMD:  'rm' -rf]", s.FirstOrDefault()?.ToDebugString());
    }

    [Fact]
    public void CanParseCommandWithManyArgs()
    {
        var s = Automaton.Run("ls -l -h -a");

        Assert.Single(s);
        Assert.Equal("[CMD:  'ls' -l, -h, -a]", s.FirstOrDefault()?.ToDebugString());
    }

    [Fact]
    public void CanParseCommandWithOneAssignment()
    {
        var s = Automaton.Run("x=3 cmnd");

        Assert.Single(s);
        Assert.Equal("[CMD: [ASS 'x'='3'] 'cmnd' ]", s.FirstOrDefault()?.ToDebugString());
    }

    [Fact]
    public void CanParseCommandWithOneManyAssignments()
    {
        var s = Automaton.Run("x=4 y=2 cmnd");

        Assert.Single(s);
        Assert.Equal("[CMD: [ASS 'x'='4', 'y'='2'] 'cmnd' ]", s.FirstOrDefault()?.ToDebugString());
    }

    [Fact]
    public void CanParseCommandWithOneManyAssignmentsAndManyArguments()
    {
        var s = Automaton.Run("x=4 y=2 cmnd a b c d");

        Assert.Single(s);
        Assert.Equal(
            "[CMD: [ASS 'x'='4', 'y'='2'] 'cmnd' a, b, c, d]",
            s.FirstOrDefault()?.ToDebugString()
        );
    }

    [Fact]
    public void CanParseOneVariable()
    {
        var s = Automaton.Run("$x");

        Assert.Single(s);
        Assert.Equal("[TMP: '{$$$}' $x ]", s.FirstOrDefault()?.ToDebugString());
    }

    [Fact]
    public void CanParseManyVariables()
    {
        var s = Automaton.Run("x=$y $a$b $c $d");

        Assert.Single(s);
        Assert.Equal(
            "[TMP: 'x={$$$} {$$$}{$$$} {$$$} {$$$}' $y, $a, $b, $c, $d ]",
            s.FirstOrDefault()?.ToDebugString()
        );
    }

    [Fact]
    public void CanParseStrongQuoteSimple()
    {
        var s = Automaton.Run("'heh'");

        Assert.Single(s);
        Assert.Equal("[CMD:  'heh' ]", s.FirstOrDefault()?.ToDebugString());
    }

    [Fact]
    public void CanParseStrongQuoteHard()
    {
        var s = Automaton.Run("x='oo \\' oo' a'b''c d e'f 'g'");

        Assert.Single(s);
        Assert.Equal(
            "[CMD: [ASS 'x'='oo ' oo'] 'abc d ef' g]",
            s.FirstOrDefault()?.ToDebugString()
        );
    }

    [Fact]
    public void CanParseWeakQuoteSimple()
    {
        var s = Automaton.Run("\"heh\"");

        Assert.Single(s);
        Assert.Equal("[CMD:  'heh' ]", s.FirstOrDefault()?.ToDebugString());
    }

    [Fact]
    public void CanParseWeakQuoteHard()
    {
        var s = Automaton.Run("x=\"oo \\\" oo\" a\"b\"\"c d e\"f \"g\"");

        Assert.Single(s);
        Assert.Equal(
            "[CMD: [ASS 'x'='oo \" oo'] 'abc d ef' g]",
            s.FirstOrDefault()?.ToDebugString()
        );
    }
}
