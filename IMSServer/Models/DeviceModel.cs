using System;
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
        public long GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual GroupModel Group { get; set; }

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
        Alarm = 6,
    }

    public static class UpdateGeneric
    {
        public static void UpdateModelGeneric(this DeviceModel oldModel, DeviceModel newModel)
        {
            if (oldModel.GetType() != newModel.GetType())
            {
                throw new IncompatibleTypeExceprion($"Expected {oldModel.GetType()}, but got {newModel.GetType()}");
            }

            oldModel.Name = newModel.Name;
            oldModel.Description = newModel.Description;
            oldModel.UpdatedAt = DateTime.Now;
            oldModel.UpdatedBy = newModel.UpdatedBy;

            if (oldModel is BinarySettingDeviceModel)
            {
                var binaryOldModel = (BinarySettingDeviceModel)oldModel;
                var binaryNewModel = (BinarySettingDeviceModel)newModel;

                binaryOldModel.BinarySetting = binaryNewModel.BinarySetting;

            }
            else if (newModel is ContinousSettingDeviceModel)
            {
                var continousOldModel = (ContinousSettingDeviceModel)oldModel;
                var continousNewModel = (ContinousSettingDeviceModel)newModel;

                continousOldModel.ContinousSetting = continousNewModel.ContinousSetting;
            }
        }
    }

    public class IncompatibleTypeExceprion : Exception
    {
        public IncompatibleTypeExceprion()
        {
        }

        public IncompatibleTypeExceprion(string msg) : base(msg)
        {
        }

        public IncompatibleTypeExceprion(string msg, Exception inner) : base(msg, inner)
        {
        }
    }
}
