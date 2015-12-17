using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IMSServer.Models;
using Newtonsoft.Json;

namespace IMSServer.ViewModels
{
    public class DeviceViewModel
    {
        public long DeviceId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long GroupId { get; set; }

        public IEnumerable<DeviceHistoryViewModel> History { get; set; }

        public DeviceType DeviceType { get; set; }

        public string Discriminator { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? ContinousSetting { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? BinarySetting { get; set; }
    }

    public class AddDeviceViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public long GroupId { get; set; }
        public DeviceType DeviceType { get; set; }

        [Required]
        public string Discriminator { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? ContinousSetting { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? BinarySetting { get; set; }

    }

    public class UpdateDeviceViewModel
    {
        [Required]
        public long DeviceId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public long GroupId { get; set; }

        [Required]
        public string Discriminator { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? ContinousSetting { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? BinarySetting { get; set; }
    }

    public class DeviceHistoryViewModel
    {
        public string ChangedBy { get; set; }

        public DateTime RecordedAt { get; set; }
        
        public string Discriminator { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? ContinousSetting { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? BinarySetting { get; set; }
    }
}
