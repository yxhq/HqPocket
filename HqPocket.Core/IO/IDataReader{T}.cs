using System.Collections.Generic;

namespace HqPocket.IO;

public interface IDataReader<out T> where T : class, new()
{
    /// <summary>
    /// 读取文件所有数据项（不含标题行）
    /// </summary>
    /// <returns>所有数据项</returns>
    IEnumerable<T> ReadAllData();
}
