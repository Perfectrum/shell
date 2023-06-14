namespace Shell.Command.Utils;

/// <summary>
///     Класс TextRecivedEvent используется,
///     для хранения записанного в ввод строки.
/// </summary>
public class TextRecivedEvent : EventArgs
{
    public string? Text { get; private set; }
    /// <summary>
    ///     Класс TextRecivedEvent используется,
    ///     для хранения записанного в ввод строки.
    ///     Создаёт объект класса TextRecivedEvent.
    /// </summary>
    public TextRecivedEvent(string? text)
    {
        Text = text;
    }
}

    
/// <summary>
///     Класс EventTextWriter используется,
///     для записи строки в ввод команды и сохранения этой строки.
///     Создаёт объект класса EventTextWriter. 
/// </summary>
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
    
    /// <summary>
    ///     Класс EventTextWriter используется,
    ///     для записи строки в ввод команды и сохранения этой строки.
    ///     Создаёт объект класса EventTextWriter. 
    /// </summary>
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
