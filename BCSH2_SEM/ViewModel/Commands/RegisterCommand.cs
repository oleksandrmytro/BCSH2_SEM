using System.Windows.Input;
using BCSH2_SEM.ViewModel;

namespace BCSH2_SEM.Commands;

public class RegisterCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;
    public LoginVM VM { get; set; }

    public RegisterCommand(LoginVM vm)
    {
        VM = vm;
    }
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        //TODO: Login functionality
    }

    
}