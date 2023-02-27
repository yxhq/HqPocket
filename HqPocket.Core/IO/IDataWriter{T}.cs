using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HqPocket.IO;

public interface IDataWriter<in T> : IDisposable where T : class, new()
{
    /// <summary>
    /// 写入标题行（必须为第1行，且只有1行），默认根据typeof(T)自动生成headLine
    /// </summary>
    void WriteHeadLine(string? head = null);

    /// <summary>
    /// 逐行写入
    /// </summary>
    /// <param name="data">数据</param>
    void AppendWrite(T data);

    /// <summary>
    /// 一次性写入所有的datas
    /// </summary>
    /// <param name="datas">数据</param>
    void AppendWrite(IEnumerable<T> datas);

    /// <summary>
    /// 关闭写入，主要执行保存/清理操作
    /// </summary>
    void SaveAndClose();

    /// <summary>
    /// 关闭写入，主要执行保存/清理操作（异步）
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SaveAndCloseAsync(CancellationToken cancellationToken = default);
}
