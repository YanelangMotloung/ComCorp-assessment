using ComCorpAssessment.Controllers;
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
        int _linesCount { get; set; }
        FileController _file { get; set; }
        Stopwatch _watch = new Stopwatch();
        Stopwatch _firstFitywatch = new Stopwatch();
        Stopwatch _firstFiftyAbovewatch = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            label6.Text = "";
            label8.Text = "";
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            richTextBox3.Text = "";

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
            _linesCount = content.Length;
            richTextBox1.Text = content;
        }

        private async void btnProcess(object sender, EventArgs e)
        {
            

                //await Task.Run(() =>
                //{
                //    for (var i = 0; i <= 1000000; i++)
                //    {
                //        UpdateUI(i);
                //        count = i;
                //    }
                //});
            
            string _firstFiftyword = string.Empty;
            string _firstFiftywordAbove = string.Empty;
            Dictionary<string, int> _words = new Dictionary<string, int>(); 

            try
            {
                if (!string.IsNullOrEmpty(_filePath))
                {
                    _file = new FileController(_filePath,_linesCount);

                    _watch.Start();
                    await Task.Run(() =>
                    {
                        _words = _file.CountWords(_file._words);

                    });

                    _watch.Stop();

                    MessageBox.Show($"Execution Time: {_watch.ElapsedMilliseconds} ms for counting words");




                    
                    //Getting the top 50
                    _firstFitywatch.Start();
                    await Task.Run(() =>
                    {
                        var _firstFifty = _file.GetSpecifiedNumberOfbjects(_words, 50);
                        foreach (var item in _firstFifty)
                        {
                            _firstFiftyword += item + "\n";
                        }

                    });
                    richTextBox2.Text = _firstFiftyword;

                    _firstFitywatch.Stop();

                    label5.Show();
                    label6.Text = "" + _firstFitywatch.ElapsedMilliseconds + " ms";

                    







                    //getting top 50 above the lenght of 6
                    _firstFiftyAbovewatch.Start();

                    await Task.Run(() =>
                    {
                        var _fisrtFityAboveSix = _file.GetSpecifiedNumberOfbjects(_words, 50, 6);
                        foreach (var item in _fisrtFityAboveSix)
                        {
                            _firstFiftywordAbove += item + "\n";
                        }

                    }); 

                    _firstFiftyAbovewatch.Stop();
                    label7.Show();
                    label8.Text = "" + _firstFiftyAbovewatch.ElapsedMilliseconds + " ms";

                    
                    richTextBox3.Text = _firstFiftywordAbove;
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
