namespace PdfTools
{
    partial class MergeComponent
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MergeComponent));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.InputFilesListView = new System.Windows.Forms.ListView();
            this.CoverSheetSelectionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FilePathColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MergeButton = new System.Windows.Forms.Button();
            this.ExcludePagesListView = new System.Windows.Forms.ListView();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.InputFilesListView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.MergeButton, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ExcludePagesListView, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(390, 291);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // InputFilesListView
            // 
            this.InputFilesListView.AllowDrop = true;
            this.InputFilesListView.CheckBoxes = true;
            this.InputFilesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.CoverSheetSelectionColumnHeader,
            this.FilePathColumnHeader});
            this.InputFilesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InputFilesListView.FullRowSelect = true;
            this.InputFilesListView.HideSelection = false;
            this.InputFilesListView.Location = new System.Drawing.Point(3, 3);
            this.InputFilesListView.MultiSelect = false;
            this.InputFilesListView.Name = "InputFilesListView";
            this.InputFilesListView.Size = new System.Drawing.Size(189, 256);
            this.InputFilesListView.TabIndex = 0;
            this.InputFilesListView.UseCompatibleStateImageBehavior = false;
            this.InputFilesListView.View = System.Windows.Forms.View.Details;
            this.InputFilesListView.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.InputFilesListView_ItemCheck);
            this.InputFilesListView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.InputFilesListView_ItemDrag);
            this.InputFilesListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.InputFilesListView_ItemSelectionChanged);
            this.InputFilesListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.InputFilesListView_DragDrop);
            this.InputFilesListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.InputFilesListView_DragEnter);
            // 
            // CoverSheetSelectionColumnHeader
            // 
            this.CoverSheetSelectionColumnHeader.Text = "Cover Sheet";
            this.CoverSheetSelectionColumnHeader.Width = 80;
            // 
            // FilePathColumnHeader
            // 
            this.FilePathColumnHeader.Text = "File Path";
            this.FilePathColumnHeader.Width = 0;
            // 
            // MergeButton
            // 
            this.MergeButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MergeButton.Image = ((System.Drawing.Image)(resources.GetObject("MergeButton.Image")));
            this.MergeButton.Location = new System.Drawing.Point(312, 265);
            this.MergeButton.Name = "MergeButton";
            this.MergeButton.Size = new System.Drawing.Size(75, 23);
            this.MergeButton.TabIndex = 1;
            this.MergeButton.Text = "Merge";
            this.MergeButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.MergeButton.UseVisualStyleBackColor = true;
            this.MergeButton.Click += new System.EventHandler(this.MergeButton_Click);
            // 
            // ExcludePagesListView
            // 
            this.ExcludePagesListView.CheckBoxes = true;
            this.ExcludePagesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExcludePagesListView.HideSelection = false;
            this.ExcludePagesListView.Location = new System.Drawing.Point(198, 3);
            this.ExcludePagesListView.Name = "ExcludePagesListView";
            this.ExcludePagesListView.Size = new System.Drawing.Size(189, 256);
            this.ExcludePagesListView.TabIndex = 2;
            this.ExcludePagesListView.UseCompatibleStateImageBehavior = false;
            this.ExcludePagesListView.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ExcludePagesListView_ItemCheck);
            // 
            // MergeComponent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MergeComponent";
            this.Size = new System.Drawing.Size(390, 291);
            this.Load += new System.EventHandler(this.MergeComponent_Load);
            this.SizeChanged += new System.EventHandler(this.MergeComponent_SizeChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView InputFilesListView;
        private System.Windows.Forms.ColumnHeader CoverSheetSelectionColumnHeader;
        private System.Windows.Forms.ColumnHeader FilePathColumnHeader;
        private System.Windows.Forms.Button MergeButton;
        private System.Windows.Forms.ListView ExcludePagesListView;
    }
}
