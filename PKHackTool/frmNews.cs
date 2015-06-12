using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PKTool
{
    public partial class frmNews : Form
    {
        List<NewsItem> LISTNEWS = new List<NewsItem>();
        public frmNews()
        {
            InitializeComponent();
        }
        public frmNews(List<NewsItem> listNews)
        {
            InitializeComponent();
            //
            LISTNEWS = listNews;
            grdNews.DataSource = LISTNEWS;
        }

        private void grdNews_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                DataGridViewRow row = grdNews.Rows[rowIndex];
                row.Cells["FB"].Value = LISTNEWS[rowIndex].ByWhom.FBID;
                row.Cells["Id"].Value = LISTNEWS[rowIndex].ByWhom.Name;
                row.Cells["Message"].Value = LISTNEWS[rowIndex].ToString();
            }
            catch
            {

            }
        }

        private void grdNews_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0) return;
            if (e.ColumnIndex == 0)
            {
                int rowIndex = e.RowIndex;
                String urlFB = String.Empty;
                String id = LISTNEWS[rowIndex].ByWhom.FBID.ToString();
                if (id == "0") return;
                if (id.StartsWith("10000"))
                {
                    urlFB = String.Format("https://www.facebook.com/profile.php?id={0}", id);
                }
                else
                {
                    urlFB = String.Format("https://www.facebook.com/app_scoped_user_id/{0}", id);
                }
                openURL(urlFB);                
            }
        }


        /// <summary>
        /// Open url
        /// </summary>
        /// <param name="url"></param>
        private void openURL(String url)
        {
            try
            {
                if (url == String.Empty) return;
                ProcessStartInfo sInfo = new ProcessStartInfo(url);
                Process.Start(sInfo);
            }
            catch
            {

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmNews_Load(object sender, EventArgs e)
        {

        }
    }
}
