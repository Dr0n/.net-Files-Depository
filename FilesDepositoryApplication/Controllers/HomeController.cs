using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FilesDepositoryApplication.Binders;
using FilesDepositoryApplication.Helpers;
using FilesDepositoryApplication.Models;
using FilesDepositoryApplication.Models.Info;
using NLog;
using File = FilesDepositoryApplication.Models.SqlRepository.File;

namespace FilesDepositoryApplication.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HomeController : BaseController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [ImportingConstructor]
        public HomeController(IMapper modMapper, IRepository repository)
            : base(modMapper, repository)
        {
        }

        [HttpGet]
        public ActionResult Index(String s)
        {
            logger.Debug("Search param:" + s);
            ViewBag.SearchFilter = s;
            if (String.IsNullOrWhiteSpace(s))
            {
                return View(Repository.Files.OrderByDescending(k => k.Id).ToList());
            }
            return View(Repository.Files.Where(k => k.original_name.Contains(s)).OrderByDescending(k => k.Id).ToList());
        }

        public JsonResult Upload(UploadedFile fileToUpload)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, error = ErrorHelper.GetModelStateErrors(this) });

            }
            fileToUpload.SaveFile();
            var file = (File)ModelMapper.Map(fileToUpload, typeof(UploadedFile), typeof(File));
            if (!Repository.CreateFile(file))
            {
                return Json(new { success = false, error = "Problem with connection to DB. Please try to save file later." });
            }
            return Json(new {success = true});

        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            File file = Repository.Files.FirstOrDefault(k => k.Id == id);
            if (file == null)
            {
                return Json(new { success = false, error = "File is not found on server." });
            }
            var filePath = Path.Combine(Server.MapPath(UploadedFile.FilesPath), file.name);
            bool isDeleted = Repository.RemoveFile(file.Id);
            if (isDeleted && System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            else if(!isDeleted)
            {
                return Json(new { success = false, error = "Is not possible to delete file from DB." });
            }
            return Json(new {success = true});
        }
        [HttpGet]
        public ActionResult Download(int id)
        {
            File file = Repository.Files.FirstOrDefault(k => k.Id == id);
            if (file == null)
            {
                return HttpNotFound(); ;
            }
            var filePath = Path.Combine(Server.MapPath(UploadedFile.FilesPath), file.name);
            if (System.IO.File.Exists(filePath))
            {
                return File(filePath, "application/octet-stream", file.original_name);
            }
            return HttpNotFound();
        }

    }
}
