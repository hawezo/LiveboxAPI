using Livebox.Core;
using Newtonsoft.Json.Linq;

namespace Livebox.Errors
{
    /// <summary>
    /// Represents an error occured while using the device API
    /// </summary>
    public class DevicesResponseError : Error
    {
        internal DevicesResponseError(JObject json)
        {
            this.Cause = json.ToString(); // TODO
        }

        /// <summary>
        /// Cause of this error (optional)
        /// </summary>
        public string Cause { get; private set; }
    }
}
