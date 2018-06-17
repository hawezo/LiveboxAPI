using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Colorful;
using Livebox.API;
using Livebox.Core;
using Livebox.Errors;
using Livebox.Objects;
using Livebox.Responses;
using Console = Colorful.Console;

namespace Sample
{
    class Program
    {

        static void Main(string[] args) => new Program().DisplayUsers();

        private void DisplayUsers()
        {
            // authenticates
            AuthenticationResponse authentication =
                new Authentication(new Credentials("admin", "password"))
                .PerformRequestAsync()
                .Result;

            if (authentication.IsSuccess)
            {
                Console.WriteLine("Authentication success.", Color.Cyan);
                Console.WriteLineFormatted(
                       "{0}: {1}",
                       Color.White,
                       new Formatter[]
                       {
                            new Formatter("Context ID", Color.White),
                            new Formatter(authentication.ContextID, Color.PaleGreen)
                       }
                   );
            }
            else
            {
                Console.WriteLine("Authentication failed.", Color.Cyan);
                Console.WriteLine(authentication.Error.ErrorMessage, Color.Gray);
            }
            Console.WriteLine();

            // get device list
            DevicesResponse devices = new Devices()
                .PerformRequestAsync()
                .Result;

            if (devices.IsSuccess)
            {
                Console.WriteLine("Device list:", Color.Cyan);
                foreach (Device device in devices.Devices.OrderBy(d => !d.IsConnected).ThenBy(d => d.DisplayName))
                {
                    Console.WriteLineFormatted(
                        " - {0} ({1}{2}{3})",
                        Color.White,
                        new Formatter[]
                        {
                            new Formatter(device.DisplayName, Color.White),
                            new Formatter(
                                device.IsConnected ? device.HasAddress ? device.Addresses[0].IpAddress : "no address" : "disconnected",
                                device.IsConnected ? device.HasAddress ? Color.PaleGreen : Color.LightGoldenrodYellow : Color.IndianRed
                            ),
                            new Formatter(
                                device.IsConnected ? device.IsEthernet ? ", " : ", " : "",
                                Color.White
                            ),
                            new Formatter(
                                device.IsConnected ? device.IsEthernet ? "ethernet" : "wireless" : "",
                                device.IsConnected ? device.IsEthernet ? Color.PaleGreen : Color.LightGoldenrodYellow : Color.White
                            )
                        }
                    );
                }
            }
            else
            {
                Console.WriteLine("Failed.", Color.Cyan);
                Console.WriteLine(authentication.Error.ErrorMessage, Color.Gray);
            }
            Console.WriteLine();

            // disables wifi
            WsResponse ws = new WsRequest(
                "NMC.Wifi", "set",
                new Dictionary<string, string>()
                {
                    { "Enable", "False" },
                    { "Status", "False" }
                })
                .PerformRequestAsync()
                .Result;

            Console.WriteLine("Disabling Wi-Fi.", Color.Cyan);
            if (ws.IsSuccess)
            {
                Console.WriteLineFormatted(
                    "{0}.",
                    Color.White,
                    new Formatter[]
                    {
                        new Formatter("Succeed", Color.PaleGreen),
                    }
                );
            }
            else
            {
                Console.WriteLineFormatted(
                    "{0}. {1} ({2}).",
                    Color.White,
                    new Formatter[]
                    {
                        new Formatter("Failed", Color.IndianRed),
                        new Formatter(ws.Error.ErrorMessage, Color.Gray),
                        new Formatter(ws.Error.ErrorCode, Color.Gray)
                    }
                );
            }
            Console.WriteLine();

            // gets ws paths
            Console.WriteLine("Listing WS paths.", Color.Cyan);
            foreach (WsPath path in WsRequest.GetWsPaths().Result)
                Console.WriteLineFormatted(
                    " - {0} [ {1} ]",
                    Color.White,
                    new Formatter[]
                    {
                        new Formatter(path.Path, Color.PaleGreen),
                        new Formatter(string.Join(", ", path.Methods.ToArray()), Color.Gray),
                    }
                );

            Console.Read();
        }
    }
}
