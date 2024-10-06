using System.Windows.Input;
using BCSH2_SEM.ViewModel;

namespace BCSH2_SEM.Commands;

public class LoginCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;
    public LoginVM VM { get; set; }
    
    public LoginCommand(LoginVM vm)
    {
        VM = vm;
    }
    
    public bool CanExecute(object? parameter)
    {
        throw new NotImplementedException();
    }

    public void Execute(object? parameter)
    {
        throw new NotImplementedException();
    }

}