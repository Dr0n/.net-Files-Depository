using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using FilesDepositoryApplication.Helpers;

namespace FilesDepositoryApplication.Models.Info
{
    /// <summary>
    /// Uses for binding, and rceiving file data from client POST request
    /// </summary>
    public class UploadedFile
    {
        /// <summary>
        /// PAth where file will be stored
        /// </summary>
        public static String FilesPath = "~/uploaded-files";
        [Required(ErrorMessage = "File is missing.")]
        public HttpPostedFileBase File { get; set; }
        [Required(ErrorMessage = "File name is missing.")]
        public String FileName { get; set; }

        [Required(ErrorMessage = "File without extention.")]
        public String FileExtention { get; set; }
        [Required(ErrorMessage = "Missing created date.")]
        public DateTime Created { get; set; }

        public String GeneratedName { get; set; }
        public String SaveFile()
        {
            GeneratedName = GenerateHash(FileName, Created) + "." +FileExtention;
            var filePath = Path.Combine(FilesPath, GeneratedName);
            var path = System.Web.HttpContext.Current.Server.MapPath(filePath);
            File.SaveAs(path);
            return filePath;
        }
        public static string GenerateHash(string inputString, DateTime dateParam)
        {
            return Security.GetHashString(inputString + dateParam.ToString(CultureInfo.InvariantCulture));
        }

    }
}