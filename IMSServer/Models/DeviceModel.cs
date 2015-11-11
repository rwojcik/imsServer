using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMSServer.Models
{
    public abstract class DeviceModel : ModelBase
    {
        [Required, Index("UniqueDeviceName", IsUnique = true)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public long BuildingId { get; set; }

        [ForeignKey("BuildingId")]
        public virtual BuildingModel Building { get; set; }

        [Required]
        public DeviceType DeviceType { get; set; }

        [Required]
        public bool RecordHistory { get; set; }
        
        public virtual ICollection<DeviceHistoryModel> DeviceHistory { get; set; } 
    }

    public class BinarySettingDeviceModel : DeviceModel
    {
        public bool BinarySetting { get; set; }
    }

    public class ContinousSettingDeviceModel : DeviceModel
    {
        public double ContinousSetting { get; set; }
    }

    public enum DeviceType
    {
        AutomaticWindow = 1,
        Window = 2,
        Thermometer = 3,
        Thermostat = 4,
        Door = 5,
    }
}
