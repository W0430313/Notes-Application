using NoteApp.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NoteApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public ViewModels.NotesViewModel NotesViewModel { get; set; }
        public static TextBox noteTextBox;
        public AboutCommand aboutCommand;
        public ExitCommand exitCommand;

        public MainPage()
        {
            this.InitializeComponent();
            noteTextBox = NoteTextBox;
            this.NotesViewModel = new ViewModels.NotesViewModel();
            aboutCommand = new AboutCommand();
            exitCommand = new ExitCommand();
            
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            
        }




    }
}
