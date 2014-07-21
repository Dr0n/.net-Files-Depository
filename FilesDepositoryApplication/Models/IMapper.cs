using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesDepositoryApplication.Models
{
    [InheritedExport]
    public interface IMapper
    {
        object Map(object source, Type sourceType, Type destinationType);
    }
}
