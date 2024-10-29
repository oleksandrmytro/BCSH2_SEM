using BCSH2_SEM.ViewModel.Commands;
using BCSH2_SEM.Model;
using System.ComponentModel;
using System.Windows;
using SQLite;
using BCSH2_SEM.ViewModel.Helpers;

namespace BCSH2_SEM.ViewModel
{
    public class LoginVM
    {
        private User user;

        public User User
        {
            get { return user; }
            set { user = value; }
        }

        public RegisterCommand RegisterCommand { get; set; }

        public LoginCommand LoginCommand { get; set; }

        public event EventHandler HasLoggedIn;

        public LoginVM()
        {
            User = new User();
            RegisterCommand = new RegisterCommand(this);
            LoginCommand = new LoginCommand(this);
        }

        public void Login()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(DatabaseHelper.dbFile))
            {
                conn.CreateTable<User>();

                var user = conn.Table<User>().Where(u => u.Username == User.Username).FirstOrDefault();

                if (user.Password == User.Password)
                {
                    App.UserId = user.Id.ToString();
                    HasLoggedIn(this, new EventArgs());
                }
            }
            NotesVM vm = new NotesVM();
            vm.ReadNotebooks();
            vm.ReadNotes();
        }

        public void Register()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(DatabaseHelper.dbFile))
            {
                conn.CreateTable<User>();

                var result = DatabaseHelper.Insert(User);

                if (result)
                {
                    App.UserId = User.Id.ToString();
                    HasLoggedIn(this, new EventArgs());
                }
            }
        }
    }
}