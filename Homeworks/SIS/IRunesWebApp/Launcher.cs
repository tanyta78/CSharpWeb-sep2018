namespace IRunesWebApp
{
    using SIS.Framework;

    public class Launcher
    {
        static void Main(string[] args)
        {
            WebHost.Start(new StartUp());
        }

    }
}
