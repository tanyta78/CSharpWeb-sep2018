namespace SIS.HTTP.Exceptions
{
    using System;
    using Enums;

    public class InternalServerErrorException : Exception
    {
        private const string ExceptionMessage = "The Server has encountered an error.";

        public const HttpResponseStatusCode StatusCode = HttpResponseStatusCode.InternalServerError;

        public InternalServerErrorException():base(ExceptionMessage){}

        public InternalServerErrorException(string message):base(message){}

    }
}
