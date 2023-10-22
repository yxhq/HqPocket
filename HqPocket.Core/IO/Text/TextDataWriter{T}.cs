using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HqPocket.IO;

public class TextDataWriter<T> : DataReaderWriterBase<T>, IDataWriter<T> where T : class
{
    private bool _disposed;
    private StreamWriter? _streamWriter;

    protected string? Separator { get; set; }

    public TextDataWriter(string fileName, bool isAlign, string? separator = "")
        : base(fileName, isAlign)
    {
        Separator = separator;
    }

    public void WriteHeadLine(string? head = null)
    {
        using StreamWriter streamWriter = new(FileName, false, Encoding.Default);
        head ??= CreateHeadLine();
        streamWriter.WriteLine(head);
        streamWriter.Close();
    }

    public void AppendWrite(T data)
    {
        _streamWriter ??= new StreamWriter(FileName, true, Encoding.Default);
        _streamWriter.WriteLine(CreateDataLine(data));
    }

    public void AppendWrite(IEnumerable<T> datas)
    {
        _streamWriter ??= new StreamWriter(FileName, true, Encoding.Default);
        foreach (var data in datas)
        {
            _streamWriter.WriteLine(CreateDataLine(data));
        }
    }

    public void SaveAndClose()
    {
        _streamWriter?.Flush();
        _streamWriter?.Close();
    }

    public async Task SaveAndCloseAsync(CancellationToken cancellationToken = default)
    {
        if (_streamWriter is not null)
        {
            await _streamWriter.FlushAsync();
            _streamWriter.Close();
        }
        await Task.CompletedTask;
    }

    protected virtual string CreateHeadLine()
    {
        var heads = ReadWriteItems.Select(item => item.DisplayName.Align(item.Format));
        return string.Join(Separator, heads);
    }

    protected virtual string CreateDataLine(T data)
    {
        var datas = ReadWriteItems.Select(item => string.Format(item.Format, item.PropertyInfo.GetValue(data)));
        return string.Join(Separator, datas);
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
