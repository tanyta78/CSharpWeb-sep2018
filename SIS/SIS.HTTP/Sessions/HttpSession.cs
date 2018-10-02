namespace SIS.HTTP.Sessions
{
    using Contracts;
    using System;
    using System.Collections.Generic;

    public class HttpSession : IHttpSession
    {
        private readonly IDictionary<string, object> parameters;

        public string Id { get; }

        public HttpSession(string id)
        {
            this.Id = id;
            this.parameters = new Dictionary<string, object>();
        }

        public object GetParameter(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException();
            }

            if (!this.ContainsParameter(name))
            {
                return null;
            }

            return this.parameters[name];
        }

        public bool ContainsParameter(string name)
        {
            return this.parameters.ContainsKey(name);
        }

        public void AddParameter(string name, object parameter)
        {
            if (!this.ContainsParameter(name))
            {
                this.parameters.Add(name, parameter);
            }
            else
            {
                this.parameters[name] = parameter;
            }


        }

        public void ClearParameters()
        {
            this.parameters.Clear();
        }
    }
}
