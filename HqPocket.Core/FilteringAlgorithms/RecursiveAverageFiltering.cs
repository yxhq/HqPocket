
using System.Collections.Generic;
using System.Linq;

namespace HqPocket.FilteringAlgorithms;

/// <summary>
/// 递推平均滤波算法
/// </summary>
public class RecursiveAverageFiltering
{
    private readonly Queue<double> _queue;
    public int Count { get; }

    public RecursiveAverageFiltering(int count)
    {
        Count = count;
        _queue = new Queue<double>(Count);
    }

    public double AddAndAverage(double value)
    {
        if (_queue.Count >= Count)
        {
            _ = _queue.Dequeue();
        }
        _queue.Enqueue(value);
        return _queue.Average();
    }

    public void Clear()
    {
        _queue.Clear();
    }
}
