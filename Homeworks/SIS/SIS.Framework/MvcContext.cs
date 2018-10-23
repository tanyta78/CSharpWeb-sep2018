namespace SIS.Framework
{
    public class MvcContext
    {
        private static MvcContext Instance;

        private MvcContext() { }

        public static MvcContext Get => Instance ?? (Instance = new MvcContext());

        public string AssemblyName { get; set; }

        public string ControllersFolder { get; set; } = "Controllers";

        public string ControllersSuffix { get; set; } = "Controller";

        public string ViewsFolderName { get; set; } = "Views";

        public string ModelsFolder { get; set; } = "Models";

        public string ResourceFolder { get; set; } = "Resources";

        public string LayoutFileName { get; set; } = "_Layout";

        public string RootDirectoryRelativePath { get; set; } = "../../../";

        public string SharedViewsFolderName { get; set; } = "Shared";
    }
}
