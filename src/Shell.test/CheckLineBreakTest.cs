namespace Shell.UI.Test;

using Shell;

public class CheckLineBreakTest
{
    private UI ui = new UI();

    [Fact]
    public void EndsWithBackslash()
    {
        Assert.Equal(expected: UI.State.Backslash, actual: ui.CheckLineBreak("\\"));
        Assert.Equal(expected: UI.State.Normal, actual: ui.CheckLineBreak("\\ "));

        Assert.Equal(expected: UI.State.Normal, actual: ui.CheckLineBreak("\\\\"));
        Assert.Equal(expected: UI.State.Backslash, actual: ui.CheckLineBreak("\\ \\"));

        Assert.Equal(expected: UI.State.Backslash, actual: ui.CheckLineBreak("\\\\ \\"));
        Assert.Equal(expected: UI.State.Normal, actual: ui.CheckLineBreak("\\ \\\\"));
    }

    [Fact]
    public void SingleQuotes()
    {
        Assert.Equal(expected: UI.State.SingleQuote, actual: ui.CheckLineBreak("'"));
        Assert.Equal(expected: UI.State.Normal, actual: ui.CheckLineBreak("''"));
        Assert.Equal(expected: UI.State.SingleQuote, actual: ui.CheckLineBreak("'''"));
        Assert.Equal(expected: UI.State.Normal, actual: ui.CheckLineBreak("'smth'"));
        Assert.Equal(expected: UI.State.SingleQuote, actual: ui.CheckLineBreak("'smth"));
        Assert.Equal(expected: UI.State.SingleQuote, actual: ui.CheckLineBreak("smth'"));
    }

    [Fact]
    public void DoubleQuotes()
    {
        Assert.Equal(expected: UI.State.DoubleQuote, actual: ui.CheckLineBreak("\""));
        Assert.Equal(expected: UI.State.Normal, actual: ui.CheckLineBreak("\"\""));
        Assert.Equal(expected: UI.State.DoubleQuote, actual: ui.CheckLineBreak("\"\"\""));
        Assert.Equal(expected: UI.State.Normal, actual: ui.CheckLineBreak("\"smth\""));
        Assert.Equal(expected: UI.State.DoubleQuote, actual: ui.CheckLineBreak("\"smth"));
        Assert.Equal(expected: UI.State.DoubleQuote, actual: ui.CheckLineBreak("smth\""));
    }

    [Fact]
    public void QoutesMix()
    {
        Assert.Equal(expected: UI.State.Normal,actual: ui.CheckLineBreak("\"'\""));
        Assert.Equal(expected: UI.State.Normal, actual: ui.CheckLineBreak("'\"'"));
        Assert.Equal(expected: UI.State.SingleQuote, actual: ui.CheckLineBreak("'\""));
        Assert.Equal(expected: UI.State.DoubleQuote, actual: ui.CheckLineBreak("\"'"));
        Assert.Equal(expected: UI.State.Normal, actual: ui.CheckLineBreak(" '\"'\"'\" "));
        Assert.Equal(expected: UI.State.DoubleQuote, actual: ui.CheckLineBreak("\"'\"'\"'\""));
    }

    [Fact]
    public void BackslahQuotesMix()
    {
        Assert.Equal(expected: UI.State.Normal, actual: ui.CheckLineBreak("\\\""));
        Assert.Equal(expected: UI.State.Normal, actual: ui.CheckLineBreak("\\'"));
        Assert.Equal(expected: UI.State.SingleQuote, actual: ui.CheckLineBreak("'\\"));
        Assert.Equal(expected: UI.State.Backslash, actual: ui.CheckLineBreak("''\\"));
        Assert.Equal(expected: UI.State.Backslash, actual: ui.CheckLineBreak("\"\"\\"));
        Assert.Equal(expected: UI.State.Normal, actual: ui.CheckLineBreak("'\\'"));
    }
}