namespace SIS.HTTP.Sessions
{
    using System.Collections.Generic;
    using SIS.HTTP.Sessions.Contracts;

    public class HttpSession : IHttpSession
    {
        private readonly IDictionary<string, object> parameters;

        public HttpSession(string id)
        {
            this.parameters = new Dictionary<string, object>();

            this.Id = id;
        }

        public string Id { get; }

        public void AddParameter(string name, object parameter)
        {
            this.parameters[name] = parameter;
        }

        public object GetParameter(string name)
        {
            if (!this.ContainsParameter(name))
            {
                return null;
            }

            return this.parameters[name];
        }

        public void ClearParameters() => this.parameters.Clear();

        public bool ContainsParameter(string name) => this.parameters.ContainsKey(name);
    }
}
