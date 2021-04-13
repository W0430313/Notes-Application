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
    public class DeleteCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private ViewModels.NotesViewModel notesViewModel;

        /// <summary>
        /// Default constructor that intializes our notesViewModel
        /// </summary>
        public DeleteCommand(ViewModels.NotesViewModel notesViewModel)
        {
            this.notesViewModel = notesViewModel;
        }

        /// <summary>
        /// Can execute if the selected note is not null
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return notesViewModel.SelectedNote != null;
        }


        /// <summary>
        /// Deletes the selected file when executed
        /// </summary>
        public async void Execute(object parameter)
        {
            string noteToDelete = notesViewModel.NoteName;



            //Shows our SaveNoteDialog and waits for a response
            ContentDialog confirmDelete = new ContentDialog { Title = "Confirm Delete", Content = "Are you sure you want to delete?", PrimaryButtonText = "Confirm", SecondaryButtonText = "Cancel" };
            ContentDialogResult result = await confirmDelete.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    Repositories.NotesRepo.DeleteNoteFromDB(noteToDelete);

                    ContentDialog deleteDialog = new ContentDialog { Title = "Delete Successful", Content = "Note Deleted Successfully", PrimaryButtonText = "OK" };
                    await deleteDialog.ShowAsync();

                    notesViewModel.SelectedNote = null;


                    //Source of refreshing page after delete: https://stackoverflow.com/questions/47792071/uwp-mvvm-refresh-page-after-change-of-language
                    Frame rootframe = Window.Current.Content as Frame;
                    rootframe?.Navigate(typeof(MainPage));
                }
                catch (Exception)
                {
                    ContentDialog errorDialog = new ContentDialog { Title = "Error deleting file", Content = "There was an error deleting the file", PrimaryButtonText = "OK" };
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
