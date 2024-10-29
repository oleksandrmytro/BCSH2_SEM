using System.Windows.Input;

namespace BCSH2_SEM.ViewModel.Commands;

public class NewNotebookCommand : ICommand
{
    public NotesVM VM { get; set; }

    public event EventHandler CanExecuteChanged;

    public NewNotebookCommand(NotesVM vm)
    {
        VM = vm;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        VM.CreateNotebook();
    }
}