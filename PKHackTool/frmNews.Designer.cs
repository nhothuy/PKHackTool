namespace PKTool
{
    partial class frmNews
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNews));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FB = new System.Windows.Forms.DataGridViewLinkColumn();
            this.grdNews = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdNews)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.grdNews, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(541, 304);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Message
            // 
            this.Message.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Message.DefaultCellStyle = dataGridViewCellStyle1;
            this.Message.HeaderText = "Message";
            this.Message.Name = "Message";
            this.Message.ReadOnly = true;
            this.Message.ToolTipText = "Message";
            // 
            // ID
            // 
            this.ID.HeaderText = "Name";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 80;
            // 
            // FB
            // 
            this.FB.HeaderText = "FBID";
            this.FB.Name = "FB";
            this.FB.ReadOnly = true;
            this.FB.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.FB.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.FB.Text = "FBID";
            this.FB.ToolTipText = "Click here to open facebook";
            this.FB.Width = 120;
            // 
            // grdNews
            // 
            this.grdNews.AllowUserToAddRows = false;
            this.grdNews.AllowUserToDeleteRows = false;
            this.grdNews.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.grdNews.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdNews.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FB,
            this.ID,
            this.Message});
            this.grdNews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdNews.Location = new System.Drawing.Point(3, 3);
            this.grdNews.Name = "grdNews";
            this.grdNews.ReadOnly = true;
            this.grdNews.RowHeadersVisible = false;
            this.grdNews.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdNews.Size = new System.Drawing.Size(535, 258);
            this.grdNews.TabIndex = 0;
            this.grdNews.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdNews_CellContentClick);
            this.grdNews.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdNews_CellFormatting);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 267);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(535, 34);
            this.panel1.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(464, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(66, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmNews
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 304);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNews";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "News";
            this.Load += new System.EventHandler(this.frmNews_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdNews)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView grdNews;
        private System.Windows.Forms.DataGridViewLinkColumn FB;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Message;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;



    }
}