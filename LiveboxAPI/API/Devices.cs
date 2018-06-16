using Livebox.Core;
using Livebox.Errors;
using Livebox.Objects;
using Livebox.Responses;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Livebox.API
{
    public class Devices : Endpoint<DevicesResponse>
    {

        /// <summary>
        /// Instanciate a new <see cref="Devices"/> object.
        /// </summary>
        public Devices(string contextID = null)
        {
            this.Path = $"sysbus/Hosts:getDevices";
            this.Headers = new Dictionary<string, string>()
            {
                { "X-Context", contextID ?? Settings.Context }
            };
        }

        /// <summary>
        /// Performs the authentication request.
        /// </summary>
        /// <returns>Returns a <see cref="AuthenticationResponse"/> object which contains the context identifier for this session.</returns>
        public override async Task<DevicesResponse> PerformRequestAsync()
        {
            this.Response = await Requester.PostAsync(this);

            if (!this.Response.IsSuccess)
            {
                try
                {
                    DevicesResponseError error = new DevicesResponseError(JObject.Parse(this.Response.RawResponse));
                    return new DevicesResponse(this.Response) { Error = error };
                }
                catch (Exception)
                {
                    return new DevicesResponse(Error.GetError(this.Response));
                }
            }

            else
            {
                List<Device> deviceList = new List<Device>();
                foreach (JObject device in (JArray)JObject.Parse(this.Response.RawResponse)["status"])
                    deviceList.Add(Device.FromJson(device.ToString()));

                return new DevicesResponse(this.Response)
                {
                    Devices = deviceList
                };
            }
        }
    }
}
