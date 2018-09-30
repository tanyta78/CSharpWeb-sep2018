namespace SIS.HTTP.Exceptions
{
    using System;
    using Enums;

    public class BadRequestException : Exception
    {
        private const string ExceptionMessage = "The Request was malformed or contains unsupported elements.";

        public const HttpResponseStatusCode StatusCode = HttpResponseStatusCode.BadRequest;

        public BadRequestException():base(ExceptionMessage)
        {
            
        }

    }
}
