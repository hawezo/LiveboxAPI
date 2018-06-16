using Livebox.Core;
using Livebox.Errors;
using Livebox.Responses;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Livebox.API
{

    /// <summary>
    /// Allows to authenticate with the Livebox.
    /// </summary>
    public class Authentication : Endpoint<AuthenticationResponse>
    {

        /// <summary>
        /// Instanciate a new <see cref="Authentication"/> object.
        /// Note that if an authentication is successful, its context will automatically be saved and you won't have to worry about it.
        /// </summary>
        /// <param name="credentials"><see cref="Credentials"/> object which contains an username and a password.</param>
        public Authentication(Credentials credentials)
        {
            this.Path = $"authenticate?username={credentials.Username}&password={credentials.Password}";
        }

        /// <summary>
        /// Performs the authentication request.
        /// </summary>
        /// <returns>Returns a <see cref="AuthenticationResponse"/> object which contains the context identifier for this session.</returns>
        public override async Task<AuthenticationResponse> PerformRequestAsync()
        {
            this.Response = await Requester.PostAsync(this);

            if (!this.Response.IsSuccess)
            {
                try
                {
                    AuthenticationResponseError error = new AuthenticationResponseError(JObject.Parse(this.Response.RawResponse));
                    return new AuthenticationResponse(this.Response) { Error = error };
                }
                catch (Exception)
                {
                    return new AuthenticationResponse(Error.GetError(this.Response));
                }
            }

            else
            {
                Settings.Context = (string)JObject.Parse(this.Response.RawResponse)["data"]["contextID"];
                return new AuthenticationResponse(this.Response)
                {
                    ContextID = Settings.Context
                };
            }
        }

    }
}
