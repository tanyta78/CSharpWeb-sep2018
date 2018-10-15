namespace SIS.Framework.Utilities
{
    using System;

    public static class ControllerUtilities
    {
        public static string GetControllerName(object controller)
        {
            return controller.GetType()
                .Name
                .Replace(MvcContext.Get.ControllersSuffix, string.Empty);
        }

        public static string GetViewFullQualifiedName(string controller, string action) =>
            string.Format("{0}\\{1}\\{2}", MvcContext.Get.ViewsFolder, controller, action);
    }
}
