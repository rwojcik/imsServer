namespace IMSServer.ViewModels
{
    public class DeviceViewModel
    {
        public long DeviceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BuildingViewModel Building { get; set; }
    }

    public class AddDeviceViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long BuildingId { get; set; }

    }

    public class UpdateDeviceViewModel
    {
        public long DeviceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long BuildingId { get; set; }
    }
}
