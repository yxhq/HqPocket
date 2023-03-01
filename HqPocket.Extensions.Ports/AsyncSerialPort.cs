using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace HqPocket.Extensions.Ports;

public class AsyncSerialPort : IAsyncSerialPort
{
    private Task _receiveTask;
    private Timer? _loopWriteTimer;
    private readonly ConcurrentQueue<byte[]> _receivedBytesQueue = new();
    public SerialPort SerialPort { get; }

    public event EventHandler<byte[]>? BytesReceived;
    public event EventHandler<CancelEventArgs>? Opening;
    public event EventHandler? Opened;
    public event EventHandler? Closed;

    public bool DtrEnable
    {
        get => SerialPort.DtrEnable;
        set => SerialPort.DtrEnable = value;
    }

    public bool RtsEnable
    {
        get => SerialPort.RtsEnable;
        set => SerialPort.RtsEnable = value;
    }

    public int DataBits
    {
        get => SerialPort.DataBits;
        set => SerialPort.DataBits = value;
    }

    public Handshake Handshake
    {
        get => SerialPort.Handshake;
        set => SerialPort.Handshake = value;
    }

    public StopBits StopBits
    {
        get => SerialPort.StopBits;
        set => SerialPort.StopBits = value;
    }

    public Parity Parity
    {
        get => SerialPort.Parity;
        set => SerialPort.Parity = value;
    }

    public int BaudRate
    {
        get => SerialPort.BaudRate;
        set => SerialPort.BaudRate = value;
    }

    public bool IsOpen => SerialPort.IsOpen;

    public string PortName
    {
        get => SerialPort.PortName;
        set
        {
            if (SerialPort.PortName != value && !string.IsNullOrWhiteSpace(value))
            {
                if (IsOpen)
                {
                    Close();
                }
                SerialPort.PortName = value;
            }
        }
    }

    public AsyncSerialPort()
    {
        SerialPort = new();
        _receiveTask = Task.Run(ReceiveAction);
    }

    public void Open()
    {
        CancelEventArgs e = new();
        OnOpening(e);
        if (e.Cancel) return;

        SerialPort.DataReceived += SerialPort_DataReceived;
        SerialPort.Open();
        OnOpened(EventArgs.Empty);
    }

    public void Close()
    {
        StopLoopWrite();
        SerialPort.DataReceived -= SerialPort_DataReceived;
        SerialPort.Close();
        _receivedBytesQueue.Clear();
        OnClosed(EventArgs.Empty);
    }

    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        if (sender is SerialPort port)
        {
            int readCount = 0;
            byte[]? readBuffer = null;

            try
            {
                int bytesToRead = port.BytesToRead;
                readBuffer = new byte[bytesToRead];
                readCount = port.Read(readBuffer, 0, bytesToRead);
            }
            catch (InvalidOperationException)   // The port is closed.
            {
                // do nothing
            }

            if (readCount > 0)
            {
                _receivedBytesQueue.Enqueue(readBuffer!);

                if (_receiveTask.IsCompleted)
                {
                    _receiveTask = Task.Run(ReceiveAction);
                }
            }
        }
    }

    private void ReceiveAction()
    {
        while (_receivedBytesQueue.TryDequeue(out var receiveBytesBuffer))
        {
            OnBytesReceived(receiveBytesBuffer);
        }
    }

    public void Write(IEnumerable<byte>? bytes)
    {
        SerialPort.Write(bytes);
    }

    public void Write(string text)
    {
        SerialPort.Write(text);
    }

    protected virtual void OnOpening(CancelEventArgs e)
    {
        Opening?.Invoke(this, e);
    }

    protected virtual void OnOpened(EventArgs e)
    {
        Opened?.Invoke(this, e);
    }

    protected virtual void OnClosed(EventArgs e)
    {
        Closed?.Invoke(this, e);
    }

    protected virtual void OnBytesReceived(byte[] receivedBytes)
    {
        BytesReceived?.Invoke(this, receivedBytes);
    }

    public void LoopWrite(IEnumerable<byte>? bytes, TimeSpan interval, Action? loopCallback = null)
    {
        _loopWriteTimer = new Timer(_ =>
        {
            Write(bytes);
            loopCallback?.Invoke();
        }, null, Timeout.Infinite, 0);
        _loopWriteTimer?.Change(TimeSpan.Zero, interval);
    }

    public void LoopWrite(string text, TimeSpan interval, Action? loopCallback = null)
    {
        _loopWriteTimer = new Timer(_ =>
        {
            Write(text);
            loopCallback?.Invoke();
        }, null, Timeout.Infinite, 0);
        _loopWriteTimer?.Change(TimeSpan.Zero, interval);
    }

    public void StopLoopWrite()
    {
        if (_loopWriteTimer is null) return;

        _loopWriteTimer.Change(Timeout.Infinite, 0);
        _loopWriteTimer.Dispose();
        _loopWriteTimer = null;
    }
}
