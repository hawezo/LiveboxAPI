using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Livebox.Core
{

    internal class Requester
    {
        /// <summary>
        /// Defines timeout for http requests.
        /// </summary>
        public static TimeSpan Timeout = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Defines encoding for reading responses and writing requests.
        /// </summary>
        public static Encoding Encoding = Encoding.UTF8;

        /// <summary>
        /// Current name of this library.
        /// </summary>
        public readonly static string Name = "livebox-api";

        /// <summary>
        /// Current version of Mojangsharp.
        /// </summary>
        public readonly static string Version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        /// <summary>
        /// Represents the http client used in the web requests.
        /// </summary>
        internal static HttpClient Client
        {
            get
            {
                if (_client == null)
                    _client = new HttpClient() { Timeout = Requester.Timeout };
                return _client;
            }
            private set
            {
                _client = value;
            }
        }
        private static HttpClient _client;

        /// <summary>
        /// Performs a POST request to the given URL with the given headers, and returns a Response object.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<BaseResponse> PostAsync<T>(Endpoint<T> endpoint)
        {
            if (endpoint == null)
                throw new ArgumentNullException("Endpoint");

            HttpResponseMessage httpResponse = null;
            Error error = null;
            string rawResponse = null;

            try
            {
                // build the request message
                HttpRequestMessage request = new HttpRequestMessage(
                    HttpMethod.Post,
                    endpoint.Address);

                // sets its user agent
                request.Headers.UserAgent.Add(new ProductInfoHeaderValue(Requester.Name, Requester.Version));
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/javascript"));

                // sets its custom headers if any
                if (endpoint.Headers != null && endpoint.Headers.Count > 0)
                    foreach (KeyValuePair<string, string> header in endpoint.Headers)
                        request.Headers.Add(header.Key, header.Value);

                // sets its content
                request.Content = endpoint.Body == null ? null : new StringContent(
                    endpoint.Body,
                    Requester.Encoding,
                    "application/x-sah-ws-1-call+json");

                // sends it and get its response
                httpResponse = await Requester.Client.SendAsync(request);
                rawResponse = await httpResponse.Content.ReadAsStringAsync();
                httpResponse.EnsureSuccessStatusCode();

                // because livebox api sucks and returns a 200 if there is a ws error,
                // i need to do that
                if (rawResponse.Contains("196618")) // code of an error
                    error = new Error() { };
            }
            catch (Exception ex)
            {
                error = new Error()
                {
                    ErrorMessage = ex.Message,
                    ErrorCode = null,
                    Exception = ex
                };
            }

            return new BaseResponse()
            {
                Code = httpResponse.StatusCode,
                IsSuccess = httpResponse.IsSuccessStatusCode && (
                            httpResponse.StatusCode == HttpStatusCode.Accepted ||
                            httpResponse.StatusCode == HttpStatusCode.Continue ||
                            httpResponse.StatusCode == HttpStatusCode.Created ||
                            httpResponse.StatusCode == HttpStatusCode.Found ||
                            httpResponse.StatusCode == HttpStatusCode.OK ||
                            httpResponse.StatusCode == HttpStatusCode.PartialContent ||
                            httpResponse.StatusCode == HttpStatusCode.NoContent) &&
                            error == null,
                RawResponse = rawResponse,
                Error = error
            };
        }

        /// <summary>
        /// Performs a GET request on the given path (concatenated to Livebox URL) to download its source.
        /// </summary>
        public static async Task<string> GetPageAsync(string path)
        {
            try
            {
                HttpClientHandler handler = new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };
                using (HttpClient client = new HttpClient(handler))
                    using (HttpResponseMessage response = await client.GetAsync(Settings.LiveboxUrl + path))
                    using (HttpContent content = response.Content)
                    {
                        byte[] buffer = await content.ReadAsByteArrayAsync();
                        return (Encoding.UTF8.GetString(buffer));
                    }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
