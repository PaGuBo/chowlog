using ExifLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Chowlog.Web.App_Code
{
    public static class Utils
    {
        public static DateTime GetDateTaken(Stream stream)
        {
            var dateTaken = DateTime.Now;
            try
            {
                var exifReader = new ExifReader(stream);
                exifReader.GetTagValue<DateTime>(ExifTags.DateTimeOriginal, out dateTaken);
            }

            //dirty fix for now
            catch (ExifLibException ex)
            {
                //log the error here
            }
            if (dateTaken == DateTime.Parse("01/01/0001"))
            {
                dateTaken = DateTime.Now;
            }
            return dateTaken;
        }

        public static DateTime GetDateTaken(byte[] buffer)
        {
            return (GetDateTaken(new MemoryStream(buffer)));
        }
    }
}