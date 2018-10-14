namespace SIS.Framework.Attributes.Methods
{
    public class HttpGetAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string reqMethod)
        {
            return reqMethod.ToUpper()=="GET";
        }
    }
}
