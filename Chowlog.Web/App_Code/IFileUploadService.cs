using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Chowlog.Web.App_Code
{
    public interface IFileUploadService
    {
        string UploadFile(HttpPostedFileBase file);
    }
}
