using Livebox.Core;
using Newtonsoft.Json.Linq;

namespace Livebox.Errors
{
    /// <summary>
    /// Represents an error occured while using authentication API
    /// </summary>
    public class AuthenticationResponseError : Error
    {
        internal AuthenticationResponseError(JObject json)
        {
            this.Cause = json.ToString(); // TODO
        }

        /// <summary>
        /// Cause of this error (optional)
        /// </summary>
        public string Cause { get; private set; }
    }
}
