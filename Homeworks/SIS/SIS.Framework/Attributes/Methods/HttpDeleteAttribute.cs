namespace SIS.Framework.Attributes.Methods
{
    public class HttpDeleteAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string reqMethod)
        {
            return reqMethod.ToUpper()=="DELETE";
        }
    }
}