using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HqPocket.IO;

public class TextDataReader : IDataReader
{
    protected string FileName { get; }
    protected int HeadLineCount { get; }

    public TextDataReader(string fileName, int headLineCount = 1)
    {
        FileName = fileName;
        HeadLineCount = headLineCount;
    }

    public IEnumerable<string> ReadAllDataLines()
    {
        return File.ReadAllLines(FileName).Skip(HeadLineCount);
    }
}
