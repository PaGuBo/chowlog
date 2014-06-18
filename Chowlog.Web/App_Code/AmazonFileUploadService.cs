using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Chowlog.Web.App_Code
{
    public class AmazonFileUploadService : IFileUploadService
    {
        public string UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    using (var client = Amazon.AWSClientFactory.CreateAmazonS3Client(ConfigurationManager.AppSettings["AWSAccessKey"],
                                                                                          ConfigurationManager.AppSettings["AWSSecretKey"]))
                    {
                        PutObjectRequest request = new PutObjectRequest();

                        request.BucketName = ConfigurationManager.AppSettings["bucketname"];
                        request.CannedACL = S3CannedACL.PublicRead;
                        request.Key = fileName;
                        request.InputStream = file.InputStream;

                        var response = client.PutObject(request);
                    }
                    return fileName;
                }
            }
            catch (Exception e)
            {
                //TempData["Result"] = "Error!" + e.Message;
                throw new Exception("File Upload Failed!");
            }
            return "";
        }
    }
}