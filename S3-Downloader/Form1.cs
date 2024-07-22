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
using System.Xml.Linq;

namespace S3_Downloader
{
    public partial class Form1 : Form
    {
        public S3Manager S3Manager;
        public ImageList imgList;
        public TreeNode selectedNode;
        public Form1()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            ImageList imgList = new ImageList();
            imgList.Images.Add(Bitmap.FromFile(@"C:\Users\kjh95\source\repos\S3-Downloader\S3-Downloader\icon\folder.png"));
            treeView1.ImageList = imgList;

            if (!string.IsNullOrEmpty(S3_Downloader.Properties.Settings.Default.AccessKey))
            {
                S3Manager = new S3Manager();

                String[] bucketNames = S3Manager.getBucketNmaeList();
                foreach (String bucketName in bucketNames)
                {
                    TreeNode treeNode = new TreeNode(bucketName, 0, 0);
                    treeNode.Tag = bucketName;
                    treeNode.Name = bucketName + ":\\";
                    treeView1.Nodes.Add(treeNode);
                }
            }
        }

        public void Form1_reload()
        {
            S3Manager = new S3Manager();
            String[] bucketNames = S3Manager.getBucketNmaeList();
            treeView1.Nodes.Clear();
            foreach (String bucketName in bucketNames)
            {
                TreeNode treeNode = new TreeNode(bucketName, 0, 0);
                treeNode.Tag = bucketName;
                treeNode.Name = bucketName + ":\\";
                treeView1.Nodes.Add(treeNode);
            }
        }

        private void SecretKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AwsTokenForm awsTokenForm = new AwsTokenForm(this);
            awsTokenForm.ShowDialog();
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // 폴더라면 하위 파일을 조사해서 노드에 추가. 아무것도 없을 경우 없음 더미 노드 추가
            // 파일이라면 다운로드

            TreeNode node = e.Node;
            String nodeRoute = node.FullPath;

            int spliterIndex = node.Name.IndexOf(":\\");
            String bucketName = node.Name.Substring(0, spliterIndex);
            String bucketDir = node.Name.Substring(spliterIndex + 2);

            if (node.Name.Substring(node.Name.Length - 1) == "\\")
            {
                // 폴더임
                if (node.Nodes.Count == 0)
                {
                    S3Manager.updateBucketDirFilesInfo(bucketName, bucketDir, node);
                }
                e.Node.Expand();
            } else
            {
                // 파일임
                saveFileDialog1.FileName = node.Text;
                if(saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // 다운로드
                    String dlPath = saveFileDialog1.FileName;

                    S3Manager.fileDownload(bucketName, bucketDir, dlPath);
                }

            }

        }

        private void folderDlButton_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                String dlPath = folderBrowserDialog1.SelectedPath;
                TreeNode node = this.selectedNode;
                int spliterIndex = node.Name.IndexOf(":\\");

                String bucketName = node.Name.Substring(0, spliterIndex);
                String bucketDir = node.Name.Substring(spliterIndex + 2);

                S3Manager.folderDownload(bucketName, bucketDir, dlPath);
            }
        }

        private void infoButton_Click(object sender, EventArgs e)
        {
            TreeNode node = this.selectedNode;
            int spliterIndex = node.Name.IndexOf(":\\");
            String bucketName = node.Name.Substring(0, spliterIndex);
            String bucketDir = node.Name.Substring(spliterIndex + 2);
            if (node.Name.Substring(node.Name.Length - 1) == "\\")
            { 
                S3DirectoryInfo s3DirectoryInfo = S3Manager.getDirInfo(bucketName, bucketDir);
                fileInfoForm fileInfoForm = new fileInfoForm(s3DirectoryInfo);
                fileInfoForm.ShowDialog();
            }
            else
            {
                S3FileInfo fileInfo = S3Manager.getFileInfo(bucketName, bucketDir, selectedNode.Text);
                long fileSize = S3Manager.getFileSize(bucketName, bucketDir, selectedNode.Text);
                fileInfoForm fileInfoForm = new fileInfoForm(fileInfo, fileSize);
                fileInfoForm.ShowDialog();
            }

        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.selectedNode = e.Node;
        }
    }
}
