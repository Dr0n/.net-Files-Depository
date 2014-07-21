using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FilesDepositoryApplication.Models;
using FilesDepositoryApplication.Models.SqlRepository;
using NLog;

namespace FilesDepositoryApplication.Controllers
{   
    /// <summary>
    /// Just base abstract, class where stored all
    /// general methods and properties for all controllers
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Repository which will working with files insode some store
        /// Init from constructor DI MEF
        /// </summary>
        protected IRepository Repository;
        /// <summary>
        /// Map request for upload files to the File model
        /// Init from constructor DI MEF
        /// </summary>
        protected IMapper ModelMapper;

        [ImportingConstructor]
        public BaseController(IMapper modMapper, IRepository repository)
        {
            ModelMapper = modMapper;
            Repository = repository;
        }

    }
}
