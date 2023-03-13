namespace Shell.Command.Utils;

public class StreamPiper
{
    private const int BUFFER_SIZE = 32;

    private TextReader _sr;
    private TextWriter _sw;
    private Task _process;

    public StreamPiper(TextReader sr, TextWriter sw, Task process)
    {
        _sr = sr;
        _sw = sw;
        _process = process;
    }

    public Task Pipe(bool closeIn = false, bool closeOut = false, bool closeOnProcessEnd = false)
    {
        var job = Task.Run(() =>
        {
            char[] buffer = new char[BUFFER_SIZE];
            bool firstTime = true;

            int size = 0;
            while (firstTime || size > 0)
            {
                if (!firstTime)
                {
                    _sw.Write(buffer, 0, size);
                }
                size = _sr.ReadBlock(buffer, 0, BUFFER_SIZE);
                firstTime = false;
            }
            if (closeIn)
            {
                _sr.Close();
            }
            if (closeOut)
            {
                _sw.Close();
            }
        });

        return closeOnProcessEnd ? Task.WhenAny(new Task[] { _process, job }) : job;
    }
}
