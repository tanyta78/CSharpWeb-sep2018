namespace SIS.HTTP.Cookies
{
    using Common;
    using Contracts;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class HttpCookieCollection : IHttpCookieCollection
    {

        private readonly Dictionary<string, HttpCookie> cookies = new Dictionary<string, HttpCookie>();

        public void Add(HttpCookie cookie)
        {
            CoreValidator.ThrowIfNull(cookie, nameof(cookie));
            this.cookies.Add(cookie.Key, cookie);

        }

        public bool ContainsCookie(string key)
        {
            CoreValidator.ThrowIfNull(key,nameof(key));
            return this.cookies.ContainsKey(key);
        }

        public HttpCookie GetCookie(string key)
        {
            CoreValidator.ThrowIfNull(key,nameof(key));
            return this.cookies.GetValueOrDefault(key, null);
        }

        public bool HasCookies()
        {

            return this.cookies.Any();
        }

        public IEnumerator<HttpCookie> GetEnumerator()
        {
            foreach (var cookie in this.cookies)
            {
                yield return cookie.Value;
            }
        }

        public override string ToString()
        {
            return string.Join(GlobalConstants.CookieSplitDelimiter, this.cookies.Values);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
