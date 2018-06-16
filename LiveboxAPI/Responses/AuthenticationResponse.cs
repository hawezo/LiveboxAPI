using Livebox.Core;

namespace Livebox.Responses
{

    /// <summary>
    /// Response to an authentication request.
    /// </summary>
    public class AuthenticationResponse : BaseResponse
    {
        internal AuthenticationResponse(BaseResponse response) : base(response) { }

        /// <summary>
        /// Context ID for the session
        /// </summary>
        public string ContextID { get; internal set; }

    }

}
