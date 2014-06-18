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
        public string UploadFile(HttpPostedFileBase file)
        {
            string uploadPath = ConfigurationManager.AppSettings["LocalUploadPath"];
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
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
    }
}