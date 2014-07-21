using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FilesDepositoryApplication.Helpers
{
    public static class ErrorHelper
    {
        /// <summary>
        /// Just convert all errors from ModelState inside ontroller to String
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static String GetModelStateErrors(Controller controller, String separator ="; ")
        {
            if (!controller.ModelState.IsValid)
            {
                var modelStateErrors = controller.ModelState.Keys.SelectMany(key => controller.ModelState[key].Errors);
                return modelStateErrors.Aggregate("", (current, modelStateError) => current + (modelStateError.ErrorMessage + separator));
            }
            return null;
        }
        public static String GetModelStateFirstError(Controller controller)
        {
            if (!controller.ModelState.IsValid)
            {
                var modelStateErrors =
                    controller.ModelState.Keys.SelectMany(key => controller.ModelState[key].Errors).First();

                return modelStateErrors.ErrorMessage;

            }
            return null;
        }

    }
}