using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Win32;
using MyNet.Xaml.Html;

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
                case ".txt": SaveAsTxt(); break;
                case ".rtf": SaveAsRtf(); break;
                case ".hdt": SaveAsHdt(); break;
                case ".html": SaveAsHtml(); break;
                default: SaveAsTxt(); break;
            }
        }

        private void SaveAsFile()
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|Rich Text Files (*.rtf)|*.rtf|Hydro Text Files (*.hdt)|*.hdt|HTML Files (*.html)|*.html|All Files (*.*)|*.*",
                Title = "Save As",
                DefaultExt = "txt"
            };

            bool? result = dlg.ShowDialog();
            if (result != true) return; // user canceled

            filename = dlg.FileName;

            // Add default extension if user didn't type one
            string ext = Path.GetExtension(filename).ToLower();
            if (string.IsNullOrEmpty(ext))
            {
                switch (dlg.FilterIndex)
                {
                    case 1: ext = ".txt"; break;
                    case 2: ext = ".rtf"; break;
                    case 3: ext = ".hdt"; break;
                    case 4: ext = ".html"; break;
                    default: ext = ".txt"; break;
                }
                filename += ext;
            }

            SaveFile();
        }

        private void SaveAsTxt()
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
                using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
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

        private void SaveAsHdt()
        {
            try
            {
                TextRange range = new TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd);
                using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
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

        private void SaveAsHtml()
        {
            try
            {
                // Make sure the document is not empty
                if (MainTextBox.Document.Blocks.Count == 0)
                {
                    MainTextBox.Document.Blocks.Add(new Paragraph(new Run("")));
                }

                // Convert the FlowDocument to XAML string
                string xaml = System.Windows.Markup.XamlWriter.Save(MainTextBox.Document);

                // Convert XAML string to HTML
                string html = MyNet.Xaml.Html.HtmlFromXamlConverter.ConvertXamlToHtml(xaml);

                // Save HTML to file
                File.WriteAllText(filename, html);

                MessageBox.Show("File saved as HTML successfully.", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving HTML file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

            string ext = Path.GetExtension(filename).ToLower();
            try
            {
                switch (ext)
                {
                    case ".txt":
                        LoadTxt(filename);
                        break;
                    case ".rtf":
                        LoadRtf(filename);
                        break;
                    case ".hdt":
                        LoadHdt(filename);
                        break;
                    case ".html":
                        
                        break;
                    default:
                        LoadTxt(filename);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadTxt(string path)
        {
            MainTextBox.Document.Blocks.Clear();
            string text = File.ReadAllText(path);
            MainTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));
        }

        private void LoadRtf(string path)
        {
            MainTextBox.Document.Blocks.Clear();
            TextRange range = new TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd);
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                range.Load(fs, DataFormats.Rtf);
            }
        }

        private void LoadHdt(string path)
        {
            MainTextBox.Document.Blocks.Clear();
            TextRange range = new TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd);
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                range.Load(fs, DataFormats.Xaml);
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