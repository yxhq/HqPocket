
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;

namespace HqPocket.Extensions.Ports;

public interface IAsyncSerialPort
{
    /// <summary>
    /// 串口中接收到数据后，byte[]长度不确定，与当前读取字节数有关
    /// </summary>
    event EventHandler<byte[]>? BytesReceived;
    /// <summary>
    /// 串口打开时
    /// </summary>
    event EventHandler<CancelEventArgs>? Opening;
    /// <summary>
    /// 串口打开后
    /// </summary>
    event EventHandler? Opened;
    /// <summary>
    /// 串口关闭后
    /// </summary>
    event EventHandler? Closed;

    SerialPort SerialPort { get; }
    bool DtrEnable { get; set; }
    bool RtsEnable { get; set; }
    int DataBits { get; set; }
    Handshake Handshake { get; set; }
    StopBits StopBits { get; set; }
    Parity Parity { get; set; }
    int BaudRate { get; set; }
    bool IsOpen { get; }
    string PortName { get; set; }
    void Open();
    void Close();
    void Write(IEnumerable<byte>? bytes);
    void Write(string text);
    /// <summary>
    /// 循环写入字节
    /// </summary>
    /// <param name="bytes">要写入的字节</param>
    /// <param name="interval">循环写入的时间间隔（周期）</param>
    /// <param name="loopCallback">每次写入后执行（回调函数）</param>
    void LoopWrite(IEnumerable<byte>? bytes, TimeSpan interval, Action? loopCallback = null);
    /// <summary>
    /// 循环写入文本
    /// </summary>
    /// <param name="text">要写入的文本</param>
    /// <param name="interval">循环写入的时间间隔（周期）</param>
    /// <param name="loopCallback">每次写入后执行（回调函数）</param>
    void LoopWrite(string text, TimeSpan interval, Action? loopCallback = null);
    /// <summary>
    /// 停止循环写入
    /// </summary>
    void StopLoopWrite();
}