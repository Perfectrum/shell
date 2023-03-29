namespace Shell.Command.Utils;

public class TextRecivedEvent : EventArgs
{
    public string? Text { get; private set; }
    public TextRecivedEvent(string? text)
    {
        Text = text;
    }
}

public class EventTextWriter
{
    public delegate void TextRecivedHandler(TextRecivedEvent e);
    public event TextRecivedHandler? OnTextRecived;
    private TextWriter? _writer;
    public TextWriter? Original => _writer;
    public EventTextWriter(TextWriter writer)
    {
        _writer = writer;
    }
    public EventTextWriter() { }
    public void Write(string? text)
    {
        _writer?.Write(text);
        OnTextRecived?.Invoke(new TextRecivedEvent(text));
    }
    public void WriteLine(string line)
    {
        _writer?.WriteLine(line);
        OnTextRecived?.Invoke(new TextRecivedEvent($"{line}\n"));
    }
}
