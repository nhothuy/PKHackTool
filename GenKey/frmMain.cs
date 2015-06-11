using MyUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GenKey
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            if (txtInput.Text.Trim() == String.Empty) return;
            txtOutput.Text = String.Empty;
            String Key = MySecurity.TripleDES_En(txtInput.Text.Trim(), "LEnHOtHuY");
            txtOutput.Text = Key;
        }
    }
}
