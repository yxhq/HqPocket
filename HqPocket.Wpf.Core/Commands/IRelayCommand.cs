using System.Windows.Input;

namespace HqPocket.Wpf.Commands;

public interface IRelayCommand : ICommand
{
    void RaiseCanExecuteChanged();
}