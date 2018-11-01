namespace AirportWebApp
{
    using System;
    using SIS.MvcFramework;

    public class Program
    {
        static void Main(string[] args)
        {

            var dateAndTime = DateTime.Now;
            var date = dateAndTime.Date;

            Console.WriteLine(date.ToString("MMMM dd"));
            Console.WriteLine(dateAndTime.ToString("HH:mm"));

            WebHost.Start(new Startup());
        }
    }
}
