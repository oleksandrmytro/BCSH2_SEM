using System;
using System.Windows;
using System.Windows.Input;
using BCSH2_SEM.Model;

namespace BCSH2_SEM.ViewModel.Commands
{
    public class DeleteNoteCommand : ICommand
    {
        private readonly NotesVM viewModel;

        public DeleteNoteCommand(NotesVM viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return parameter is Note note;
        }

        public void Execute(object parameter)
        {
            if (parameter is Note note)
            {
                // Optional: Add confirmation before deletion
                var result = MessageBox.Show(
                    $"Are you sure you want to delete the note \"{note.Title}\"?",
                    "Delete Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    viewModel.DeleteNote(note);
                }
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}