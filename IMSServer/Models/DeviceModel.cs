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

    public static class DeviceGenericOps
    {
        public static void UpdateModelGeneric(this DeviceModel oldModel, DeviceModel newModel)
        {
            if (oldModel.GetType() != newModel.GetType())
            {
                throw new IncompatibleTypeException($"Expected {oldModel.GetType()}, but got {newModel.GetType()}.");
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

        public static DeviceHistoryModel CreateDeviceHistoryModel(this DeviceModel model)
        {
            DeviceHistoryModel historyModel = null;

            if (model is BinarySettingDeviceModel)
            {
                var binaryModel = (BinarySettingDeviceModel)model;

                historyModel = new BinaryDeviceHistoryModel
                {
                    BinarySetting = binaryModel.BinarySetting
                };
            }
            else if (model is ContinousSettingDeviceModel)
            {
                var continousModel = (ContinousSettingDeviceModel)model;

                historyModel = new ContinousDeviceHistoryModel
                {
                    ContinousSetting = continousModel.ContinousSetting
                };
            }

            if(historyModel == null) throw new IncompatibleTypeException($"Given type {model.GetType()} was not recognized.");

            historyModel.ChangedBy = model.UpdatedBy;
            historyModel.RecordedAt = model.UpdatedAt;
            historyModel.Device = model;
            historyModel.DeviceId = model.Id;
            
            return historyModel;
        }
    }

    public class IncompatibleTypeException : Exception
    {
        public IncompatibleTypeException()
        {
        }

        public IncompatibleTypeException(string msg) : base(msg)
        {
        }

        public IncompatibleTypeException(string msg, Exception inner) : base(msg, inner)
        {
        }
    }
}
