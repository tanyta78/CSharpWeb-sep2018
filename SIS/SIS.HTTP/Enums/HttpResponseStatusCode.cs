namespace SIS.HTTP.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum HttpResponseStatusCode
    {
        Ok = 200,
        Created = 201,
        Redirect = 301,
        Found = 302,
        [Display(Name = "See other")]
        SeeOther = 303,
        [Display(Name = "Bad Request")]
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        [Display(Name = "Not Found")]
        NotFound = 404,
        [Display(Name = "Internal Server Error")]
        InternalServerError = 500
    }
}
