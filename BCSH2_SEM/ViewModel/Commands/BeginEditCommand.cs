// BeginEditCommand.cs
using BCSH2_SEM.Model;
using System;
using System.Windows.Input;

namespace BCSH2_SEM.ViewModel.Commands
{
    public class BeginEditCommand : ICommand
    {
        public NotesVM ViewModel { get; set; }

        public BeginEditCommand(NotesVM vm)
        {
            ViewModel = vm;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is Notebook notebook)
            {
                ViewModel.StartEditing(notebook);
            }
        }
    }
}