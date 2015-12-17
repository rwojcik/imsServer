using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IMSServer.ViewModels;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace IMSServer.Hub
{
    [HubName("device")]
    public class DeviceHub : Microsoft.AspNet.SignalR.Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void HelloMsg(string msg)
        {
            Clients.All.helloMsg(msg);
        }

        public void BinaryDeviceUpdate(long deviceId, bool setting)
        {
            Clients.All.binaryDeviceUpdate(deviceId, setting);
        }

        public void ContinousDeviceUpdate(long deviceId, double setting)
        {
            Clients.All.continousDeviceUpdate(deviceId, setting);
        }

        public void BinaryDeviceUpdated(long deviceId, bool success, bool binarySetting)
        {
            Clients.All.binaryDeviceUpdated(deviceId, success, binarySetting);
        }
        public void ContinousDeviceUpdated(long deviceId, bool success, double continousSetting)
        {
            Clients.All.continousDeviceUpdated(deviceId, success, continousSetting);
        }
    }
}