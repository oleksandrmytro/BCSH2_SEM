using System;
using System.Windows.Input;
using BCSH2_SEM.Model;

namespace BCSH2_SEM.ViewModel.Commands
{
    public class SaveEditedNoteCommand : ICommand
    {
        private readonly NotesVM viewModel;

        public SaveEditedNoteCommand(NotesVM viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return parameter is Note note && note.IsEditing;
        }

        public void Execute(object parameter)
        {
            if (parameter is Note note)
            {
                viewModel.SaveEditedNote(note);
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}