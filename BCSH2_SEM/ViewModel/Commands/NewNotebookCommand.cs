using System.Windows.Input;
using BCSH2_SEM.ViewModel;

namespace BCSH2_SEM.Commands;

public class NewNotebookCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;
    public NotesVM VM { get; set; }
    
    public NewNotebookCommand(NotesVM vm)
    {
        VM = vm;
    }
    
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        VM.CreateNotebook();
    }
}