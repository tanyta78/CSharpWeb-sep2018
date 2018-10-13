namespace SIS.MvcFramework.Logger
{
    using System;
    using System.IO;

    public class FileLogger : ILogger
    {
        private static readonly object LockObject = new object();

        private readonly string fileName;

        public FileLogger()
        {
            this.fileName = "file.txt";
        }

        //public FileLogger(string fileName)
        //{
        //    this.fileName = fileName;
        //}

        public void Log(string message)
        {
            lock (LockObject)
            {
                File.AppendAllText(this.fileName, message + Environment.NewLine);

            }
        }
    }
}