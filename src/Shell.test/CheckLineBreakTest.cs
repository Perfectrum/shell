namespace Shell.UI.Test;

using Shell;

public class CheckLineBreakTest
{
    private UI ui = new UI();

    [Fact]
    public void EndsWithBackslash()
    {
        Assert.Equal(ui.CheckLineBreak("\\"), UI.State.Backslash);
        Assert.Equal(ui.CheckLineBreak("\\ "), UI.State.Normal);

        Assert.Equal(ui.CheckLineBreak("\\\\"), UI.State.Normal);
        Assert.Equal(ui.CheckLineBreak("\\ \\"), UI.State.Backslash);

        Assert.Equal(ui.CheckLineBreak("\\\\ \\"), UI.State.Backslash);
        Assert.Equal(ui.CheckLineBreak("\\ \\\\"), UI.State.Normal);
    }

    [Fact]
    public void SingleQuotes()
    {
        Assert.Equal(ui.CheckLineBreak("'"), UI.State.SingleQuote);
        Assert.Equal(ui.CheckLineBreak("''"), UI.State.Normal);
        Assert.Equal(ui.CheckLineBreak("'''"), UI.State.SingleQuote);
        Assert.Equal(ui.CheckLineBreak("'smth'"), UI.State.Normal);
        Assert.Equal(ui.CheckLineBreak("'smth"), UI.State.SingleQuote);
        Assert.Equal(ui.CheckLineBreak("smth'"), UI.State.SingleQuote);
    }

    [Fact]
    public void DoubleQuotes()
    {
        Assert.Equal(ui.CheckLineBreak("\""), UI.State.DoubleQuote);
        Assert.Equal(ui.CheckLineBreak("\"\""), UI.State.Normal);
        Assert.Equal(ui.CheckLineBreak("\"\"\""), UI.State.DoubleQuote);
        Assert.Equal(ui.CheckLineBreak("\"smth\""), UI.State.Normal);
        Assert.Equal(ui.CheckLineBreak("\"smth"), UI.State.DoubleQuote);
        Assert.Equal(ui.CheckLineBreak("smth\""), UI.State.DoubleQuote);
    }

    [Fact]
    public void QoutesMix()
    {
        Assert.Equal(ui.CheckLineBreak("\"'\""), UI.State.Normal);
        Assert.Equal(ui.CheckLineBreak("'\"'"), UI.State.Normal);
        Assert.Equal(ui.CheckLineBreak("'\""), UI.State.SingleQuote);
        Assert.Equal(ui.CheckLineBreak("\"'"), UI.State.DoubleQuote);
        Assert.Equal(ui.CheckLineBreak(" '\"'\"'\" "), UI.State.Normal);
        Assert.Equal(ui.CheckLineBreak("\"'\"'\"'\""), UI.State.DoubleQuote);
    }

    [Fact]
    public void BackslahQuotesMix()
    {
        Assert.Equal(ui.CheckLineBreak("\\\""), UI.State.Normal);
        Assert.Equal(ui.CheckLineBreak("\\'"), UI.State.Normal);
        Assert.Equal(ui.CheckLineBreak("'\\"), UI.State.SingleQuote);
        Assert.Equal(ui.CheckLineBreak("''\\"), UI.State.Backslash);
        Assert.Equal(ui.CheckLineBreak("\"\"\\"), UI.State.Backslash);
        Assert.Equal(ui.CheckLineBreak("'\\'"), UI.State.Normal);
    }
}