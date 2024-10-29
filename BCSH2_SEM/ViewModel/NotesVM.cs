using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using BCSH2_SEM.Model;
using BCSH2_SEM.ViewModel.Commands;
using BCSH2_SEM.ViewModel.Helpers;
using SQLite;

namespace BCSH2_SEM.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {
        private bool isEditing;

        /// <summary>
        /// Indicates whether the user is currently editing a notebook.
        /// </summary>
        public bool IsEditing
        {
            get { return isEditing; }
            set
            {
                isEditing = value;
                OnPropertyChanged("IsEditing");
            }
        }

        /// <summary>
        /// Collection of all notebooks belonging to the user.
        /// </summary>
        public ObservableCollection<Notebook> Notebooks { get; set; }

        private Notebook selectedNotebook;

        /// <summary>
        /// The currently selected notebook.
        /// </summary>
        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set
            {
                selectedNotebook = value;
                OnPropertyChanged("SelectedNotebook");
                if (selectedNotebook != null)
                    ReadNotes();
            }
        }

        private Note note;

        /// <summary>
        /// The currently selected note.
        /// </summary>
        public Note SelectedNote
        {
            get { return note; }
            set
            {
                note = value;
                SelectedNoteChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Collection of all notes within the selected notebook.
        /// </summary>
        public ObservableCollection<Note> Notes { get; set; }

        // Commands for various actions
        public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }
        public BeginEditCommand BeginEditCommand { get; set; }
        public HasEditedCommand HasEditedCommand { get; set; }
        public DeleteNotebookCommand DeleteNotebookCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler SelectedNoteChanged;

        /// <summary>
        /// Initializes a new instance of the NotesVM class.
        /// </summary>
        public NotesVM()
        {
            IsEditing = false;

            // Initialize commands
            NewNotebookCommand = new NewNotebookCommand(this);
            NewNoteCommand = new NewNoteCommand(this);
            BeginEditCommand = new BeginEditCommand(this);
            HasEditedCommand = new HasEditedCommand(this);
            DeleteNotebookCommand = new DeleteNotebookCommand(this);

            // Initialize collections
            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            // Load notebooks and notes
            ReadNotebooks();
            ReadNotes();
        }

        /// <summary>
        /// Handles property change notifications.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Creates a new notebook for the current user.
        /// </summary>
        public void CreateNotebook()
        {
            Notebook newNotebook = new Notebook()
            {
                Name = "New notebook",
                UserId = int.Parse(App.UserId)
            };

            DatabaseHelper.Insert(newNotebook);

            ReadNotebooks();
        }

        /// <summary>
        /// Deletes the specified notebook.
        /// </summary>
        /// <param name="notebook">The notebook to delete.</param>
        public void DeleteNotebook(Notebook notebook)
        {
            DatabaseHelper.Delete(notebook);
            Notebooks.Remove(notebook);
            // Optionally, you can call ReadNotebooks() to refresh the list
            // ReadNotebooks();
        }

        /// <summary>
        /// Creates a new note within the specified notebook.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook.</param>
        public void CreateNote(int notebookId)
        {
            Note newNote = new Note()
            {
                NotebookId = notebookId,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                Title = "New note"
            };

            DatabaseHelper.Insert(newNote);

            ReadNotes();
        }

        /// <summary>
        /// Reads all notebooks for the current user from the database.
        /// </summary>
        public void ReadNotebooks()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.dbFile))
            {
                // Get the current UserId
                if (int.TryParse(App.UserId, out int currentUserId))
                {
                    // Filter notebooks by UserId
                    var notebooks = connection.Table<Notebook>()
                        .Where(n => n.UserId == currentUserId)
                        .ToList();

                    Notebooks.Clear();
                    notebooks.ForEach(notebook =>
                    {
                        Notebooks.Add(notebook);
                    });
                }
                else
                {
                    Console.WriteLine("Invalid UserId.");
                }
            }
        }

        /// <summary>
        /// Reads all notes within the selected notebook from the database.
        /// </summary>
        public void ReadNotes()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.dbFile))
            {
                if (SelectedNotebook != null)
                {
                    var notes = connection.Table<Note>().Where(note => note.NotebookId == SelectedNotebook.Id).ToList();

                    Notes.Clear();
                    notes.ForEach(note =>
                    {
                        Notes.Add(note);
                    });
                }
            }
        }

        /// <summary>
        /// Initiates the editing mode for a notebook.
        /// </summary>
        public void StartEditing()
        {
            IsEditing = true;
        }

        /// <summary>
        /// Renames a notebook after editing.
        /// </summary>
        /// <param name="notebook">The notebook to rename.</param>
        public void HasRenamed(Notebook notebook)
        {
            if (notebook != null)
            {
                DatabaseHelper.Update(notebook);
                IsEditing = false;
                ReadNotebooks();
            }
        }

        /// <summary>
        /// Updates the selected note in the database.
        /// </summary>
        public void UpdateSelectedNote()
        {
            DatabaseHelper.Update(SelectedNote);
        }
    }
}
