using NoteApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Microsoft.Data.Sqlite;

namespace NoteApp.Repositories
{
    public class NotesRepo
    {


        // This was a private function used internally for testing purposes, when I wanted to make a change to the structure
        // of the DB or Table
        //
        //private static void DropTable()
        //{
        //    using (SqliteConnection db =
        //        new SqliteConnection("Filename=NotesDB.db"))
        //    {
        //        db.Open();

        //        String dropTable = "DROP TABLE IF EXISTS NoteTable";

        //        SqliteCommand create = new SqliteCommand(dropTable, db);
        //        create.ExecuteReader();

        //        db.Close();
        //    }
        //}

        /// <summary>
        /// Used to Initialize the Table in the Database, and only executes if the table does not exist
        /// </summary>
        public static void InitializeDB()
        {

            //DropTable();

            using (SqliteConnection db =
                new SqliteConnection("Filename=NotesDB.db"))
            {
                db.Open();

                //The table only has 2 fields, NoteName and NoteContent
                String createTable = "CREATE TABLE IF NOT EXISTS " +
                    "NoteTable ( " +
                    "NoteName nvarchar(100) NOT NULL, " +
                    "NoteContent nvarchar(500) );";

                SqliteCommand create = new SqliteCommand(createTable, db);
                create.ExecuteReader();

                db.Close();
            }
        }

        /// <summary>
        /// Adds a record to the Database
        /// </summary>
        /// <param name="noteName">The name of the note to add to the DB</param>
        /// <param name="noteContent">The content of the note to add to the DB</param>
        public static void AddRecord(String noteName, String noteContent)
        {
            //Normalizes the names of the notes
            noteName = noteName.Replace(" ", "_");
            //noteName.Replace(".txt", "");
            //noteName += ".txt";

            using (SqliteConnection db =
                new SqliteConnection("Filename=NotesDB.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Inserts the values passed in as paramaters into the DB, using parameters in the command to prevent SQL injection
                insertCommand.CommandText = "INSERT INTO NoteTable " +
                    "VALUES (@name, @content);";
                insertCommand.Parameters.AddWithValue("@name", noteName);
                insertCommand.Parameters.AddWithValue("@content", noteContent);
                insertCommand.ExecuteReader();

                db.Close();
            }
        }

        /// <summary>
        /// Gets all the notes in the DB
        /// </summary>
        /// <returns>A list of type NoteModels</returns>
        public static List<NoteModel> GetNotes()
        {
            List<NoteModel> notes = new List<NoteModel>();

            using (SqliteConnection db =
               new SqliteConnection("Filename=NotesDB.db"))
            {
                db.Open();
                
                //selects the name and content from all records in the DB
                SqliteCommand selectCommand =
                    new SqliteCommand("SELECT NoteName, NoteContent FROM NoteTable;", db);

                //saves the results of the query into an object
                SqliteDataReader query = selectCommand.ExecuteReader();

                //Runs while there are stil records to read
                while (query.Read() )
                {
                    //Create a new NoteModel each loop
                    NoteModel note = new NoteModel();

                    //Sets name and content of the temp NoteModel object to the values from the current record
                    note.Name = query.GetString(0);
                    note.Name = note.Name.Replace("_", " ");
                    //note.Name = note.Name.Replace(".txt", "");

                    note.Content = query.GetString(1);

                    //Adds the NoteModel object to our list of NoteModels
                    notes.Add(note);
                }

                db.Close();

                //Returns the List of NoteModels containing all records in the DB
                return notes;
            }
        }

        /// <summary>
        /// Deletes a note from the database
        /// </summary>
        /// <param name="noteName">The name of the note to delete</param>
        public static void DeleteNoteFromDB(String noteName)
        {
            noteName = noteName.Replace(" ", "_");
            //noteName += ".txt";


            using (SqliteConnection db =
                new SqliteConnection("Filename=NotesDB.db"))
            {
                db.Open();
                SqliteCommand deleteCommand =
                    new SqliteCommand("DELETE FROM NoteTable where NoteName = @name ;", db);

                deleteCommand.Parameters.AddWithValue("@name", noteName);
                SqliteDataReader query = deleteCommand.ExecuteReader();

                db.Close();

            }
        }


    }
}
