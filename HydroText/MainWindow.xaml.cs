using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Win32;

namespace HydroText
{
    public partial class MainWindow : Window
    {
        private string filename = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SaveFile()
        {
            if (string.IsNullOrEmpty(filename))
            {
                SaveAsFile();
                return;
            }

            string ext = Path.GetExtension(filename).ToLower();
            switch (ext)
            {
                case ".txt": SaveAsText(); break;
                case ".rtf": SaveAsRtf(); break;
                case ".hdt": SaveAsCustom(); break;
                default: SaveAsText(); break;
            }
        }

        private void SaveAsFile()
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|Rich Text Files (*.rtf)|*.rtf|Hydro Text Files (*.hdt)|*.hdt|All Files (*.*)|*.*",
                Title = "Save As",
                DefaultExt = "txt"
            };

            bool? result = dlg.ShowDialog();
            if (result != true) return; // user canceled

            filename = dlg.FileName;

            // Add default extension if user didnt type one
            string ext = Path.GetExtension(filename).ToLower();
            if (string.IsNullOrEmpty(ext))
            {
                switch (dlg.FilterIndex)
                {
                    case 1: ext = ".txt"; break;
                    case 2: ext = ".rtf"; break;
                    case 3: ext = ".hdt"; break;
                    default: ext = ".txt"; break;
                }
                filename += ext;
            }

            SaveFile();
        }


        private void SaveAsText()
        {
            try
            {
                TextRange range = new TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd);
                File.WriteAllText(filename, range.Text);
                MessageBox.Show("File saved as TXT successfully.", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving TXT file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveAsRtf()
        {
            try
            {
                TextRange range = new TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd);
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    range.Save(fs, DataFormats.Rtf);
                }
                MessageBox.Show("File saved as RTF successfully.", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving RTF file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveAsCustom()
        {
            try
            {
                TextRange range = new TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd);
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    range.Save(fs, DataFormats.Xaml);
                }
                MessageBox.Show("File saved as Hydro Text successfully.", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving Hydro Text file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|Rich Text Files (*.rtf)|*.rtf|Hydro Text Files (*.hdt)|*.hdt|All Files (*.*)|*.*",
                Title = "Open File"
            };

            bool? result = dlg.ShowDialog();
            if (result != true) return;

            filename = dlg.FileName;

            try
            {
                TextRange range = new TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd);

                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    string ext = Path.GetExtension(filename).ToLower();

                    if (ext == ".rtf")
                        range.Load(fs, DataFormats.Rtf);
                    else if (ext == ".hdt")
                        range.Load(fs, DataFormats.Xaml);
                    else
                        range.Load(fs, DataFormats.Text);
                }
            }
            catch (Exception ex) // catch everything to avoid crashing
            {
                MessageBox.Show($"Error reading file: {ex.Message} ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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