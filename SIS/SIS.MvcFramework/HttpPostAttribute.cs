namespace SIS.MvcFramework
{
    using HTTP.Enums;

    public class HttpPostAttribute : HttpAttribute
    {
        public HttpPostAttribute(string path = null) : base(path)
        {

        }

        public override HttpRequestMethod Method => HttpRequestMethod.Post;
    }
}
