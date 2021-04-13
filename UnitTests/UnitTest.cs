using NoteApp;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoteApp.Models;
using NoteApp.ViewModels;
using Windows.Storage;
using System.Collections.Generic;
using NoteApp.Repositories;
using Microsoft.Data.Sqlite;
using System.Diagnostics;

namespace UnitTests
{

    #region "Part 3 Tests"
    //[TestClass]
    //All Tests From Part 3
    //public class UnitTest1
    //{
    //    /// <summary>
    //    /// Test to see if the ViewModels variables containing the selected notes name and content get updated when the selected note changes
    //    /// </summary>
    //    [TestMethod]
    //    public void Test_SelectedNote_UpdatesViewModel()
    //    {
    //        //create a new note model with some dummy data
    //        NoteModel nm = new NoteModel();
    //        nm.Content = "Testing the view model";
    //        nm.Name = "Test";

    //        //create a note view model, and set the selected note equal to the notemodel we just created
    //        NotesViewModel nvm = new NotesViewModel();
    //        nvm.SelectedNote = nm;

    //        //compare the selected note and the view models variables
    //        Assert.AreEqual(nm.Content, nvm.NoteText);
    //    }

    //    /// <summary>
    //    /// Test to see if the collection of notes gets filled when the NoteViewModel gets instantiated
    //    /// </summary>
    //    [TestMethod]
    //    public void Test_NotesCollection_RetrievesNotes()
    //    {

    //        NotesViewModel nvm = new NotesViewModel();

    //        //right now my local packages has ONE file in it, yours will likely be different, so change this accordingly
    //        //also, this is in a different package/location than the noteapp, so the files will be different as well
    //        int expected = 1;

    //        //get the count of the notes collection to see how many notes are in it
    //        int result = nvm.Notes.Count;

    //        //compare to see if they are equal
    //        Assert.AreEqual(expected, result);
    //    }

    //    /// <summary>
    //    /// Test to make sure a note can be added
    //    /// </summary>
    //    [TestMethod]
    //    public void Test_AddNewNote()
    //    {
    //        //create a new NVM with some dummy data
    //        NotesViewModel nvm = new NotesViewModel();
    //        nvm.NoteText = "Add new note";
    //        nvm.NoteName = "Test Note";

    //        //the expected result should be the amount of notes we have in our collection + 1, accounting for the note we're about to add
    //        int expected = nvm.Notes.Count + 1;

    //        //Call our notes repo to create and save the file
    //        NotesRepo.SaveNoteToFile(nvm.NoteName, nvm.NoteText);

    //        //to ensure the count refreshes, get a new NVM
    //        NotesViewModel nvm2 = new NotesViewModel();
    //        int result = nvm2.Notes.Count;

    //        //compare the counts of the two collections of notes to see if our note got added
    //        Assert.AreEqual(expected, result);
    //    }

    //    [TestMethod]
    //    public void Test_DeleteNote()
    //    {
    //        //create a note view model
    //        NotesViewModel nvm = new NotesViewModel();
    //        nvm.NoteName = "Test Note";

    //        //the expected result should be the length of the notes collection minus 1, accounting for the note we're about to delete
    //        int expected = nvm.Notes.Count - 1;

    //        //call our repo to delete the note, using the file name from our add note test
    //        NotesRepo.DeleteNote(nvm.NoteName);

    //        //get a new nvm to ensure the count is updated
    //        NotesViewModel nvm2 = new NotesViewModel();
    //        int result = nvm2.Notes.Count;

    //        //compare the counts of the two collections to see if the note got deleted
    //        Assert.AreEqual(expected, result);
    //    }
    //}

    #endregion

    [TestClass]
    public class UnitTest2
    {

        
        /// <summary>
        /// Test Method to ensure that we can make a connection to the SQLite database
        /// </summary>
        [TestMethod]
        public void Test_DbConnection()
        {
            //Calls function to initialize the database if the database does not already exist
            NotesRepo.InitializeDB();

            //Opens an sqlite connection to the same DB used in NotesRepo.InitializeDB()
            using (SqliteConnection db =
             new SqliteConnection("Filename=NotesDB.db"))
            {
                db.Open();

                //The test passes if the database object is not null
                Assert.IsNotNull(db);
                
                db.Close();
            }
        }

        /// <summary>
        /// Test Method to test adding a new record to the database
        /// </summary>
        [TestMethod]
        public void Test_AddNewRecord()
        {
            //Calls a function to get the count of the records in the database
            //Since we are going to be adding a new record, the expected result is that + 1
            int expected = GetCountOfRecords()+1;

            //Calls the AddRecord() method in the NotesRepo to add a new note to the DB
            NotesRepo.AddRecord("Test Inserting Note", "Testing to see if we can add a new note to the DB");

            //Gets the count of records in the DB after the insert
            int result = GetCountOfRecords();

            //The test passes if the initial count of the DB + 1 is equal to the count of the DB after inserting a record
            Assert.AreEqual(expected, result);
        }


        /// <summary>
        /// Test method to test deleting a record from the database
        /// </summary>
        [TestMethod]
        public void Test_DeleteRecord()
        {
            //First we add a new dummy note that we'll try to delete after
            NotesRepo.AddRecord("Test Deleting A Note", "");

            //After we add the dummy note, the expected result is the count of records - 1
            int expected = GetCountOfRecords() - 1;

            //Calls function to delete the note we just made from the DB
            NotesRepo.DeleteNoteFromDB("Test Deleting A Note");

            //Gets the count of records in the DB after the delete
            int result = GetCountOfRecords();

            //Compares the two values, and passes the test if they're equal
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Test method to test the Notes collection in the NotesViewModel gets populated by the DB
        /// </summary>
        [TestMethod]
        public void Test_ViewModelList_GetsNotesFromDB()
        {
            //The expected result is the count of records in the DB
            int expected = GetCountOfRecords();

            //Instantiate a new NoteViewModel
            //The method to populate the Notes collection gets called in the constructor
            NotesViewModel nvm = new NotesViewModel();

            //Gets the count of the Notes collection
            int result = nvm.Notes.Count;
            
            //Compares the two values, and passes if they're equal
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Private Method to get the count of records currently in the DB
        /// </summary>
        /// <returns>An int containing the number of records in the DB</returns>
        private int GetCountOfRecords()
        {
            int count = 0;

            using (SqliteConnection db =
                new SqliteConnection("Filename=NotesDB.db"))
            {
                
                db.Open();

                
                SqliteCommand countCommand = new SqliteCommand();
                countCommand.Connection = db;

                //Uses the aggregate function COUNT() to get the records in the Table
                countCommand.CommandText = "SELECT COUNT(*) FROM NoteTable ";

                //Saves the results of the query
                SqliteDataReader query = countCommand.ExecuteReader();

                //Since we used COUNT(), the returning object should only have 1 record, so the while loop while only run 1 time
                while(query.Read() )
                {
                    //COUNT() returns only the number of records in the table, so we read the 1st column in the results of the query
                    count = int.Parse( query.GetString(0) );
                }


                db.Close();
            }

            return count;

        }
    }
}
