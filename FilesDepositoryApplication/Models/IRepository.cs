using System.ComponentModel.Composition;
using System.Linq;
using FilesDepositoryApplication.Models.SqlRepository;

namespace FilesDepositoryApplication.Models
{
    [InheritedExport]
    public interface IRepository
    {
        IQueryable<File> Files { get; }

        bool CreateFile(File model);

        bool UpdateFile(File model);

        bool RemoveFile(int model);
    }
}
