using System.Collections.Generic;

namespace HqPocket.IO;

/// <summary>
/// 数据读取
/// </summary>
public interface IDataReader
{
    /// <summary>
    /// 读取文件所有数据项（不含标题行）
    /// </summary>
    /// <returns>所有数据行（不含标题行）</returns>
    IEnumerable<string> ReadAllDataLines();
}
