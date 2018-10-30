namespace IRunesWebApp
{
    using SIS.Framework;
    using TorshiaWebApp;

    public class Launcher
    {
        static void Main(string[] args)
        {
            WebHost.Start(new StartUp());
        }

    }
}
