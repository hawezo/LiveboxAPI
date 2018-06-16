using Livebox.Core;

namespace Livebox.Responses
{
    /// <summary>
    /// Response to an authentication request.
    /// </summary>
    public class WsResponse : BaseResponse
    {
        internal WsResponse(BaseResponse response) : base(response) { }

        /// <summary>
        /// Result of the API request.
        /// </summary>
        public string Result { get; internal set; }

    }
}
