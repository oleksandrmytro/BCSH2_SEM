using System.Windows;
using System.Windows.Input;
using BCSH2_SEM.ViewModel;

namespace BCSH2_SEM.Commands;

public class ExitCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;
    public NotesVM VM { get; set; }

    public ExitCommand(NotesVM vm)
    {
        VM = vm;
    }
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        VM.Exit();
    }

}