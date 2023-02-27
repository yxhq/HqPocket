// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq;

public static partial class LinqExtensions
{
    /// <summary>
    /// Lazily invokes an action for each value in the sequence.
    /// </summary>
    /// <typeparam name="TSource">Source sequence element type.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="onNext">Action to invoke for each element.</param>
    /// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
    public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(onNext);          

        return DoCore(source, onNext, _ => { }, () => { });
    }

    /// <summary>
    /// Lazily invokes an action for each value in the sequence, and executes an action for successful termination.
    /// </summary>
    /// <typeparam name="TSource">Source sequence element type.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="onNext">Action to invoke for each element.</param>
    /// <param name="onCompleted">Action to invoke on successful termination of the sequence.</param>
    /// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
    public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext, Action onCompleted)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(onNext);
        ArgumentNullException.ThrowIfNull(onCompleted);

        return DoCore(source, onNext, _ => { }, onCompleted);
    }

    /// <summary>
    /// Lazily invokes an action for each value in the sequence, and executes an action upon exceptional termination.
    /// </summary>
    /// <typeparam name="TSource">Source sequence element type.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="onNext">Action to invoke for each element.</param>
    /// <param name="onError">Action to invoke on exceptional termination of the sequence.</param>
    /// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
    public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(onNext);
        ArgumentNullException.ThrowIfNull(onError);

        return DoCore(source, onNext, onError, () => { });
    }

    /// <summary>
    /// Lazily invokes an action for each value in the sequence, and executes an action upon successful or exceptional
    /// termination.
    /// </summary>
    /// <typeparam name="TSource">Source sequence element type.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="onNext">Action to invoke for each element.</param>
    /// <param name="onError">Action to invoke on exceptional termination of the sequence.</param>
    /// <param name="onCompleted">Action to invoke on successful termination of the sequence.</param>
    /// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
    public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(onNext);
        ArgumentNullException.ThrowIfNull(onError);
        ArgumentNullException.ThrowIfNull(onCompleted);

        return DoCore(source, onNext, onError, onCompleted);
    }

    /// <summary>
    /// Lazily invokes observer methods for each value in the sequence, and upon successful or exceptional termination.
    /// </summary>
    /// <typeparam name="TSource">Source sequence element type.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="observer">Observer to invoke notification calls on.</param>
    /// <returns>Sequence exhibiting the side-effects of observer method invocation upon enumeration.</returns>
    public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, IObserver<TSource> observer)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(observer);           

        return DoCore(source, observer.OnNext, observer.OnError, observer.OnCompleted);
    }

    private static IEnumerable<TSource> DoCore<TSource>(IEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
    {
        using var e = source.GetEnumerator();

        while (true)
        {
            TSource current;
            try
            {
                if (!e.MoveNext())
                    break;

                current = e.Current;
            }
            catch (Exception ex)
            {
                onError(ex);
                throw;
            }

            onNext(current);

            yield return current;
        }

        onCompleted();
    }
}
