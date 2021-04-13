using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteApp.Models
{
    public class NoteModel
    {
        /// <summary>
        /// The text inside of a note
        /// </summary>
        public string Content;

        /// <summary>
        /// The name of a given note
        /// </summary>
        public string Name;


        /// <summary>
        /// Default constructor that on creation sets the content of a note to be an empty string and the name of a note to be untitled 
        /// </summary>
        public NoteModel()
        {
            Content = "";
            Name = "Untitled";
        }
    }
}
