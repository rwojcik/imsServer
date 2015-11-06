using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMSServer.Models
{
    public abstract class DeviceHistoryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HistoryId { get; set; }

        [Required]
        public DateTime RecordedAt { get; set; }

        [Required]
        public long DeviceId { get; set; }

        [ForeignKey("DeviceId")]
        public DeviceModel Device { get; set; }

        [Required]
        public string ChangedBy { get; set; }
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
