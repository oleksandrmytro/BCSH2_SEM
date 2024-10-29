using BCSH2_SEM.Model;
using System.Windows.Input;

namespace BCSH2_SEM.ViewModel.Commands;

public class HasEditedCommand : ICommand
{
    public NotesVM VM { get; set; }
    public event EventHandler CanExecuteChanged;

    public HasEditedCommand(NotesVM vm)
    {
        VM = vm;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        Notebook notebook = parameter as Notebook;
        VM.HasRenamed(notebook);
    }
}