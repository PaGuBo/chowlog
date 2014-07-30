using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Chowlog.Web.App_Code
{
    public class LocalFileUploadService : IFileUploadService
    {
        private string uploadPath = ConfigurationManager.AppSettings["LocalUploadPath"];

        public string UploadFile(HttpPostedFileBase file, string fileName)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    var path = Path.Combine(uploadPath, fileName);
                    file.SaveAs(path);
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


        public string UploadFile(byte[] file, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}