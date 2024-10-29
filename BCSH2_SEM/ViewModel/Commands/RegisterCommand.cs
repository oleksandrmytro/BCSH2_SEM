using BCSH2_SEM.Model;
using System.Windows.Input;

namespace BCSH2_SEM.ViewModel.Commands;

public class RegisterCommand : ICommand
{
    public LoginVM VM { get; set; }
    public event EventHandler CanExecuteChanged;

    public RegisterCommand(LoginVM vm)
    {
        VM = vm;
    }

    public bool CanExecute(object parameter)
    {
        var user = parameter as User;
        return true;
    }

    public void Execute(object parameter)
    {
        VM.Register();
    }
}