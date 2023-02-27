using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HqPocket.IO;

public class TextDataWriter : IDataWriter
{
    private bool _disposed;
    private StreamWriter? _streamWriter;

    protected string FileName { get; }

    public TextDataWriter(string fileName)
    {
        FileName = fileName;
    }

    public void WriteLine(string line)
    {
        using StreamWriter streamWriter = new(FileName, false, Encoding.Default);
        streamWriter.WriteLine(line);
        streamWriter.Close();

        _streamWriter = new(FileName, true, Encoding.Default);
    }

    public void AppendWriteLine(string line)
    {
        _streamWriter?.WriteLine(line);
        _streamWriter?.Flush();
    }

    public void AppendWriteLine(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            _streamWriter?.WriteLine(line);
        }
        _streamWriter?.Flush();
    }

    public void Close()
    {
        _streamWriter?.Flush();
        _streamWriter?.Close();
    }

    /// <summary>
    /// 为了防止忘记显式的调用Dispose方法
    /// </summary>
    ~TextDataWriter()
    {
        Dispose(false); //必须为false
    }

    public void Dispose()
    {
        Dispose(true);  //必须为true
        GC.SuppressFinalize(this);  //通知垃圾回收器不再调用终结器
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        //清理托管资源
        if (disposing)
        {
            //TODO: 
        }

        //清理非托管资源
        if (_streamWriter is not null)
        {
            _streamWriter.Dispose();
            _streamWriter = null;
        }

        //告诉自己已经被释放
        _disposed = true;
    }
}
