using NoteApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace NoteApp.ViewModels
{
    public class NotesViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// SaveCommand instance that implements the ICommand interface and is used to save a note to a file
        /// </summary>
        public SaveCommand SaveCommand { get; }
        /// <summary>
        /// EditCommand instance that implements the ICommand interface and is used to edit the contents of a note
        /// </summary>
        public EditCommand EditCommand { get; }

        /// <summary>
        /// DeleteCommand instance that implements the ICommand interface and is used to delete the file a note is associated with
        /// </summary>
        public DeleteCommand DeleteCommand { get; }

        /// <summary>
        /// AddCommand instance that implements the ICommand interface and is used to create a new note
        /// </summary>
        public AddCommand AddCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// A public collection of NoteModels that returns the filtered selection of notes
        /// </summary>
        public ObservableCollection<NoteModel> Notes { get; set; }



        private string _noteText;
        /// <summary>
        /// The content of the currently selected note
        /// </summary>
        public string NoteText
        {
            get
            {
                return _noteText;
            }
            set
            {
                
                _noteText = value;
                SaveCommand.FireCanExecuteChanged();
                EditCommand.FireCanExecuteChanged();
                DeleteCommand.FireCanExecuteChanged();
                AddCommand.FireCanExecuteChanged();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NoteText"));
            }
        }

        /// <summary>
        /// The name of the currently selected note
        /// </summary>
        public string NoteName { get; set; }

        /// <summary>
        /// Private NoteModel used internally to update the currently selected note
        /// </summary>
        private NoteModel _selectedNote;

        /// <summary>
        /// Private string used to internally filter the notes
        /// </summary>
        private string _filter;




        /// <summary>
        /// Private list of NoteModels that contains all notes in the WindowsStorage folder
        /// </summary>
        private List<NoteModel> _allNotes = new List<NoteModel>();

        /// <summary>
        /// Default constructor that takes no paramaters, creates instances of our Commands, and populates our Notes collection
        /// </summary>
        public NotesViewModel()
        {
            //create instances of our commands
            SaveCommand = new SaveCommand(this);
            EditCommand = new EditCommand(this);
            DeleteCommand = new DeleteCommand(this);
            AddCommand = new AddCommand(this);

            //create instance of our ObservableCollection of Notes
            //Notes = new ObservableCollection<NoteModel>();

            _allNotes = Repositories.NotesRepo.GetNotes();
            Notes = new ObservableCollection<NoteModel>(_allNotes);

            //Calls a private function to populate our Notes and _allNotes collections
            //GetNotes();
            
        }

        /// <summary>
        /// Public string that's Two-Way bound to determine our filtered notes
        /// </summary>
        public string Filter
        {
            get
            { 
                return _filter; 
            }
            set
            {
                if(value == _filter)
                {
                    return;
                }
                _filter = value;
                PerformFiltering();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Filter)));
            }
        }

        /// <summary>
        /// Public NoteModel that's Two-Way bound to represent the currently selected note
        /// </summary>
        public NoteModel SelectedNote
        {
            get 
            {
                return _selectedNote; 
            }
            set
            {
                _selectedNote = value;
                if(value == null)
                {
                    NoteText = "";   
                }
                else
                {
                    NoteText = value.Content;
                    NoteName = value.Name;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NoteText"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NoteName"));
                SaveCommand.FireCanExecuteChanged();
                EditCommand.FireCanExecuteChanged();
                DeleteCommand.FireCanExecuteChanged();
                AddCommand.FireCanExecuteChanged();
            }
        }

        /// <summary>
        /// Private function to filter our public Notes collection
        /// </summary>
        private void PerformFiltering()
        {
            if(_filter == null)
            {
                _filter = "";
            }

            //Gets the lowercase variant of our filter
            var lowerCaseFilter = Filter.ToLowerInvariant().Trim();

            //Gets a list containing all the notes where the name of the note matches the filtered criteria
            var result = _allNotes.Where(n => n.Name.ToLowerInvariant().Contains(lowerCaseFilter)).ToList();

            //Gets a list of notes to remove from our public collection
            var toRemove = Notes.Except(result).ToList();

            //Removes all notes not meeting our filter criteria from our Notes collection
            foreach(var x in toRemove)
            {
                Notes.Remove(x);
            }

            var resultCount = result.Count;

            for(int x = 0; x < resultCount; x++)
            {
                var resultItem = result[x];

                if(x + 1 > Notes.Count || !Notes[x].Equals(resultItem))
                {
                    Notes.Insert(x, resultItem);
                }
            }
        }

    }
}
