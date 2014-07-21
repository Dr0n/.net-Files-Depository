using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FilesDepositoryApplication.Models.Info;
using FilesDepositoryApplication.Models.SqlRepository;
using NLog;

namespace FilesDepositoryApplication.Binders
{
    public class FileUploadDataBinder : DefaultModelBinder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="bindingContext"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, System.Type modelType)
        {
            if (modelType == typeof(UploadedFile))
            {
                var request = controllerContext.HttpContext.Request;
                //just first file is correct file, but possible to extend it for using with Files
                
                if (request.Files.Count == 0)
                {
                    return new UploadedFile();
                }
                var fileToUpload = request.Files.Get(0);
                var fileExt = Path.GetExtension(fileToUpload.FileName);
                if (!string.IsNullOrEmpty(fileExt))
                {
                    fileExt = fileExt.Replace(".", "");
                }
                return new UploadedFile
                {
                    FileExtention = fileExt,
                    FileName = fileToUpload.FileName,
                    Created = DateTime.Now
                };
            }

            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}