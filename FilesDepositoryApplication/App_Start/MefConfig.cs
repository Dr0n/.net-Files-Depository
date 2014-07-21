using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Web.Mvc;
using FilesDepositoryApplication.Mef;

namespace FilesDepositoryApplication
{
    public static class MefConfig
    {
        public static void RegisterMef()
        {
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var composition = new CompositionContainer(catalog);
            IControllerFactory mefControllerFactory = new MefControllerFactory(composition);
            ControllerBuilder.Current.SetControllerFactory(mefControllerFactory);
        }
    }
}