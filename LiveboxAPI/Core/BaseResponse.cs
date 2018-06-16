using System.Net;

namespace Livebox.Core
{

    public partial class BaseResponse
    {

        /// <summary>
        /// Status code of the response.
        /// </summary>
        public HttpStatusCode Code { get; internal set; }

        /// <summary>
        /// Defines weither or note the request is a success.
        /// </summary>
        public bool IsSuccess { get; internal set; }

        /// <summary>
        /// Response's raw message contents.
        /// </summary>
        public string RawResponse { get; internal set; }

        /// <summary>
        /// Contains an error if the request failed.
        /// </summary>
        public Error Error { get; internal set; }

        internal BaseResponse() { }
        internal BaseResponse(BaseResponse response) : this()
        {
            this.Code = response.Code;
            this.IsSuccess = response.IsSuccess;
            this.RawResponse = response.RawResponse;
            this.Error = response.Error;
        }
    }

}
