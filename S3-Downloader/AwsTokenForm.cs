using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace S3_Downloader
{
    public partial class AwsTokenForm : Form
    {
        String accessKey;
        String accessSecret;
        Form1 mainForm;

        public AwsTokenForm(Form1 form)
        {
            InitializeComponent();
            accessKey = S3_Downloader.Properties.Settings.Default.AccessKey;
            accessSecret = S3_Downloader.Properties.Settings.Default.AccessSecret;
            textBoxAccessKey.Text = accessKey;
            textBoxAccessSecret.Text = accessSecret;
            mainForm = form;
        }

        private void save_click(object sender, EventArgs e)
        {
            S3_Downloader.Properties.Settings.Default.AccessKey = textBoxAccessKey.Text;
            S3_Downloader.Properties.Settings.Default.AccessSecret = textBoxAccessSecret.Text;
            S3_Downloader.Properties.Settings.Default.Save();

            mainForm.Form1_reload();

            this.Close();
        }

        private void cancel_click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
