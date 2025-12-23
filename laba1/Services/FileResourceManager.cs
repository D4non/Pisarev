using System.Text;

namespace ConsoleApp2.Services;

public class FileResourceManager : IDisposable
{
    private FileStream? _fileStream;
    private StreamWriter? _writer;
    private StreamReader? _reader;
    private bool _disposed = false;
    private readonly string _filePath;
    private readonly FileMode _fileMode;

    public FileResourceManager(string filePath, FileMode fileMode = FileMode.OpenOrCreate)
    {
        _filePath = filePath;
        _fileMode = fileMode;
    }

    public void OpenForWriting()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(FileResourceManager));

        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        _fileStream = new FileStream(_filePath, _fileMode, FileAccess.Write);
        _writer = new StreamWriter(_fileStream, Encoding.UTF8);
    }

    public void OpenForReading()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(FileResourceManager));

        _fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
        _reader = new StreamReader(_fileStream, Encoding.UTF8);
    }

    public void WriteLine(string text)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(FileResourceManager));
        if (_writer == null) throw new InvalidOperationException("Файл не открыт для записи");

        _writer.WriteLine(text);
        _writer.Flush();
    }

    public string ReadAllText()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(FileResourceManager));
        if (_reader == null) throw new InvalidOperationException("Файл не открыт для чтения");

        _reader.BaseStream.Position = 0;
        return _reader.ReadToEnd();
    }

    public void AppendText(string text)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(FileResourceManager));

        if (_writer != null)
        {
            _writer.Write(text);
            _writer.Flush();
        }
        else
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.AppendAllText(_filePath, text, Encoding.UTF8);
        }
    }

    public FileInfo GetFileInfo()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(FileResourceManager));
        return new FileInfo(_filePath);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _writer?.Dispose();
                _reader?.Dispose();
                _fileStream?.Dispose();
            }
            _disposed = true;
        }
    }

    ~FileResourceManager()
    {
        Dispose(false);
    }
}

