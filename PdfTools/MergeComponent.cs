using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfTools.Properties;
using PdfToolsLibrary;

namespace PdfTools
{
    public partial class MergeComponent : UserControl
    {
        private const int NameColumnIndex = 0;
        private const int PathColumnIndex = 1;

        #region Private Properties

        private List<string> InputFilePaths { get; set; }
        private List<string> InputSafeFileNames { get; set; }
        private int CoverSheetIndex { get; set; }
        private string ExclusionWorkingPath { get; set; }
        private Dictionary<string, HashSet<int>> Exclusions { get; set; }

        #endregion

        public MergeComponent() => InitializeComponent();

        #region Public Interface

        public void SetInputFiles(string[] filePaths, string[] safeFileNames)
        {
            ResetSecondaryDetails();
            TestFiles(filePaths.ToList(), safeFileNames.ToList());
            ListFiles();
        }

        #endregion

        #region Private Helper Methods

        private void TestFiles(List<string> filePaths, List<string> safeFileNames)
        {
            if (filePaths.Count != safeFileNames.Count)
            {
                throw new ApplicationException(
                    $"File Paths ({filePaths?.Count ?? 0}) and Safe File Names ({safeFileNames?.Count ?? 0}) must contain the same number of entries");
            }

            var remove = new List<KeyValuePair<int, string>>();

            for (var idx = 0; idx < filePaths.Count; idx++)
            {
                var path = filePaths[idx];

                if(PdfReader.TestPdfFile(path) == 0)
                {
                    remove.Add(new KeyValuePair<int, string>(idx, "Not a valid PDF file"));

                    continue;
                }

                try
                {
                    using (var pdf = PdfReader.Open(
                        path,
                        PdfDocumentOpenMode.InformationOnly)) ;
                }
                catch (PdfReaderException ex)
                {
                    remove.Add(new KeyValuePair<int, string>(idx, ex.Message));
                }
            }

            if (remove.Any())
            {
                remove.Select(r => r.Key)
                    .OrderByDescending(r => r)
                    .ToList()
                    .ForEach(idx =>
                    {
                        filePaths.RemoveAt(idx);
                        safeFileNames.RemoveAt(idx);
                    });
                MessageBox.Show(
                        "The following PDFs were removed as invalid or inaccessible:\n"
                        + string.Join("\n", remove.Select(r => r.Value)),
                        "Invalid or Inaccessible PDF Detected",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }

            InputFilePaths = new List<string>(filePaths);
            InputSafeFileNames = new List<string>(safeFileNames);
        }

        private void ResetSecondaryDetails()
        {
            ExclusionWorkingPath = null;
            Exclusions = new Dictionary<string, HashSet<int>>();
            CoverSheetIndex = -1;
        }

        private void ListFiles()
        {
            var icon = Resources.pdf;
            var group = new ListViewGroup("Input Files");

            InputFilesListView.SmallImageList = new ImageList();
            InputFilesListView.SmallImageList.Images.Add(icon);
            InputFilesListView.LargeImageList = new ImageList();
            InputFilesListView.LargeImageList.Images.Add(icon);
            InputFilesListView.Items.Clear();

            for (var idx = 0; idx < InputSafeFileNames.Count; idx++)
            {
                var lvi = new ListViewItem(InputSafeFileNames[idx], 0, group);

                lvi.SubItems.Add(InputFilePaths[idx]);
                InputFilesListView.Items.Add(lvi);
            }
        }

        private void ListPages(PdfPages pages)
        {
            var icon = Resources.page;
            var group = new ListViewGroup("Pages");

            ExcludePagesListView.SmallImageList = new ImageList();
            ExcludePagesListView.SmallImageList.Images.Add(icon);
            ExcludePagesListView.LargeImageList = new ImageList();
            ExcludePagesListView.LargeImageList.Images.Add(icon);

            for (var idx = 0; idx < pages.Count; idx++)
            {
                var lvi = new ListViewItem($"Page {idx + 1}", 0, group);

                lvi.SubItems.Add(idx.ToString());
                lvi.Checked = Exclusions.ContainsKey(ExclusionWorkingPath)
                    ? Exclusions[ExclusionWorkingPath].Contains(idx + 1) : false;
                ExcludePagesListView.Items.Add(lvi);
            }
        }

        #endregion Private Helper Methods

        #region Event Hanlders

        private void MergeComponent_Load(object sender, EventArgs e) 
            => Dock = DockStyle.Fill;

        private void MergeComponent_SizeChanged(object sender, EventArgs e)
            => InputFilesListView.Columns[PathColumnIndex].Width
                = InputFilesListView.Width
                - InputFilesListView.Columns[NameColumnIndex].Width
                - InputFilesListView.Margin.Left
                - InputFilesListView.Margin.Right;

        private void InputFilesListView_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                if (CoverSheetIndex > -1)
                {
                    InputFilesListView.Items[CoverSheetIndex].Checked = false;
                }

                CoverSheetIndex = e.Index;
            }
            else CoverSheetIndex = -1;
        }

