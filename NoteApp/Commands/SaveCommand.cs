using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NoteApp
{
    public class SaveCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private ViewModels.NotesViewModel notesViewModel;

        /// <summary>
        /// Default constructor that intializes our notesViewModel
        /// </summary>
        public SaveCommand(ViewModels.NotesViewModel notesViewModel)
        {
            this.notesViewModel = notesViewModel;
        }


        /// <summary>
        /// Command can execute if the selected note is not null and the note text box is not read only
        /// </summary>
        public bool CanExecute(object parameter)
        {

            return MainPage.noteTextBox.IsReadOnly == false;
        }

        /// <summary>
        /// Shows a dialog to get a file name and then saves the note to a file then refreshes the application when executed
        /// </summary>
        public async void Execute(object parameter)
        {
            //Shows our SaveNoteDialog and waits for a response
            SaveNoteDialog saveNote = new SaveNoteDialog();
            ContentDialogResult result = await saveNote.ShowAsync();

            if(result == ContentDialogResult.Primary)
            {

                try
                {
                    Repositories.NotesRepo.AddRecord(saveNote.NoteName, notesViewModel.NoteText);

                    ContentDialog savedDialog = new ContentDialog { Title = "Save Successful", Content = "Note Saved Successfully", PrimaryButtonText = "OK" };
                    await savedDialog.ShowAsync();

                    //Source of refreshing page after add: https://stackoverflow.com/questions/47792071/uwp-mvvm-refresh-page-after-change-of-language
                    Frame rootframe = Window.Current.Content as Frame;
                    rootframe?.Navigate(typeof(MainPage));
                }
                catch (Exception e)
                {
                    ContentDialog errorDialog = new ContentDialog
                    {
                        Title = "Error Saving File",
                        Content = "There was an error saving the file, please try again",
                        PrimaryButtonText = "OK"
                    };

                    await errorDialog.ShowAsync();
                }
            }
        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
