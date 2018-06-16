using Livebox.Core;
using Livebox.Errors;
using Livebox.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Livebox.API
{
    /// <summary>
    /// Allows to perform a request to the WS API.
    /// </summary>
    public class WsRequest : Endpoint<WsResponse>
    {

        /// <summary>
        /// Instanciate a new <see cref="Authentication"/> object.
        /// Note that if an authentication is successful, its context will automatically be saved and you won't have to worry about it.
        /// </summary>
        /// <param name="credentials"><see cref="Credentials"/> object which contains an username and a password.</param>
        public WsRequest(string service, string method, Dictionary<string, string> parameters, string context = null)
        {
            this.Path = $"ws";
            this.Body = $@"
                {{
                    ""parameters"": {JsonConvert.SerializeObject(parameters)},
                    ""service"": ""{service}"",
                    ""method"": ""{method}""
                }}"
                .Replace(" ", null)
                .Replace("\n", null)
                .Replace("\r", null);
            this.Headers = new Dictionary<string, string>() { { "X-Context", context ?? Settings.Context } };
        }
        
        /// <summary>
        /// Looks for all possible WS path and methods in your Livebox.
        /// </summary>
        public async static Task<List<WsPath>> GetWsPaths()
        {
            List<WsPath> wsPaths = new List<WsPath>();
            string script = await Requester.GetPageAsync("scripts.js");

            foreach (Match match in new Regex(@"(sysbus[.].*)").Matches(script))
            {
                string entry = match.Value;

                string path, method = "";

                int i = entry.IndexOf(':');
                if (i >= 0)
                {
                    path = entry.Substring(0, i);
                    method = entry.Substring(i + 1, entry.Length - (i + 1));
                }
                else
                {
                    path = entry;
                    i = path.IndexOf('\"');
                    if (i >= 0) path = path.Substring(0, i);
                }

                i = method.IndexOf('\"');
                if (i >= 0) method = method.Substring(0, i);

                path = new Regex(@"""(.*)""").Replace(path, "<o>");
                path = path.Replace("/", ".");

                if (!wsPaths.Any(x => x.Path == path))
                    wsPaths.Add(new WsPath() { Path = path, Methods = new List<string>() { method } });
                else
                {
                    i = wsPaths.FindIndex(x => x.Path == path);
                    if (i >= 0 && !wsPaths[i].Methods.Contains(method))
                        wsPaths[i].Methods.Add(method);
                    wsPaths[i].Methods = wsPaths[i].Methods.OrderBy(t => t).ToList();
                }
            }
            return wsPaths.OrderBy(x => x.Path).ToList();
        }

        /// <summary>
        /// Performs the WS API request.
        /// </summary>
        /// <returns>Returns a <see cref="WsResponseError"/> object.</returns>
        public override async Task<WsResponse> PerformRequestAsync()
        {
            this.Response = await Requester.PostAsync(this);

            if (!this.Response.IsSuccess)
            {
                try
                {
                    WsResponseError wserror = new WsResponseError(
                        JObject.Parse(this.Response.RawResponse)["result"]["errors"][0]["error"].ToString(),
                        JObject.Parse(this.Response.RawResponse)["result"]["errors"][0]["description"].ToString(),
                        JObject.Parse(this.Response.RawResponse)["result"]["errors"][0]["info"].ToString());
                    return new WsResponse(this.Response) { Error = wserror };
                }
                catch (Exception)
                {
                    return new WsResponse(Error.GetError(this.Response));
                }
            }

            else
            {
                return new WsResponse(this.Response)
                {
                    Result = ((JObject)JObject.Parse(this.Response.RawResponse)["result"]).ToString()
                };
            }
        }

    }
}
