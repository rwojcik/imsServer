using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IMSServer.ViewModels;

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

    public static class DeviceHistoryOps
    {
        public static DeviceHistoryViewModel CreateDeviceHistoryViewModel(this DeviceHistoryModel model)
        {
            if (model == null) return null;

            var deviceHistoryViewModel = new DeviceHistoryViewModel
            {
                ChangedBy = model.ChangedBy,
                RecordedAt = model.RecordedAt
            };

            if (model is ContinousDeviceHistoryModel)
            {
                deviceHistoryViewModel.Discriminator = "Continous";
                deviceHistoryViewModel.ContinousSetting = ((ContinousDeviceHistoryModel) model).ContinousSetting;
            }
            else if(model is BinaryDeviceHistoryModel)
            {
                deviceHistoryViewModel.Discriminator = "Binary";
                deviceHistoryViewModel.BinarySetting = ((BinaryDeviceHistoryModel) model).BinarySetting;
            }
            
            return deviceHistoryViewModel;
        }
    }
}
