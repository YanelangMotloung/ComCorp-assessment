using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace ComCorpAssessment
{
    public partial class MainWindow : Form
    {
        string _filePath = string.Empty;
        Stopwatch _watch = new Stopwatch();
        DataProcessor _textProcessor = new TextProcessor();


        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            lblTime.Text = "";
        }

       
        private void btnLoad_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            rTxtWords.Text = "";
            lblTime.Text = "";
            txtWordsCount.Text = "";
            txtWordsLength.Text = "";

            string fileContent = string.Empty;
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "txt files (*.txt)|*.txt";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        _filePath = openFileDialog.FileName;

                        //Read the contents of the file into a stream
                        var fileStream = openFileDialog.OpenFile();

                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            fileContent = reader.ReadToEnd();
                        }

                        FillRichText(_filePath);
                    }
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message,"File Error" , MessageBoxButtons.OKCancel,MessageBoxIcon.Error);
            }
        }

        public void FillRichText(string aPath)
        {
            //NB// This is just for Displaying on the user side, no PROCESSING
            string content = File.ReadAllText(aPath);
            richTextBox1.Text = content;
        }

        private async void btnProcess(object sender, EventArgs e)
        {
            rTxtWords.Text = "";
            lblTime.Text = "";

            int _wordsCount = 0;
            int _wordsLength = 0;
            
            string _words = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(_filePath))
                {
                    var stringData = File.ReadAllLines(_filePath);
                    if ((Int32.TryParse(txtWordsCount.Text, out _wordsCount)) && (Int32.TryParse(txtWordsLength.Text, out _wordsLength)))
                    {
                        _watch.Restart(); // Start the counter from where it stopped

                        await Task.Run(() =>
                        {
                            var data =_textProcessor.ProcessData(stringData, _wordsCount, _wordsLength);
                            
                            foreach (var item in data)
                            {
                                _words += item + "\n";
                            }
                        });
                        
                        _watch.Stop();
                        lblTime.Text = _watch.ElapsedMilliseconds.ToString();
                        _watch.Reset();

                        rTxtWords.Text = _words;
                    }
                    else if ((Int32.TryParse(txtWordsCount.Text, out _wordsCount)) && (!Int32.TryParse(txtWordsLength.Text, out _wordsLength)))
                    {
                        _watch.Restart(); // Start the counter from where it stopped

                        await Task.Run(() =>
                        {
                            var data = _textProcessor.ProcessData(stringData, _wordsCount);

                            foreach (var item in data)
                            {
                                _words += item + "\n";
                            }
                          
                        });

                        _watch.Stop();
                        lblTime.Text = _watch.ElapsedMilliseconds.ToString();
                        _watch.Reset();

                        rTxtWords.Text = _words;
                    }
                    else
                    {
                        MessageBox.Show(" Please populate the valid words count and words length", "File Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(" The file path is empty", "File Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "File Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
    }
}
