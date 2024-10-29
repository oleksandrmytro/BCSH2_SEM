using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BCSH2_SEM.Model;

namespace BCSH2_SEM.View.UserControls
{
    /// <summary>
    /// Interaction logic for NotebookControl.xaml
    /// </summary>
    public partial class NotebookControl : UserControl
    {
        public Model.Notebook DisplayNotebook
        {
            get { return (Model.Notebook)GetValue(notebookProperty); }
            set { SetValue(notebookProperty, value); }
        }

        // Using a DependencyProperty as the backing store for notebook.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty notebookProperty =
            DependencyProperty.Register("DisplayNotebook", typeof(Model.Notebook), typeof(NotebookControl), new PropertyMetadata(null, SetValues));

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NotebookControl notebookControl = d as NotebookControl;

            if (notebookControl != null)
            {
                notebookControl.DataContext = notebookControl.DisplayNotebook;
            }
        }

        public NotebookControl()
        {
            InitializeComponent();
        }
    }
}
