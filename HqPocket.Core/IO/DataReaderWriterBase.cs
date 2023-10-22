using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace HqPocket.IO;

public abstract class DataReaderWriterBase<T> where T : class
{
    protected string FileName { get; }
    protected IList<ReadWriteItem> ReadWriteItems { get; }

    /// <summary>
    /// 数据读取、写入
    /// </summary>
    /// <param name="fileName">文件名称</param>
    /// <param name="isAlign">是否需要对齐</param>
    protected DataReaderWriterBase(string fileName, bool isAlign)
    {
        FileName = fileName;

        var readWriteItems = from p in typeof(T).GetProperties()
                             let dn = p.GetCustomAttribute<DisplayNameAttribute>()
                             let rw = p.GetCustomAttribute<ReadWriteAttribute>()
                             where rw is not null
                             orderby rw.Column
                             select new ReadWriteItem(p, dn?.DisplayName, isAlign, rw);
        ReadWriteItems = readWriteItems.ToList();
    }
}
