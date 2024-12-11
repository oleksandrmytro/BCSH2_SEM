using System.Windows;
using System.Windows.Controls;
using BCSH2_SEM.Model;

namespace BCSH2_SEM.View.UserControls
{
    public partial class NoteControl : UserControl
    {
        public NoteControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty NoteProperty =
            DependencyProperty.Register("Note", typeof(Note), typeof(NoteControl), new PropertyMetadata(null, OnNoteChanged));

        public Note Note
        {
            get { return (Note)GetValue(NoteProperty); }
            set { SetValue(NoteProperty, value); }
        }

        private static void OnNoteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as NoteControl;
            if (control != null && e.NewValue is Note note)
            {
                control.DataContext = note;
            }
        }
    }
}