using System.Windows.Input;

namespace ViewModelBase.Commands.AsyncCommand
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();
        bool CanExecute();
    }
}
