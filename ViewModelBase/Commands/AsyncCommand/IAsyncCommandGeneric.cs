using System.Windows.Input;

namespace ViewModelBase.Commands.AsyncCommand
{
    public interface IAsyncCommand<in T> : ICommand
    {
        Task ExecuteAsync(T parameter);
        bool CanExecute(T? parameter);
    }
}
