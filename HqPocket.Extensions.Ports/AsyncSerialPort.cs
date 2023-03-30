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
    private CancellationTokenSource? _receiveTaskCts;
    private CancellationTokenSource? _continuousSendTaskCts;
    private Timer? _loopWriteTimer;
    private readonly ConcurrentQueue<byte[]> _receivedBytesQueue = new();
    public SerialPort? SerialPort { get; private set; }

    public event EventHandler<byte[]>? BytesReceived;
    public event EventHandler<CancelEventArgs>? Opening;
    public event EventHandler? Opened;
    public event EventHandler? Closed;

    public bool DtrEnable { get; set; }
    public bool RtsEnable { get; set; }
    public int DataBits { get; set; } = 8;
    public Handshake Handshake { get; set; } = Handshake.None;
    public StopBits StopBits { get; set; } = StopBits.One;
    public Parity Parity { get; set; } = Parity.None;
    public int BaudRate { get; set; } = 9600;
    public int ReadBufferSize { get; set; } = 409600;
    public int WriteBufferSize { get; set; } = 2048;
    public bool IsOpen => SerialPort?.IsOpen ?? false;

    private string _portName = "COM1";
    public string PortName
    {
        get => _portName;
        set
        {
            if (_portName != value && !string.IsNullOrWhiteSpace(value))
            {
                if (IsOpen)
                {
                    Close();
                }
                _portName = value;
            }
        }
    }

    public void Open()
    {
        SerialPort = new(PortName, BaudRate, Parity, DataBits, StopBits)
        {
            ReadBufferSize = ReadBufferSize,
            WriteBufferSize = WriteBufferSize
        };

        CancelEventArgs e = new();
        OnOpening(e);
        if (e.Cancel) return;

        _receiveTaskCts = new();
        Task.Run(()=>ProcessReceivedBytes(_receiveTaskCts.Token), _receiveTaskCts.Token);

        SerialPort.DataReceived += SerialPort_DataReceived;
        SerialPort.Open();
        OnOpened(EventArgs.Empty);
    }

    public void Close()
    {
        if (SerialPort is null) return;

        StopLoopWrite();

        SerialPort.DataReceived -= SerialPort_DataReceived;
        SerialPort.Close();
        SerialPort = null;

        _receiveTaskCts?.Cancel();
        _receivedBytesQueue.Clear();
        OnClosed(EventArgs.Empty);
    }

    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        if (sender is SerialPort port)
        {
            try
            {
                int bytesToRead = port.BytesToRead;
                byte[] readBuffer = new byte[bytesToRead];

                if (port.Read(readBuffer, 0, bytesToRead) > 0)
                {
                    _receivedBytesQueue.Enqueue(readBuffer);
                }
            }
            catch (InvalidOperationException)    // The port is closed.
            {
                // do nothing                
            }
        }
    }

    private void ProcessReceivedBytes(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (_receivedBytesQueue.IsEmpty)
            {
                try
                {
                    Task.Delay(1, token).Wait(token);
                }
                catch (OperationCanceledException)
                {
                }
            }
            else
            {
                while (_receivedBytesQueue.TryDequeue(out var receiveBytesBuffer))
                {
                    OnBytesReceived(receiveBytesBuffer);
                }
            }
        }
        _receiveTaskCts?.Dispose();
        _receiveTaskCts = null;
    }

    public void Write(IEnumerable<byte>? bytes)
    {
        SerialPort?.Write(bytes);
    }

    public void Write(string text)
    {
        SerialPort?.Write(text);
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
        if (interval == TimeSpan.Zero)
        {
            _continuousSendTaskCts = new();
            Task.Run(() =>
            {
                while (!_continuousSendTaskCts.IsCancellationRequested)
                {
                    Write(bytes);
                    loopCallback?.Invoke();
                }
                _continuousSendTaskCts.Dispose();
                _continuousSendTaskCts = null;

            }, _continuousSendTaskCts.Token);
        }
        else
        {
            _loopWriteTimer = new Timer(_ =>
            {
                Write(bytes);
                loopCallback?.Invoke();
            }, null, Timeout.Infinite, 0);

            _loopWriteTimer.Change(TimeSpan.Zero, interval);
        }
    }

    public void LoopWrite(string text, TimeSpan interval, Action? loopCallback = null)
    {
        if (interval == TimeSpan.Zero)
        {
            _continuousSendTaskCts = new();
            Task.Run(() =>
            {
                while (!_continuousSendTaskCts.IsCancellationRequested)
                {
                    Write(text);
                    loopCallback?.Invoke();
                }
                _continuousSendTaskCts.Dispose();
                _continuousSendTaskCts = null;

            }, _continuousSendTaskCts.Token);
        }
        else
        {
            _loopWriteTimer = new Timer(_ =>
            {
                Write(text);
                loopCallback?.Invoke();
            }, null, Timeout.Infinite, 0);

            _loopWriteTimer.Change(TimeSpan.Zero, interval);
        }
    }

    public void StopLoopWrite()
    {
        _continuousSendTaskCts?.Cancel();

        if (_loopWriteTimer is not null)
        {
            _loopWriteTimer.Change(Timeout.Infinite, 0);
            _loopWriteTimer.Dispose();
            _loopWriteTimer = null;
        }
    }
}
