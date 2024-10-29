using BCSH2_SEM.Model;
using System.Windows.Input;

namespace BCSH2_SEM.ViewModel.Commands;

public class DeleteNotebookCommand : ICommand
{
    public NotesVM Vm { get; set; }
    public event EventHandler CanExecuteChanged;

    public DeleteNotebookCommand(NotesVM vm)
    {
        Vm = vm;
    }

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
        Notebook notebook = parameter as Notebook;
        if (notebook != null)
            Vm.DeleteNotebook(notebook);
    }
}