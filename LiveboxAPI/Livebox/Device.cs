using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

// TODO document

namespace Livebox.Objects
{

    public partial class Device
    {

        /// <summary>
        /// Physical address (or MAC) of this device.
        /// </summary>
        [JsonProperty("physAddress")]
        public string PhysAddress { get; internal set; }

        /// <summary>
        /// Current IP address of this device.
        /// </summary>
        [JsonProperty("ipAddress")]
        public string IpAddress { get; internal set; }
        
        /// <summary>
        /// Unknown property.
        /// </summary>
        [JsonProperty("addressSource")]
        public string AddressSource { get; internal set; }

        /// <summary>
        /// Detected types for this device. Livebox automatically detects types, but this is barely accurate.
        /// This is a list separated by spaces.
        /// </summary>
        [JsonProperty("detectedTypes")]
        public string DetectedTypes { get; internal set; }

        /// <summary>
        /// Lease time remaining for this device. The DHCP lease is how long a device reserves an IP address on your network.
        /// </summary>
        [JsonProperty("leaseTimeRemaining")]
        public long LeaseTimeRemaining { get; internal set; }

        /// <summary>
        /// Vendor class identifier of this device.
        /// The vendor class identifier is used to identify the manufacturer of the hardware running the DHCP client.
        /// </summary>
        [JsonProperty("vendorClassID")]
        public string VendorClassId { get; internal set; }

        /// <summary>
        /// Unique client identifier for this device.
        /// </summary>
        [JsonProperty("clientID")]
        public string ClientId { get; internal set; }

        /// <summary>
        /// User class identifier of this device.
        /// The user class identifier is a user-defined string.
        /// </summary>
        [JsonProperty("userClassID")]
        public string UserClassId { get; internal set; }

        /// <summary>
        /// Display name for the device, known as hostname or webui name on the Livebox.
        /// </summary>
        [JsonProperty("hostName")]
        public string DisplayName { get; internal set; }

        /// <summary>
        /// DNS name set by the Livebox. It is formatted with its name and both of its ID and alias(es).
        /// </summary>
        [JsonProperty("dnsName")]
        public string DnsName { get; internal set; }

        /// <summary>
        /// UPNP names for this device.
        /// </summary>
        [JsonProperty("uPnPNames")]
        public string UPnPNames { get; internal set; }

        /// <summary>
        /// myDNS names for this device.
        /// </summary>
        [JsonProperty("mDNSNames")]
        public string MDnsNames { get; internal set; }

        /// <summary>
        /// Unknown property.
        /// </summary>
        [JsonProperty("lLTDDevice")]
        public bool LLtdDevice { get; internal set; }

        /// <summary>
        /// Service Set Identifier of this device. Most have none.
        /// </summary>
        [JsonProperty("sSID")]
        public string Ssid { get; internal set; }

        /// <summary>
        /// Defines if the device is currently active (connected to the network).
        /// </summary>
        [JsonProperty("active")]
        public bool IsConnected { get; internal set; }

        /// <summary>
        /// Offset of the last connection of this device.
        /// </summary>
        [JsonProperty("lastConnection")]
        public DateTimeOffset LastConnection { get; internal set; }

        /// <summary>
        /// Tags used by the Livebox. It's a list with spaces as separator.
        /// </summary>
        [JsonProperty("tags")]
        public string Tags { get; internal set; }

        /// <summary>
        /// Interface this device is connected to. 
        /// - wlx (wlanx, wireless interface)
        /// - ethx (ethernet interface)
        /// </summary>
        [JsonProperty("layer2Interface")]
        public string Layer2Interface { get; internal set; }
        
        /// <summary>
        /// Unknown property.
        /// </summary>
        [JsonProperty("interfaceType")]
        public string InterfaceType { get; internal set; }

        /// <summary>
        /// Organizationally unique identifier of this device's manufacturer.
        /// </summary>
        [JsonProperty("manufacturerOUI")]
        public string ManufacturerOui { get; internal set; }

        /// <summary>
        /// Serial number of this device. Not always communicated.
        /// </summary>
        [JsonProperty("serialNumber")]
        public string SerialNumber { get; internal set; }
        
        /// <summary>
        /// Unknown property.
        /// </summary>
        [JsonProperty("productClass")]
        public string ProductClass { get; internal set; }

        /// <summary>
        /// Identifier of this icon used on the web interface.
        /// </summary>
        [JsonProperty("deviceIcon")]
        public string DeviceIcon { get; internal set; }

        /// <summary>
        /// Unknown property.
        /// </summary>
        [JsonProperty("deviceLocation")]
        public string DeviceLocation { get; internal set; }

        /// <summary>
        /// Set type for this device. By default, first <see cref="DetectedTypes"/>.
        /// </summary>
        [JsonProperty("deviceType")]
        public string DeviceType { get; internal set; }

        /// <summary>
        /// List of addresses for this device.
        /// </summary>
        [JsonProperty("addresses")]
        public List<Address> Addresses { get; internal set; }

        /// <summary>
        /// A list of names binded to the device. It contains the default name, the DNS names and the display (webui) name.
        /// </summary>
        [JsonProperty("names")]
        public List<Name> Names { get; internal set; }
        
        /// <summary>
        /// Unknown property.
        /// </summary>
        [JsonProperty("icons")]
        public List<object> Icons { get; internal set; }

        /// <summary>
        /// Defines if the device has an address. If not, it may not be connected, but better check with <see cref="IsConnected"/>, because some devices do not have addresses (CPL for instance).
        /// </summary>
        [JsonIgnore]
        public bool HasAddress
        {
            get => (this.Addresses.Count > 0);
        }

        /// <summary>
        /// Defines if the device is on ethernet or no.
        /// Using the interface the device is connected to.
        /// - wlx (wlanx, wireless interface)
        /// - ethx (ethernet interface)
        /// </summary>
        public bool IsEthernet
        {
            get => (this.Layer2Interface.StartsWith("eth"));
        }

        /// <summary>
        /// Fancy display text for this device.
        /// </summary>
        public override string ToString()
        {
            return ($"{this.DisplayName} ({(this.HasAddress ? this.Addresses[0].IpAddress : "no address")}, {this.Layer2Interface})");
        }
    }

    public partial class Address
    {
        [JsonProperty("ipAddress")]
        public string IpAddress { get; internal set; }

        [JsonProperty("addressSource")]
        public string AddressSource { get; internal set; }

        [JsonProperty("family")]
        public string Family { get; internal set; }

        [JsonProperty("scope", NullValueHandling = NullValueHandling.Ignore)]
        public string Scope { get; internal set; }
    }

    public partial class Name
    {
        [JsonProperty("name")]
        public string NameName { get; internal set; }

        [JsonProperty("source")]
        public string Source { get; internal set; }
    }

    public partial class Device
    {
        public static Device FromJson(string json) => JsonConvert.DeserializeObject<Device>(json, Converter.Settings);
        public string ToJson() => JsonConvert.SerializeObject(this, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
