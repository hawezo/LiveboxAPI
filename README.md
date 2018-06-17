# Livebox API

This is an unfinished wrapper that allows to interact with a Livebox 3 or 4.
It is a C# implementation of [@rene-d](https://github.com/rene-d/)'s work on his [sysbus](https://github.com/rene-d/sysbus) script.

# How to use it

Refer to [the sample program](Sample/Program.cs#L20) to see usage examples.

## Authenticating

An authentication is done with the `Authentication` request. 

```csharp
AuthenticationResponse authentication =
    new Authentication(new Credentials("admin", password"))
    .PerformRequestAsync()
    .Result;
```

You can check if the request is suscessful with `authentication.IsSuccess`. Check `authentication.Error` if previous property is false.

If the authentication is successful, the context for your session is automatically saved. If you need it, it's in `Settings.Context`. `Settings` also contains modifiable data.

## Listing devices

You can list all devices that have been once connected to your Livebox by using the `Devices` object.

```csharp
DevicesResponse devices = new Devices()
    .PerformRequestAsync()
    .Result;
```

You can check if the request is suscessful the same way you did for the authentication request.

## Calling the API

As [@rene-d](https://github.com/rene-d/) did with his script, you can scrap your Livebox's script file in order to get all callable endpoints.

```csharp
foreach (WsPath path in WsRequest.GetWsPaths().Result)
    // use path.Path and path.Methods
```

Once you have a path, you can request the API.

```csharp
 WsResponse ws = new WsRequest(
    "NMC.Wifi", "set",
    new Dictionary<string, string>()
    {
        { "Enable", "False" },
        { "Status", "False" }
    })
    .PerformRequestAsync()
    .Result;
```

`MMC.Wifi` is an endpoint (`path.Path`) and `set` is a method (`path.Methods`). The dictionary will be converted into a JSON string in the body of the request ([see here](https://github.com/rene-d/sysbus/blob/master/sysbus.py#L356)).

This call to `MMC.Wifi` with those settings will disable your Wi-Fi. In order to find the settings, [you'll have to look by yourself](https://github.com/rene-d/sysbus#où-trouver-les-requêtes-) to the `script.js` file of your Livebox.
