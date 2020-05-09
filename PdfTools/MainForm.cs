using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using PdfToolsLibrary;

namespace PdfTools
{
    public partial class MainForm : Form
    {
        private const string PdfOpenFileDialogFilter  = "PDF files (*" 
            + Constants.MediaExtension + ") | *" + Constants.MediaExtension;

        private OpenFileDialog MergeFileDialog { get; set; }
        private MergeComponent MergeTool { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MergeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MergeFileDialog == null) SetupMergeFileDialog();

            MergeFileDialog.FileName = "";
            MergeFileDialog.ShowDialog(this);   
        }

        private void SetupMergeFileDialog()
        {
            MergeFileDialog = new OpenFileDialog()
            {
                Title = "Select PDFs to Merge",
                DefaultExt = Constants.MediaExtension,
                Filter = PdfOpenFileDialogFilter,
                InitialDirectory = Environment.CurrentDirectory,
                Multiselect = true,
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                RestoreDirectory = true
            };
            MergeFileDialog.FileOk += MergeFileDialog_FileOk;
        }

        private void MergeFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            if (MergeTool == null) SetupMergeTool();

            MergeTool.SetInputFiles(
                MergeFileDialog.FileNames, MergeFileDialog.SafeFileNames);
            MainPanel.Controls.Add(MergeTool);
        }   

        private void SetupMergeTool() => MergeTool = new MergeComponent();
    }
}
