using Amazon.S3.IO;
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
    public partial class fileInfoForm : Form
    {
        public fileInfoForm()
        {
            InitializeComponent();
        }

        public fileInfoForm(S3FileInfo fileFinfo, long fileSize)
        {
            InitializeComponent();
            String infoText = "이름 : " + fileFinfo.Name + "\r\n";
            infoText += "경로 : " + fileFinfo.FullName + "\r\n";
            infoText += "확장자 : " + fileFinfo.Extension + "\r\n";
            infoText += "수정일 : " + fileFinfo.LastWriteTime + "\r\n";
            infoText += "파일 크기 : " + fileSize + "byte\r\n";
            label2.Text = infoText;
        }

        public fileInfoForm(S3DirectoryInfo dirInfo)
        {
            InitializeComponent();
            String infoText = "이름 : " + dirInfo.Name + "\r\n";
            infoText += "경로 : " + dirInfo.FullName + "\r\n";
            infoText += "수정일 : " + dirInfo.LastWriteTime + "\r\n";
            label2.Text = infoText;
        }
    }
}
