using System.Collections.ObjectModel;
using System.Windows;
using BCSH2_SEM.Commands;
using BCSH2_SEM.Model;
using BCSH2_SEM.ViewModel.Helpers;

namespace BCSH2_SEM.ViewModel;

public class NotesVM
{
    public ObservableCollection<Notebook> Notebooks { get; set; }
    private Notebook selectedNotebook;
    
    public Notebook SelectedNotebook
    {
        get => selectedNotebook;
        set
        {
            selectedNotebook = value;
            //TODO: get notes
        }
    }
    
    public ObservableCollection<Note> Notes { get; set; }
    public NewNotebookCommand NewNotebookCommand { get; set; }
    public NewNoteCommand NewNoteCommand { get; set; }
    public ExitCommand ExitCommand { get; set; }
    
    public NotesVM()
    {
        NewNotebookCommand = new NewNotebookCommand(this);
        NewNoteCommand = new NewNoteCommand(this);
        ExitCommand = new ExitCommand(this);
    }

    public void CreateNotebook()
    {
        Notebook newNotebook = new Notebook()
        {
            Name = "New Notebook"
        };

        DatabaseHelper.Insert(newNotebook);
    }

    public void CreateNote(int notebookId)
    {
        var newNote = new Note()
        {
            NotebookId = notebookId,
            Title = "New Note",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        };

        DatabaseHelper.Insert(newNote);
    }

    public void Exit()
    {
        Application.Current.Shutdown();
    }
}