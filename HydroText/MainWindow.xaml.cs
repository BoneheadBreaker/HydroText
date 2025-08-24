using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.IO.Enumeration;

namespace HydroText
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string filename = ""; // define up here so everywhere can access it
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SaveFile()
        {
            if (!string.IsNullOrEmpty(filename))
            {
                try
                {
                    File.WriteAllText(filename, MainTextBox.Text);
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                SaveAsFile();
            }
        }
        
        private void SaveAsFile()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            dlg.Title = "Save As";

            if (dlg.ShowDialog() == true)
            {
                filename = dlg.FileName; // update the current filename
                try
                {
                    File.WriteAllText(filename, MainTextBox.Text);
                    MessageBox.Show("File saved successfully.", "Save As", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void MainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) // open folder clicked
        {
            OpenFileDialog dlg = new OpenFileDialog(); // creates an instance of the open file dialog class
            dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"; // what can be clicked syntax: "Description|Extension" 
            dlg.Title = "Open Text File"; // Title of the window

            bool? userClickedOk = dlg.ShowDialog(); // Show the dialog and wait for user to click OK or Cancel

            if (userClickedOk == true)
            {
                filename = dlg.FileName; // Full path to the file
                try
                {
                    string fileText = File.ReadAllText(filename); // opens file and reads all text
                    MainTextBox.Text = fileText;
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); // if it runs into an error it'll display it
                }
            }
        }

        private void SaveFolder_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private void SaveAsFolder_Click(object sender, RoutedEventArgs e)
        {
            SaveAsFile();
        }
    }
}