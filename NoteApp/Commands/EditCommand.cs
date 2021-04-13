using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace NoteApp
{
    public class EditCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private ViewModels.NotesViewModel notesViewModel;

        /// <summary>
        /// Default constructor that intializes our notesViewModel
        /// </summary>
        public EditCommand(ViewModels.NotesViewModel notesViewModel)
        {
            this.notesViewModel = notesViewModel;
        }

        /// <summary>
        /// Can execute if the selected node is not null
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return notesViewModel.SelectedNote != null;
        }

        /// <summary>
        /// When executed, sets the textbox containing the notes text to be editable
        /// </summary>
        public void Execute(object parameter)
        {

            MainPage.noteTextBox.IsReadOnly = false;
        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
