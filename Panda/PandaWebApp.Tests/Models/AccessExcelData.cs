namespace PandaWebApp.Tests.Models
{
    using System;
    using System.Configuration;
    using System.Data.OleDb;
    using System.IO;

    public class AccessExcelData
    {
        public static string TestDataFileConnection(string fileName)
        {
            var path = ConfigurationManager.AppSettings["TestDataSheetPath"];
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

            // var path = AppDomain.CurrentDomain.BaseDirectory + "ExcelFilesData\\";      
            var con = string.Format("Provider=Microsoft.ACE.OLEDB.12.0; Data Source = {0}; Extended Properties='Excel 12.0 Xml; HDR=YES; IMEX=1,';", path + fileName);
            return con;
        }

        //public static T GetTestData<T>(string fileName, string sheet, string keyName)
        //{
        //    //TODO FIND WAY TO GET TEST DATA
        //    //using (var connection = new
        //    //    OleDbConnection(TestDataFileConnection(fileName)))
        //    //{
        //    //    connection.Open();
        //    //    var query = string.Format("select * from [{0}$]where key = '{1}'", sheet, keyName);
        //    //    var value = connection.Query<T>(query).FirstOrDefault();
        //    //    connection.Close();
        //    //    return value;
        //    //}
        //}
    }
}
