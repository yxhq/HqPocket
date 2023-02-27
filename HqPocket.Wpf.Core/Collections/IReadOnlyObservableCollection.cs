using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace HqPocket.Wpf.Collections;

public interface IReadOnlyObservableCollection<out T> : IReadOnlyList<T>, INotifyCollectionChanged, INotifyPropertyChanged
{
}