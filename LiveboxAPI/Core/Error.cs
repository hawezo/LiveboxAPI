using System;
using System.Net;

namespace Livebox.Core
{

    /// <summary>
    /// Default error class.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Code of the given error.
        /// </summary>
        public string ErrorCode { get; internal set; }

        /// <summary>
        /// Details of the error.
        /// </summary>
        public string ErrorMessage { get; internal set; }

        /// <summary>
        /// Exception if code-side error.
        /// </summary>
        public Exception Exception { get; internal set; }

        /// <summary>
        /// Gets an error thanks to a response.
        /// </summary>
        public static BaseResponse GetError(BaseResponse response)
        {
            // This has to be fill
            switch (response.Code)
            {
                case HttpStatusCode.NoContent:
                    {
                        return new BaseResponse(response)
                        {
                            IsSuccess = false,
                            Error = new Error()
                            {
                                ErrorMessage = "Object or parameter not found",
                                ErrorCode = "196618"
                            }
                        };
                    }

                default:
                    {
                        return new BaseResponse(response)
                        {
                            IsSuccess = false,
                            Error = new Error()
                            {
                                ErrorMessage = response.Code.ToString(),
                                ErrorCode = response.Code.ToString()
                            }
                        };
                    }
            }
        }

    }
}
