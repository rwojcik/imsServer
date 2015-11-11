using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMSServer.Models
{
    public abstract class DeviceHistoryModel : ModelBase
    {
        [Required]
        public long DeviceId { get; set; }

        [ForeignKey("DeviceId")]
        public DeviceModel Device { get; set; }

        [NotMapped]
        public string ChangedBy
        {
            get { return CreatedBy; }
            set { CreatedBy = value; }
        }

        [NotMapped]
        public DateTime RecordedAt
        {
            get { return CreatedAt; }
            set { CreatedAt = value; }
        }
    }

    public class BinaryDeviceHistoryModel : DeviceHistoryModel
    {
        public bool BinarySetting { get; set; }
    }

    public class ContinousDeviceHistoryModel : DeviceHistoryModel
    {
        public double ContinousSetting { get; set; }
    }
}
