using System;
using System.Collections.Generic;

namespace HqPocket.IO;

/// <summary>
/// 数据写入
/// </summary>
public interface IDataWriter : IDisposable
{
    /// <summary>
    /// 写入1行数据
    /// </summary>
    /// <param name="line">行内容</param>
    void WriteLine(string line);

    /// <summary>
    /// 逐行写入
    /// </summary>
    /// <param name="line">行内容</param>
    void AppendWriteLine(string line);

    /// <summary>
    /// 一次性写入所有的lines
    /// </summary>
    /// <param name="lines">行内容</param>
    void AppendWriteLine(IEnumerable<string> lines);

    /// <summary>
    /// 关闭写入，主要执行清理/保存操作
    /// </summary>
    void Close();
}
