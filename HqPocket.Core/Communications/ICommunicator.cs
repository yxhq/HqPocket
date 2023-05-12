using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HqPocket.Communications
{
    public interface ICommunicator
    {
        /// <summary>
        /// 接收到数据时
        /// </summary>
        event EventHandler<byte[]>? BytesReceived;

        /// <summary>
        /// 本机准备连接，对Client：准备连接到服务端，对Server：准备打开服务器，对串口：准备打开串口
        /// </summary>
        event EventHandler<CancelEventArgs>? Connecting;

        /// <summary>
        /// 本机已经连接，对Client：已成功连接到服务端，对Server：已成功打开服务器，对串口：已成功打开串口
        /// </summary>
        event EventHandler<EventArgs>? Connected;

        /// <summary>
        /// 本机准备断开连接，对Client：准备断开与服务端的连接，对Server：准备关闭服务器，对串口：准备关闭串口
        /// </summary>
        event EventHandler<CancelEventArgs>? Disconnecting;

        /// <summary>
        /// 本机已经断开连接，对Client：已断开与服务端的连接，对Server：已关闭服务器，对串口：已关闭串口
        /// </summary>
        event EventHandler<EventArgs>? Disconnected;

        /// <summary>
        /// 远端已连接，对Client：同Connected，对Server：有Client成功连接，对串口：无
        /// </summary>
        event EventHandler<RemoteConnectionEventArgs>? RemoteConnected;

        /// <summary>
        /// 远端已断开，对Client：服务器关闭，对Server：有Client断开连接，对串口：无
        /// </summary>
        event EventHandler<RemoteConnectionEventArgs>? RemoteDisconnected;

        /// <summary>
        /// 唯一名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 本机是否，对Client：成功连接到服务端，对Server：成功打开服务器，对串口：成功打开串口
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 远端是否，对Client：服务器关闭，对Server：Client连接/断开，对串口：无
        /// </summary>
        bool IsRemoteConnected { get; }

        /// <summary>
        /// 对Client：连接到服务器，对Server：打开服务器，对串口：打开串口
        /// </summary>
        void Connect();

        /// <summary>
        /// 对Client：断开与服务器的连接，对Server：关闭服务器，对串口：关闭串口
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 发送数据到，对Client：服务器，对Server：客户端，对串口：串口
        /// </summary>
        /// <param name="bytes">要发送的字节</param>
        void Send(IEnumerable<byte>? bytes);

        /// <summary>
        /// 循环发送数据到，对Client：服务器，对Server：客户端，对串口：串口
        /// </summary>
        /// <param name="bytes"><要发送的字节/param>
        /// <param name="period">循环间隔</param>
        /// <param name="loopCallback">每次发送完后回调函数</param>
        void LoopSend(IEnumerable<byte>? bytes, TimeSpan period, Action? loopCallback = null);

        /// <summary>
        /// 停止循环发送
        /// </summary>
        void StopLoopSend();
    }
}
