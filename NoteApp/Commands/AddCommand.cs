using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace NoteApp
{
    public class AddCommand : ICommand 
    {
        public event EventHandler CanExecuteChanged;
        private ViewModels.NotesViewModel notesViewModel;


        /// <summary>
        /// Default constructor that intializes our notesViewModel
        /// </summary>
        public AddCommand(ViewModels.NotesViewModel notesViewModel)
        {
            this.notesViewModel = notesViewModel;
        }


        /// <summary>
        /// Can always execute
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return true;
        }


        /// <summary>
        /// Makes the note text box editable and clears the textbox when executed
        /// </summary>
        public void Execute(object parameter)
        {
            //TextBox noteTextBox = (TextBox)parameter;
            //noteTextBox.IsReadOnly = false;

            MainPage.noteTextBox.IsReadOnly = false;
            //notesViewModel.NoteText = "";
            MainPage.noteTextBox.Focus(Windows.UI.Xaml.FocusState.Keyboard);
        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
