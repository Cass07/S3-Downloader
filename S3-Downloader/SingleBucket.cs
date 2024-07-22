using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Amazon;
using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Transfer;

namespace S3_Downloader
{
    public class SingleBucket
    {
        private String bucketName;
        private TreeNode bucketNode;
        private AmazonS3Client s3Client;
        private RegionEndpoint region = RegionEndpoint.APNortheast2;

        public SingleBucket(String bucketName, AmazonS3Client s3Client)
        {
            this.bucketName = bucketName;
            this.s3Client = s3Client;

            bucketNode = new TreeNode(bucketName, 0, 0);
        }

        private S3FileInfo[] bucketDirFilesInfo(String bucketDir)
        {
            try
            {
                S3DirectoryInfo dir = new S3DirectoryInfo(this.s3Client, this.bucketName, bucketDir);
                return dir.GetFiles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return null;
            }
        }

        public TreeNode getBucketNode()
        {
            return bucketNode;
        }
    }
}