        private void InputFilesListView_ItemDrag(object sender, ItemDragEventArgs e)        
            => InputFilesListView.DoDragDrop(e.Item, DragDropEffects.Move);

        private void InputFilesListView_DragEnter(object sender, DragEventArgs e)        
            => e.Effect = DragDropEffects.Move;

        private void InputFilesListView_DragDrop(object sender, DragEventArgs e)
        {
            if (InputFilesListView.SelectedItems.Count > 1)
            {
                throw new ApplicationException("Only single drag and drop is supported.");
            }

            if (InputFilesListView.SelectedItems.Count < 1) return;

            var dragItem = InputFilesListView.SelectedItems[0];
            var point = InputFilesListView.PointToClient(new Point(e.X, e.Y));
            var dropItem = InputFilesListView.GetItemAt(point.X, point.Y);

            if (dragItem == dropItem) return;

            var clone = (ListViewItem)dragItem.Clone();
            var checkedItem = CoverSheetIndex == -1
                ? null
                : dragItem.Index == CoverSheetIndex 
                    ? clone 
                    : InputFilesListView.Items[CoverSheetIndex];

            if (dropItem == null)
            {
                InputFilesListView.Items.Add(clone);
            }
            else
            {
                InputFilesListView.Items.Insert(
                    dropItem.Index + (dragItem.Index < dropItem.Index ? 1 : 0), 
                    clone);
            }

            InputFilesListView.Items.Remove(dragItem);

            if (checkedItem != null)
            {
                CoverSheetIndex = InputFilesListView.Items.IndexOf(checkedItem);
            }
        }

        private void MergeButton_Click(object sender, EventArgs e)
        {
            if (InputFilesListView.Items.Count < 1)
            {
                MessageBox.Show(
                    $"There are no valid input files to merge.",
                    "Merge Aborted",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            var inputDocuments = new List<InputDocumentData>();
            var coverSheetFound = false;

            for (var idx = 0; idx < InputFilesListView.Items.Count; idx++)
            {
                var item = InputFilesListView.Items[idx];
                var path = item.SubItems[PathColumnIndex].Text;
                var isCoverSheet = item.Checked;
                var order = isCoverSheet 
                    ? 0 
                    : coverSheetFound 
                        ? item.Index - 1 
                        : item.Index;

                inputDocuments.Add(new InputDocumentData(
                    path, 
                    item.Checked, 
                    Exclusions.ContainsKey(path) 
                        ? Exclusions[path].ToList() : null, 
                    order));
                coverSheetFound |= isCoverSheet;
            }

            var outputDocuments = string.Join(
                "\n", new MergeTool(inputDocuments).Merge());

            MessageBox.Show(
                $"The following files were generated:\n{outputDocuments}", 
                "Merge Complete", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
        }

        private void InputFilesListView_ItemSelectionChanged(
            object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ExcludePagesListView.Items.Clear();
            ExclusionWorkingPath = null;

            if (!e.IsSelected) return;

            ExclusionWorkingPath = e.Item.SubItems[PathColumnIndex].Text;
            PdfPages pages;

            try
            {
                using (var pdf = PdfReader.Open(
                    ExclusionWorkingPath,
                    PdfDocumentOpenMode.InformationOnly))
                    pages = (PdfPages)pdf.Pages.Clone();

                ListPages(pages);
            }
            catch(PdfReaderException ex)
            {
                MessageBox.Show(
                    ex.Message, 
                    "Error opening PDF", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void ExcludePagesListView_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!Exclusions.ContainsKey(ExclusionWorkingPath))
            {
                Exclusions.Add(ExclusionWorkingPath, new HashSet<int>());
            }

            var pageNum = e.Index + 1;

            if (e.NewValue != CheckState.Checked) Exclusions[ExclusionWorkingPath].Remove(pageNum);
            else Exclusions[ExclusionWorkingPath].Add(pageNum);
        }

        #endregion Event Handlers
    }
}
