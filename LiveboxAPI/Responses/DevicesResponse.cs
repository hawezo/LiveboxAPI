using Livebox.Core;
using Livebox.Objects;
using System.Collections.Generic;

namespace Livebox.Responses
{

    /// <summary>
    /// Response to an authentication request.
    /// </summary>
    public class DevicesResponse : BaseResponse
    {
        internal DevicesResponse(BaseResponse response) : base(response) { }

        /// <summary>
        /// Context ID for the session
        /// </summary>
        public List<Device> Devices { get; internal set; }

    }
    
}
