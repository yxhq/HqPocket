using HqPocket.Helpers;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace HqPocket.IO;

public class TextDataReader<T> : DataReaderWriterBase<T>, IDataReader<T> where T : class, new()
{
    protected string? Separator { get; set; }
    protected int HeadLineCount { get; }

    public TextDataReader(string fileName, string? separator = " ", int headLineCount = 1)
        : base(fileName, false)
    {
        Separator = separator;
        HeadLineCount = headLineCount;
    }

    public IEnumerable<T> ReadAllData()
    {
        return File.ReadAllLines(FileName).Skip(HeadLineCount).Select(CreateData);
    }

    protected virtual T CreateData(string line)
    {
        try
        {
            var allColumns = line.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
            var readColumns = ReadWriteItems.Where(item => item.CanRead).Select(item => allColumns[item.Column]);

            StringBuilder dataBuilder = new("{");
            var elements = ReadWriteItems.Select(t => t.PropertyInfo).Zip(readColumns, JsonHelper.GetJsonString);
            dataBuilder.AppendJoin(',', elements);
            dataBuilder.Append('}');
            return JsonSerializer.Deserialize<T>(dataBuilder.ToString()) ?? new();
        }
        catch
        {
            return new();
        }
    }
}
