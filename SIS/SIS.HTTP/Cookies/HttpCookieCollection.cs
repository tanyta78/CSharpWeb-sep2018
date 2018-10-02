namespace SIS.HTTP.Cookies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;

    public class HttpCookieCollection:IHttpCookieCollection
    {
        private readonly IDictionary<string, HttpCookie> cookies= new Dictionary<string, HttpCookie>();

        public void Add(HttpCookie cookie)
        {
            if (cookie==null )
            {
                throw new ArgumentNullException();
            }

            if (!this.ContainsCookie(cookie.Key))
            {
                this.cookies.Add(cookie.Key,cookie);
            }
            else
            {
                this.cookies[cookie.Key] = cookie;
            }
        }

        public bool ContainsCookie(string key)
        {
            return this.cookies.ContainsKey(key);
        }

        public HttpCookie GetCookie(string key)
        {
            if (!this.ContainsCookie(key))
            {
                return null;
            }

            return this.cookies[key];
        }

        public bool HasCookies()
        {

            return this.cookies.Any();
        }

        public override string ToString()
        {
            return string.Join("; ", this.cookies.Values);
        }
    }
}
