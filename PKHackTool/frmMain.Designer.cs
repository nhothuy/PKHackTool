namespace PKTool
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.rtbRet = new System.Windows.Forms.RichTextBox();
            this.btnHTML = new System.Windows.Forms.Button();
            this.rtbHTML = new System.Windows.Forms.RichTextBox();
            this.lbFriends = new System.Windows.Forms.ListBox();
            this.btnSetVicTim = new System.Windows.Forms.Button();
            this.btnSetAttacker = new System.Windows.Forms.Button();
            this.btnAttack = new System.Windows.Forms.Button();
            this.lbAttackers = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnGet = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.chkFull = new System.Windows.Forms.CheckBox();
            this.chkAutoSteal = new System.Windows.Forms.CheckBox();
            this.chkAutoAttack = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtFBID = new System.Windows.Forms.TextBox();
            this.lbOther = new System.Windows.Forms.ListBox();
            this.nudNum = new System.Windows.Forms.NumericUpDown();
            this.btnRemove = new System.Windows.Forms.Button();
            this.rdoOther = new System.Windows.Forms.RadioButton();
            this.rdoByme = new System.Windows.Forms.RadioButton();
            this.rdoRandom = new System.Windows.Forms.RadioButton();
            this.btnKill = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtVictim = new System.Windows.Forms.TextBox();
            this.chkSet = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.avatarPre = new System.Windows.Forms.PictureBox();
            this.avatarNext = new System.Windows.Forms.PictureBox();
            this.avatar = new System.Windows.Forms.PictureBox();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.avatarPre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.avatarNext)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.avatar)).BeginInit();
            this.SuspendLayout();
            // 
            // rtbRet
            // 
            this.rtbRet.Location = new System.Drawing.Point(134, 323);
            this.rtbRet.Name = "rtbRet";
            this.rtbRet.ReadOnly = true;
            this.rtbRet.Size = new System.Drawing.Size(325, 123);
            this.rtbRet.TabIndex = 3;
            this.rtbRet.Text = "";
            // 
            // btnHTML
            // 
            this.btnHTML.Location = new System.Drawing.Point(678, 374);
            this.btnHTML.Name = "btnHTML";
            this.btnHTML.Size = new System.Drawing.Size(75, 23);
            this.btnHTML.TabIndex = 9;
            this.btnHTML.Text = "Start";
            this.btnHTML.UseVisualStyleBackColor = true;
            this.btnHTML.Click += new System.EventHandler(this.btnHTML_Click);
            // 
            // rtbHTML
            // 
            this.rtbHTML.Location = new System.Drawing.Point(471, 31);
            this.rtbHTML.Name = "rtbHTML";
            this.rtbHTML.Size = new System.Drawing.Size(282, 339);
            this.rtbHTML.TabIndex = 8;
            this.rtbHTML.Text = "";
            // 
            // lbFriends
            // 
            this.lbFriends.DisplayMember = "Name";
            this.lbFriends.FormattingEnabled = true;
            this.lbFriends.Location = new System.Drawing.Point(5, 31);
            this.lbFriends.Name = "lbFriends";
            this.lbFriends.Size = new System.Drawing.Size(116, 368);
            this.lbFriends.TabIndex = 0;
            this.lbFriends.ValueMember = "Id";
            // 
            // btnSetVicTim
            // 
            this.btnSetVicTim.Location = new System.Drawing.Point(5, 407);
            this.btnSetVicTim.Name = "btnSetVicTim";
            this.btnSetVicTim.Size = new System.Drawing.Size(48, 23);
            this.btnSetVicTim.TabIndex = 4;
            this.btnSetVicTim.Text = "Victim";
            this.btnSetVicTim.UseVisualStyleBackColor = true;
            this.btnSetVicTim.Click += new System.EventHandler(this.btnSetVicTim_Click);
            // 
            // btnSetAttacker
            // 
            this.btnSetAttacker.Enabled = false;
            this.btnSetAttacker.Location = new System.Drawing.Point(59, 407);
            this.btnSetAttacker.Name = "btnSetAttacker";
            this.btnSetAttacker.Size = new System.Drawing.Size(62, 23);
            this.btnSetAttacker.TabIndex = 5;
            this.btnSetAttacker.Text = "Attacker";
            this.btnSetAttacker.UseVisualStyleBackColor = true;
            this.btnSetAttacker.Click += new System.EventHandler(this.btnSetAttacker_Click);
            // 
            // btnAttack
            // 
            this.btnAttack.Location = new System.Drawing.Point(242, 151);
            this.btnAttack.Name = "btnAttack";
            this.btnAttack.Size = new System.Drawing.Size(75, 23);
            this.btnAttack.TabIndex = 4;
            this.btnAttack.Text = "Attack";
            this.btnAttack.UseVisualStyleBackColor = true;
            this.btnAttack.Click += new System.EventHandler(this.btnAttack_Click);
            // 
            // lbAttackers
            // 
            this.lbAttackers.DisplayMember = "Name";
            this.lbAttackers.FormattingEnabled = true;
            this.lbAttackers.Location = new System.Drawing.Point(168, 39);
            this.lbAttackers.Name = "lbAttackers";
            this.lbAttackers.Size = new System.Drawing.Size(149, 82);
            this.lbAttackers.TabIndex = 2;
            this.lbAttackers.ValueMember = "Id";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnGet);
            this.groupBox1.Controls.Add(this.btnPlay);
            this.groupBox1.Controls.Add(this.chkFull);
            this.groupBox1.Controls.Add(this.chkAutoSteal);
            this.groupBox1.Controls.Add(this.chkAutoAttack);
            this.groupBox1.Location = new System.Drawing.Point(132, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(325, 60);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Play";
            // 
            // btnGet
            // 
            this.btnGet.Location = new System.Drawing.Point(225, 19);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(40, 23);
            this.btnGet.TabIndex = 4;
            this.btnGet.Text = "Play";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(268, 19);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(51, 23);
            this.btnPlay.TabIndex = 3;
            this.btnPlay.Text = "Play all";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // chkFull
            // 
            this.chkFull.AutoSize = true;
            this.chkFull.Location = new System.Drawing.Point(152, 24);
            this.chkFull.Name = "chkFull";
            this.chkFull.Size = new System.Drawing.Size(77, 17);
            this.chkFull.TabIndex = 2;
            this.chkFull.Text = "Full shields";
            this.chkFull.UseVisualStyleBackColor = true;
            // 
            // chkAutoSteal
            // 
            this.chkAutoSteal.AutoSize = true;
            this.chkAutoSteal.Location = new System.Drawing.Point(83, 24);
            this.chkAutoSteal.Name = "chkAutoSteal";
            this.chkAutoSteal.Size = new System.Drawing.Size(73, 17);
            this.chkAutoSteal.TabIndex = 1;
            this.chkAutoSteal.Text = "Auto steal";
            this.chkAutoSteal.UseVisualStyleBackColor = true;
            // 
            // chkAutoAttack
            // 
            this.chkAutoAttack.AutoSize = true;
            this.chkAutoAttack.Location = new System.Drawing.Point(7, 24);
            this.chkAutoAttack.Name = "chkAutoAttack";
            this.chkAutoAttack.Size = new System.Drawing.Size(81, 17);
            this.chkAutoAttack.TabIndex = 0;
            this.chkAutoAttack.Text = "Auto attack";
            this.chkAutoAttack.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDelete);
            this.groupBox2.Controls.Add(this.btnAdd);
            this.groupBox2.Controls.Add(this.txtFBID);
            this.groupBox2.Controls.Add(this.lbOther);
            this.groupBox2.Controls.Add(this.nudNum);
            this.groupBox2.Controls.Add(this.btnRemove);
            this.groupBox2.Controls.Add(this.rdoOther);
            this.groupBox2.Controls.Add(this.rdoByme);
            this.groupBox2.Controls.Add(this.rdoRandom);
            this.groupBox2.Controls.Add(this.lbAttackers);
            this.groupBox2.Controls.Add(this.btnAttack);
            this.groupBox2.Location = new System.Drawing.Point(134, 119);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(325, 185);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Attack";
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(94, 123);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(63, 23);
            this.btnDelete.TabIndex = 30;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(116, 148);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(41, 23);
            this.btnAdd.TabIndex = 29;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtFBID
            // 
            this.txtFBID.Location = new System.Drawing.Point(8, 150);
            this.txtFBID.Name = "txtFBID";
            this.txtFBID.Size = new System.Drawing.Size(104, 20);
            this.txtFBID.TabIndex = 28;
            // 
            // lbOther
            // 
            this.lbOther.DisplayMember = "Name";
            this.lbOther.FormattingEnabled = true;
            this.lbOther.Location = new System.Drawing.Point(8, 39);
            this.lbOther.Name = "lbOther";
            this.lbOther.Size = new System.Drawing.Size(149, 82);
            this.lbOther.TabIndex = 7;
            this.lbOther.ValueMember = "Id";
            // 
            // nudNum
            // 
            this.nudNum.Location = new System.Drawing.Point(249, 15);
            this.nudNum.Maximum = new decimal(new int[] {
            13,
            0,
            0,
            0});
            this.nudNum.Name = "nudNum";
            this.nudNum.Size = new System.Drawing.Size(68, 20);
            this.nudNum.TabIndex = 6;
            this.nudNum.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(254, 125);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(63, 23);
            this.btnRemove.TabIndex = 5;
            this.btnRemove.Text = "Delete";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // rdoOther
            // 
            this.rdoOther.AutoSize = true;
            this.rdoOther.Location = new System.Drawing.Point(162, 18);
            this.rdoOther.Name = "rdoOther";
            this.rdoOther.Size = new System.Drawing.Size(51, 17);
            this.rdoOther.TabIndex = 1;
            this.rdoOther.Text = "Other";
            this.rdoOther.UseVisualStyleBackColor = true;
            this.rdoOther.CheckedChanged += new System.EventHandler(this.rdoOther_CheckedChanged);
            // 
            // rdoByme
            // 
            this.rdoByme.AutoSize = true;
            this.rdoByme.Location = new System.Drawing.Point(88, 18);
            this.rdoByme.Name = "rdoByme";
            this.rdoByme.Size = new System.Drawing.Size(68, 17);
            this.rdoByme.TabIndex = 3;
            this.rdoByme.Text = "By mysell";
            this.rdoByme.UseVisualStyleBackColor = true;
            // 
            // rdoRandom
            // 
            this.rdoRandom.AutoSize = true;
            this.rdoRandom.Checked = true;
            this.rdoRandom.Location = new System.Drawing.Point(25, 18);
            this.rdoRandom.Name = "rdoRandom";
            this.rdoRandom.Size = new System.Drawing.Size(65, 17);
            this.rdoRandom.TabIndex = 0;
            this.rdoRandom.TabStop = true;
            this.rdoRandom.Text = "Random";
            this.rdoRandom.UseVisualStyleBackColor = true;
            // 
            // btnKill
            // 
            this.btnKill.Location = new System.Drawing.Point(200, 90);
            this.btnKill.Name = "btnKill";
            this.btnKill.Size = new System.Drawing.Size(62, 23);
            this.btnKill.TabIndex = 1;
            this.btnKill.Text = "Kill spin";
            this.btnKill.UseVisualStyleBackColor = true;
            this.btnKill.Click += new System.EventHandler(this.btnKill_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(471, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Html friends";
            // 
            // txtVictim
            // 
            this.txtVictim.Enabled = false;
            this.txtVictim.Location = new System.Drawing.Point(5, 452);
            this.txtVictim.Name = "txtVictim";
            this.txtVictim.Size = new System.Drawing.Size(116, 20);
            this.txtVictim.TabIndex = 7;
            this.txtVictim.TextChanged += new System.EventHandler(this.txtVictim_TextChanged);
            // 
            // chkSet
            // 
            this.chkSet.AutoSize = true;
            this.chkSet.Location = new System.Drawing.Point(5, 434);
            this.chkSet.Name = "chkSet";
            this.chkSet.Size = new System.Drawing.Size(61, 17);
            this.chkSet.TabIndex = 6;
            this.chkSet.Text = "Manual";
            this.chkSet.UseVisualStyleBackColor = true;
            this.chkSet.CheckedChanged += new System.EventHandler(this.chkSet_CheckedChanged);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(400, 449);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(59, 23);
            this.btnClear.TabIndex = 23;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(132, 90);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(62, 23);
            this.btnView.TabIndex = 24;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Name:";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(50, 8);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(111, 13);
            this.lblName.TabIndex = 26;
            this.lblName.Text = "Please login to next ...";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(136, 307);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Logs";
            // 
            // avatarPre
            // 
            this.avatarPre.Location = new System.Drawing.Point(515, 404);
            this.avatarPre.Name = "avatarPre";
            this.avatarPre.Size = new System.Drawing.Size(64, 64);
            this.avatarPre.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.avatarPre.TabIndex = 30;
            this.avatarPre.TabStop = false;
            this.avatarPre.Visible = false;
            // 
            // avatarNext
            // 
            this.avatarNext.Location = new System.Drawing.Point(668, 404);
            this.avatarNext.Name = "avatarNext";
            this.avatarNext.Size = new System.Drawing.Size(64, 64);
            this.avatarNext.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.avatarNext.TabIndex = 29;
            this.avatarNext.TabStop = false;
            this.avatarNext.Visible = false;
            // 
            // avatar
            // 
            this.avatar.Location = new System.Drawing.Point(592, 404);
            this.avatar.Name = "avatar";
            this.avatar.Size = new System.Drawing.Size(64, 64);
            this.avatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.avatar.TabIndex = 28;
            this.avatar.TabStop = false;
            this.avatar.Visible = false;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "1.png");
            this.imageList.Images.SetKeyName(1, "2.png");
            this.imageList.Images.SetKeyName(2, "3.png");
            this.imageList.Images.SetKeyName(3, "4.png");
            this.imageList.Images.SetKeyName(4, "5.png");
            this.imageList.Images.SetKeyName(5, "6.png");
            this.imageList.Images.SetKeyName(6, "7.png");
            this.imageList.Images.SetKeyName(7, "8.png");
            this.imageList.Images.SetKeyName(8, "9.png");
            this.imageList.Images.SetKeyName(9, "10.png");
            this.imageList.Images.SetKeyName(10, "11.png");
            this.imageList.Images.SetKeyName(11, "12.png");
            this.imageList.Images.SetKeyName(12, "13.png");
            this.imageList.Images.SetKeyName(13, "14.png");
            this.imageList.Images.SetKeyName(14, "16.png");
            this.imageList.Images.SetKeyName(15, "17.png");
            this.imageList.Images.SetKeyName(16, "19.png");
            this.imageList.Images.SetKeyName(17, "20.png");
            this.imageList.Images.SetKeyName(18, "21.png");
            this.imageList.Images.SetKeyName(19, "22.png");
            this.imageList.Images.SetKeyName(20, "23.png");
            this.imageList.Images.SetKeyName(21, "15.png");
            this.imageList.Images.SetKeyName(22, "18.png");
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 477);
            this.Controls.Add(this.avatarPre);
            this.Controls.Add(this.avatarNext);
            this.Controls.Add(this.avatar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.chkSet);
            this.Controls.Add(this.txtVictim);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnKill);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSetAttacker);
            this.Controls.Add(this.btnSetVicTim);
            this.Controls.Add(this.lbFriends);
            this.Controls.Add(this.rtbHTML);
            this.Controls.Add(this.btnHTML);
            this.Controls.Add(this.rtbRet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pirate King Tool - nhothuy48cb@gmail.com";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.avatarPre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.avatarNext)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.avatar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbRet;
        private System.Windows.Forms.Button btnHTML;
        private System.Windows.Forms.RichTextBox rtbHTML;
        private System.Windows.Forms.ListBox lbFriends;
        private System.Windows.Forms.Button btnSetVicTim;
        private System.Windows.Forms.Button btnSetAttacker;
        private System.Windows.Forms.Button btnAttack;
        private System.Windows.Forms.ListBox lbAttackers;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.CheckBox chkFull;
        private System.Windows.Forms.CheckBox chkAutoSteal;
        private System.Windows.Forms.CheckBox chkAutoAttack;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnKill;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdoRandom;
        private System.Windows.Forms.RadioButton rdoOther;
        private System.Windows.Forms.RadioButton rdoByme;
        private System.Windows.Forms.TextBox txtVictim;
        private System.Windows.Forms.CheckBox chkSet;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.NumericUpDown nudNum;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lbOther;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtFBID;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.PictureBox avatarPre;
        private System.Windows.Forms.PictureBox avatarNext;
        private System.Windows.Forms.PictureBox avatar;
        private System.Windows.Forms.ImageList imageList;
        
    }
}

