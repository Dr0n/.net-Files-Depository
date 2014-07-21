using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilesDepositoryApplication.Models.SqlRepository
{
    public partial class SqlRepository
    {
        public IQueryable<File> Files 
        {
            get { return Db.Files; }
        }
         public bool CreateFile(File model)
        {
            Db.Files.InsertOnSubmit(model);
            Db.Files.Context.SubmitChanges();
            return true;
        }

        public bool UpdateFile(File model)
        {
            var existingModel = Db.Files.FirstOrDefault(file => file.Id == model.Id);
            if (existingModel == null || model.Id == 0)
            {
                return false;
            }
            existingModel.name = model.name;
            existingModel.original_name = model.original_name;
            existingModel.date = model.date;
            Db.Files.Context.SubmitChanges();
            return true;
        }

        public bool RemoveFile(int fileId)
        {
            var existingModel = Db.Files.FirstOrDefault(file => file.Id == fileId);
            if (fileId == 0 || existingModel == null)
            {
                return false;
            }
            Db.Files.DeleteOnSubmit(existingModel);
            Db.Files.Context.SubmitChanges();
            return true;
        }
    }
 }