using System.Collections.Generic;
using System.Threading.Tasks;

namespace Livebox.Core
{

    /// <summary>
    /// Allows to create an unreferenced endpoint to the Livebox.
    /// </summary>
    /// <typeparam name="T">A <see cref="Response"/> inherited class which will contain the response to the request to that endpoint.</typeparam>
    public abstract class Endpoint<T>
    {
        /// <summary>
        /// A path for that endpoint. Will be concatenated to <see cref="Settings.LiveboxUrl"/>.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// A concatenation of <see cref="Settings.LiveboxUrl"/> and <see cref="Endpoint{T}.Path"/>.
        /// </summary>
        public string Address
        {
            get
            {
                return Settings.LiveboxUrl + this.Path;
            }
        }

        /// <summary>
        /// A <see cref="Response"/> object containing a basic response from the <see cref="Requester.PostAsync{T}(Endpoint{T})"/> method.
        /// </summary>
        public BaseResponse Response { get; set; }
        
        /// <summary>
        /// A string-based body which will be added to the request.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// A dictionary which contains custom headers for the request.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Performs the actual request.
        /// </summary>
        /// <returns>An instanciated <see cref="Response"/>-inherited object which contains details for that endpoint's request.</returns>
        public abstract Task<T> PerformRequestAsync();
    }

}
