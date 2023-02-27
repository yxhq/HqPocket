namespace HqPocket.Wpf.Commands;

public interface IRelayCommand<in T> : IRelayCommand
{
    bool CanExecute(T parameter);
    void Execute(T parameter);
}