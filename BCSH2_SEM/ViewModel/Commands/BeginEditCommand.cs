using System.Windows.Input;

namespace BCSH2_SEM.ViewModel.Commands;

public class BeginEditCommand : ICommand
{
    public NotesVM Vm { get; set; }
    public event EventHandler CanExecuteChanged;

    public BeginEditCommand(NotesVM vm)
    {
        Vm = vm;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        Vm.StartEditing();
    }
}