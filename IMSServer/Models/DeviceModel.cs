using IMSServer.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

namespace IMSServer.Models
{
    public abstract class DeviceModel : ModelBase
    {
        //[Required, Index("UniqueDeviceGuid", IsUnique = true), DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public Guid Guid { get; set; }
        //bug

        [Required, Index("UniqueDeviceName", IsUnique = true), StringLength(250)]
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

        public static DeviceHistoryModel CreateDeviceHistoryModel(this DeviceModel deviceModel)
        {
            DeviceHistoryModel historyModel = null;

            if (deviceModel is BinarySettingDeviceModel)
            {
                var binaryModel = (BinarySettingDeviceModel)deviceModel;

                historyModel = new BinaryDeviceHistoryModel
                {
                    BinarySetting = binaryModel.BinarySetting
                };
            }
            else if (deviceModel is ContinousSettingDeviceModel)
            {
                var continousModel = (ContinousSettingDeviceModel)deviceModel;

                historyModel = new ContinousDeviceHistoryModel
                {
                    ContinousSetting = continousModel.ContinousSetting
                };
            }

            if (historyModel == null) throw new IncompatibleTypeException($"Given type {deviceModel.GetType()} was not recognized.");

            historyModel.ChangedBy = deviceModel.UpdatedBy;
            historyModel.RecordedAt = deviceModel.UpdatedAt;
            historyModel.Device = deviceModel;
            historyModel.DeviceId = deviceModel.Id;
            historyModel.UpdatedAt = deviceModel.UpdatedAt;
            historyModel.UpdatedBy = deviceModel.UpdatedBy;

            return historyModel;
        }

        public static DeviceModel CreateDeviceModel(this AddDeviceViewModel addDeviceViewModel)
        {
            if (addDeviceViewModel.Discriminator == "Continous")
            {
                if (!addDeviceViewModel.ContinousSetting.HasValue)
                    return null;

                return new ContinousSettingDeviceModel
                {
                    GroupId = addDeviceViewModel.GroupId,
                    ContinousSetting = addDeviceViewModel.ContinousSetting.Value,
                    Description = addDeviceViewModel.Description,
                    DeviceType = addDeviceViewModel.DeviceType,
                    Name = addDeviceViewModel.Name,
                };
            }
            else if (addDeviceViewModel.Discriminator == "Binary")
            {
                if (!addDeviceViewModel.BinarySetting.HasValue)
                    return null;

                return new BinarySettingDeviceModel
                {
                    GroupId = addDeviceViewModel.GroupId,
                    BinarySetting = addDeviceViewModel.BinarySetting.Value,
                    Description = addDeviceViewModel.Description,
                    DeviceType = addDeviceViewModel.DeviceType,
                    Name = addDeviceViewModel.Name,
                };
            }
            else
            {
                return null;
            }
        }

        public static DeviceViewModel CreateDeviceViewModel(this DeviceModel deviceModel)
        {

            var deviceViewModel = new DeviceViewModel
            {
                Description = deviceModel.Description,
                DeviceId = deviceModel.Id,
                GroupId = deviceModel.GroupId,
                Name = deviceModel.Name,
                DeviceType = deviceModel.DeviceType,
                History = deviceModel.DeviceHistory?.Take(10).Select(dev => dev.CreateDeviceHistoryViewModel()),
            };

            if (deviceModel is ContinousSettingDeviceModel)
            {
                deviceViewModel.Discriminator = "Continous";
                deviceViewModel.ContinousSetting = ((ContinousSettingDeviceModel)deviceModel).ContinousSetting;
            }
            else if (deviceModel is BinarySettingDeviceModel)
            {
                deviceViewModel.Discriminator = "Binary";
                deviceViewModel.BinarySetting = ((BinarySettingDeviceModel)deviceModel).BinarySetting;
            }
            else
            {
                throw new IncompatibleTypeException($"Given type {deviceModel.GetType()} was not recognized.");
            }

            return deviceViewModel;
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
