using System;
using System.Windows.Forms;
using Amazon;
using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using S3_Downloader;

public class S3Manager
{
	private String accessKey;
	private String accessSecret;
    private RegionEndpoint region = RegionEndpoint.APNortheast2;
    private String[] bucketNameList;
	public S3Manager()
	{
        accessKey = S3_Downloader.Properties.Settings.Default.AccessKey;
        accessSecret = S3_Downloader.Properties.Settings.Default.AccessSecret;
        AmazonS3Client s3Client = new AmazonS3Client(accessKey, accessSecret, region);
        try { this.bucketNameList = this.getBucketList(); }
        catch (Exception ex)
        {
            MessageBox.Show("인증에 실패했습니다. AWS 키와 시크릿을 확인해 주세요.", "Failure!");
            this.bucketNameList = new String[0];
        }

    }

    public String[] getBucketNmaeList()
    {
        return bucketNameList;
    }

    public void folderDownload(String bucketName, string bucketPath, string dlFolderPath)
    {
        AmazonS3Client S3Client = new AmazonS3Client(accessKey, accessSecret, region);
        S3DirectoryInfo s3DirectoryInfo = new S3DirectoryInfo(S3Client, bucketName, bucketPath);
        S3FileInfo[] files = s3DirectoryInfo.GetFiles();
        var fileTransferUtility = new TransferUtility(accessKey, accessSecret, region);

        try
        {
            foreach (S3FileInfo file in files)
            {
                String dlFilePath = dlFolderPath + "\\" + file.Name;
                dlFilePath = dlFilePath.Replace("\\", "/");
                String fileBucketPath = bucketPath + file.Name;
                fileBucketPath = fileBucketPath.Replace("\\", "/");

                fileTransferUtility.DownloadAsync(dlFilePath, bucketName, fileBucketPath);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("폴더 다운로드 에러", "Failure!");
        }
        finally
        {
            MessageBox.Show("폴더 다운로드 완료", "Success!");
        }
    }

    public String[] getBucketList()
    {
        var s3Client = new AmazonS3Client(accessKey, accessSecret, region);
        var response = s3Client.ListBuckets();
        String[] bucketList = new String[response.Buckets.Count];
        for (int i = 0; i < response.Buckets.Count; i++)
        {
            bucketList[i] = response.Buckets[i].BucketName;
        }
        return bucketList;
    }

    public void updateBucketDirFilesInfo(String bucketName, String bucketPath, TreeNode node)
    {
        var s3Client = new AmazonS3Client(accessKey, accessSecret, region);
        var s3DirectoryInfo = new S3DirectoryInfo(s3Client, bucketName, bucketPath);
        var directories = s3DirectoryInfo.GetDirectories();
        
        foreach(var directory in directories)
        {
            TreeNode dirNode = new TreeNode(directory.Name, 0, 0);
            dirNode.Tag = bucketName;
            dirNode.Name = directory.FullName;
            node.Nodes.Add(dirNode);
        }

        var files = s3DirectoryInfo.GetFiles();
        foreach (var file in files)
        {
            TreeNode fileNode = new TreeNode(file.Name, 1, 1);
            fileNode.Tag = bucketName;
            fileNode.Name = file.FullName;
            node.Nodes.Add(fileNode);
        }

        int fileCount = files.Length + directories.Length;
        node.Text = node.Text + " (" + fileCount + ")";
    }

	private void fileUpload(String bucket, String bucketPath, String path)
	{
        var fileTransferUtility = new TransferUtility(accessKey, accessSecret, region);

        try
        {
            fileTransferUtility.UploadAsync(path, bucket, bucketPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show(path + " 업로드 에러", "Failure!");
        }
    }

    private S3FileInfo[] listFiles(String bucket, String bucketPath)
    {
        var s3Client = new AmazonS3Client(accessKey, accessSecret, region);
        var s3DirectoryInfo = new S3DirectoryInfo(s3Client, bucket, bucketPath);
        return s3DirectoryInfo.GetFiles();
    }

    public S3FileInfo getFileInfo(String bucket, String bucketPath, String filename)
    {
        var s3Client = new AmazonS3Client(accessKey, accessSecret, region);
        return new S3FileInfo(s3Client, bucket, bucketPath);
    }

    public S3DirectoryInfo getDirInfo(String bucket, String bucketPath)
    {
        var s3Client = new AmazonS3Client(accessKey, accessSecret, region);
        return new S3DirectoryInfo(s3Client, bucket, bucketPath);
    }

    public long getFileSize(String bucket, String bucketPath, String filename)
    {
        bucketPath = bucketPath.Replace("\\", "/");
        var s3Client = new AmazonS3Client(accessKey, accessSecret, region);
        var getObjectMetadataRequest = new GetObjectMetadataRequest() { BucketName = bucket, Key = bucketPath };
        var meta = s3Client.GetObjectMetadata(getObjectMetadataRequest);
        var fileSize = meta.Headers.ContentLength;
        return fileSize;
    }


    public void fileDownload(String bucket, String bucketPath, String path)
    {
        var fileTransferUtility = new TransferUtility(accessKey, accessSecret, region);

        bucketPath = bucketPath.Replace("\\", "/");
        path = path.Replace("\\", "/");

        try
        {
            fileTransferUtility.DownloadAsync(path, bucket, bucketPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show(path + " 다운로드 에러", "Failure!");
        }
        finally
        {
            MessageBox.Show("다운로드 완료", "Success!");
        }
    }


}
