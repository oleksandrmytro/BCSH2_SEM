using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using BCSH2_SEM.Model;
using BCSH2_SEM.ViewModel.Commands;
using BCSH2_SEM.ViewModel.Helpers;
using SQLite;

namespace BCSH2_SEM.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {
        private bool isEditing;
        private string notebookSearchText;
        private string noteSearchText;
        private readonly ICollectionView notebooksView;
        private readonly ICollectionView notesView;

        public bool IsEditing
        {
            get { return isEditing; }
            set
            {
                if (isEditing != value)
                {
                    isEditing = value;
                    OnPropertyChanged(nameof(IsEditing));
                }
            }
        }

        public ObservableCollection<Notebook> Notebooks { get; set; }
        public ICollectionView NotebooksView => notebooksView;
        public ObservableCollection<Note> Notes { get; set; }
        public ICollectionView NotesView => notesView;

        private Notebook selectedNotebook;
        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set
            {
                if (selectedNotebook != value)
                {
                    selectedNotebook = value;
                    OnPropertyChanged(nameof(SelectedNotebook));
                    if (selectedNotebook != null)
                        ReadNotes();
                }
            }
        }

        private Note note;
        public Note SelectedNote
        {
            get { return note; }
            set
            {
                if (note != value)
                {
                    note = value;
                    OnPropertyChanged(nameof(SelectedNote));
                    SelectedNoteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        // Commands
        public ICommand NewNotebookCommand { get; set; }
        public ICommand NewNoteCommand { get; set; }
        public ICommand BeginEditCommand { get; set; }
        public ICommand HasRenamedCommand { get; set; }
        public ICommand DeleteNotebookCommand { get; set; }

        public ICommand BeginEditNoteCommand { get; set; }
        public ICommand SaveEditedNoteCommand { get; set; }
        public ICommand DeleteNoteCommand { get; set; }

        // Search
        public string NotebookSearchText
        {
            get { return notebookSearchText; }
            set
            {
                if (notebookSearchText != value)
                {
                    notebookSearchText = value;
                    OnPropertyChanged(nameof(NotebookSearchText));
                    notebooksView.Refresh();
                }
            }
        }

        public string NoteSearchText
        {
            get { return noteSearchText; }
            set
            {
                if (noteSearchText != value)
                {
                    noteSearchText = value;
                    OnPropertyChanged(nameof(NoteSearchText));
                    notesView.Refresh();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler SelectedNoteChanged;

        public NotesVM()
        {
            IsEditing = false;

            // Ініціалізація команд
            NewNotebookCommand = new NewNotebookCommand(this);
            NewNoteCommand = new NewNoteCommand(this);
            BeginEditCommand = new RelayCommand<Notebook>(StartEditing);
            HasRenamedCommand = new RelayCommand<Notebook>(HasRenamed);
            DeleteNotebookCommand = new DeleteNotebookCommand(this); // Ініціалізуємо команду видалення

            BeginEditNoteCommand = new RelayCommand<Note>(StartEditingNote);
            SaveEditedNoteCommand = new RelayCommand<Note>(SaveEditedNote);
            DeleteNoteCommand = new DeleteNoteCommand(this);

            // Ініціалізація колекцій
            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            // Ініціалізація CollectionViews для фільтрації
            notebooksView = CollectionViewSource.GetDefaultView(Notebooks);
            notebooksView.Filter = FilterNotebooks;

            notesView = CollectionViewSource.GetDefaultView(Notes);
            notesView.Filter = FilterNotes;
        }

        private bool FilterNotebooks(object obj)
        {
            if (obj is Notebook notebook)
            {
                if (string.IsNullOrWhiteSpace(NotebookSearchText))
                    return true;
                return notebook.Name.IndexOf(NotebookSearchText, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return false;
        }

        private bool FilterNotes(object obj)
        {
            if (obj is Note note)
            {
                if (string.IsNullOrWhiteSpace(NoteSearchText))
                    return true;
                return (note.Title != null && note.Title.IndexOf(NoteSearchText, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            return false;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Метод створення нового нотбука
        public void CreateNotebook()
        {
            if (int.TryParse(App.UserId, out int userId))
            {
                Notebook newNotebook = new Notebook()
                {
                    Name = "New notebook",
                    UserId = userId
                };

                DatabaseHelper.Insert(newNotebook);
                Notebooks.Add(newNotebook);
            }
            else
            {
                // Обробка помилки
                Console.WriteLine("Invalid UserId.");
            }
        }

        // Метод створення нової замітки
        public void CreateNote(int notebookId)
        {
            Note newNote = new Note()
            {
                NotebookId = notebookId,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                Title = "New note",
            };

            DatabaseHelper.Insert(newNote);
            Notes.Add(newNote);
        }

        // Метод видалення нотбука
        public void DeleteNotebook(Notebook notebook)
        {
            if (notebook != null)
            {
                // (Опціонально) Видалення всіх заміток, пов'язаних з нотбуком
                var relatedNotes = Notes.Where(n => n.NotebookId == notebook.Id).ToList();
                foreach (var note in relatedNotes)
                {
                    DatabaseHelper.Delete(note);
                    Notes.Remove(note);
                }

                DatabaseHelper.Delete(notebook);
                Notebooks.Remove(notebook);
            }
        }

        // Метод читання нотбуків з бази даних
        public void ReadNotebooks()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.dbFile))
            {
                if (int.TryParse(App.UserId, out int currentUserId))
                {
                    var notebooks = connection.Table<Notebook>()
                        .Where(n => n.UserId == currentUserId)
                        .ToList();

                    Notebooks.Clear();
                    foreach (var notebook in notebooks)
                    {
                        notebook.IsEditing = false;
                        Notebooks.Add(notebook);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid UserId.");
                }
            }
        }

        // Метод читання заміток з бази даних
        public void ReadNotes()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.dbFile))
            {
                if (SelectedNotebook != null)
                {
                    var notes = connection.Table<Note>()
                        .Where(note => note.NotebookId == SelectedNotebook.Id)
                        .ToList();

                    Notes.Clear();
                    foreach (var note in notes)
                    {
                        note.IsEditing = false;
                        Notes.Add(note);
                    }
                }
                else
                {
                    Notes.Clear();
                }
            }
        }

        // Метод для початку редагування нотбука
        public void StartEditing(Notebook notebook)
        {
            foreach (var nb in Notebooks)
            {
                nb.IsEditing = false; // Reset editing state
            }
            if (notebook != null)
            {
                notebook.IsEditing = true;
            }
        }

        // Метод для завершення редагування та перейменування нотбука
        public void HasRenamed(Notebook notebook)
        {
            if (notebook != null)
            {
                DatabaseHelper.Update(notebook);
                notebook.IsEditing = false; // Закриваємо режим редагування

                var existingNotebook = Notebooks.FirstOrDefault(n => n.Id == notebook.Id);
                if (existingNotebook != null)
                {
                    int index = Notebooks.IndexOf(existingNotebook);
                    Notebooks[index] = notebook;
                }

                notebooksView.Refresh();
            }
        }

        // Метод оновлення вибраної замітки
        public void UpdateSelectedNote()
        {
            if (SelectedNote != null)
            {
                SelectedNote.UpdatedTime = DateTime.Now; // Оновлюємо час зміни
                DatabaseHelper.Update(SelectedNote);
            }
        }

        public void StartEditingNote(Note note)
        {
            foreach (var n in Notes)
            {
                n.IsEditing = false; // Reset editing state if applicable
            }
            if (note != null)
            {
                note.IsEditing = true;
            }
        }

        public void SaveEditedNote(Note note)
        {
            if (note != null)
            {
                note.UpdatedTime = DateTime.Now; // Update the modified time
                DatabaseHelper.Update(note);
                note.IsEditing = false; // Close edit mode

                var existingNote = Notes.FirstOrDefault(n => n.Id == note.Id);
                if (existingNote != null)
                {
                    int index = Notes.IndexOf(existingNote);
                    Notes[index] = note;
                }

                notesView.Refresh();
            }
        }

        public void DeleteNote(Note note)
        {
            if (note != null)
            {
                // Optional: Confirm deletion
                // This is typically handled in the command implementation

                DatabaseHelper.Delete(note);
                Notes.Remove(note);
            }
        }

    }
}
