using System.Threading.Tasks;

namespace HqPocket.Wpf.Commands;

public interface IAsyncRelayCommand<in T> : IAsyncRelayCommand, IRelayCommand<T>
{
    Task ExecuteAsync(T parameter);
}