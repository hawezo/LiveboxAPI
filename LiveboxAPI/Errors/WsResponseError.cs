using Livebox.Core;
using Newtonsoft.Json.Linq;

namespace Livebox.Errors
{
    /// <summary>
    /// Represents an error occured while using authentication API
    /// </summary>
    public class WsResponseError : Error
    {
        internal WsResponseError(string error, string description, string info)
        {
            this.ErrorCode = error;
            this.ErrorMessage = description;
            this.Info = info;
        }

        /// <summary>
        /// Cause of this error (optional)
        /// </summary>
        public string Info { get; private set; }
    }
}
