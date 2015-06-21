using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace PKTool
{
    public partial class frmLogin : Form
    {
        private const String URLLOGIN = "http://prod.cashkinggame.com/CKService.svc/v3.0/login/?{0}";
        private String req = String.Empty;
        public String Req
        {
            get { return req; }
        }
        private String resp = String.Empty;
        public String Resp
        {
            get { return resp; }
            set { resp = value; }
        }

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (rtbLogin.Text == String.Empty) return;
            String urlLogin = String.Format(URLLOGIN, DateTime.Now.ToOADate().ToString());
            String retLogin = doPost(urlLogin, rtbLogin.Text.Trim());
            JToken jTokenResp = JObject.Parse(retLogin);
            if (Convert.ToInt32(jTokenResp["ErrorCode"]) == 100)
            {
                req = rtbLogin.Text.Trim();
                resp = retLogin;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Failled login. Please try again.", "PKTool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                rtbLogin.Focus();
            }
        }

        public string doPost(string uri, string parameters)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                System.Net.ServicePointManager.Expect100Continue = false;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] bytes = Encoding.UTF8.GetBytes(parameters);
                request.ContentLength = bytes.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);

                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                var result = reader.ReadToEnd();
                stream.Dispose();
                reader.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
