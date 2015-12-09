﻿using System;
using System.Collections.Generic;
using IMSServer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IMSServer.Tests.Models
{
    [TestClass]
    public class DeviceTest
    {
        [TestMethod]
        public void CreateContDeviceHistory()
        {
            var devicesGroup = new GroupModel
            {
                Id = 1,
                Description = "TestDescription",
                Devices = new List<DeviceModel>(),
                Name = "TestGroup",
            };

            devicesGroup.AuditEntity("TestUser");

            var model = new ContinousSettingDeviceModel
            {
                Id = 1,
                ContinousSetting = 20.5d,
                Description = "TestDescription",
                DeviceHistory = new List<DeviceHistoryModel>(),
                Name = "TestDevice",
                DeviceType = DeviceType.Thermometer,
                Group = devicesGroup,
                GroupId = devicesGroup.Id,
            };

            model.AuditEntity("TestUser");

            var historyEntity = model.CreateDeviceHistoryModel();

            Assert.IsNotNull(historyEntity);

            Assert.IsFalse(historyEntity is BinaryDeviceHistoryModel);

            Assert.IsTrue(historyEntity is ContinousDeviceHistoryModel);

            var contHistoryEntity = (ContinousDeviceHistoryModel) historyEntity;

            Assert.AreEqual(model.ContinousSetting, contHistoryEntity.ContinousSetting, 1d);
        }

        [TestMethod]
        public void CreateBinaryDeviceHistory()
        {
            var devicesGroup = new GroupModel
            {
                Id = 1,
                Description = "TestDescription",
                Devices = new List<DeviceModel>(),
                Name = "TestGroup",
            };

            devicesGroup.AuditEntity("TestUser");
            
            var model = new BinarySettingDeviceModel
            {
                Id = 1,
                BinarySetting = false,
                Description = "TestDescription",
                DeviceHistory = new List<DeviceHistoryModel>(),
                Name = "TestDevice",
                DeviceType = DeviceType.Thermometer,
                Group = devicesGroup,
                GroupId = devicesGroup.Id,
            };

            model.AuditEntity("TestUser");
            
            var historyEntity = model.CreateDeviceHistoryModel();

            Assert.IsNotNull(historyEntity);

            Assert.IsFalse(historyEntity is ContinousDeviceHistoryModel);

            Assert.IsTrue(historyEntity is BinaryDeviceHistoryModel);

            var contHistoryEntity = (BinaryDeviceHistoryModel)historyEntity;

            Assert.AreEqual(model.BinarySetting, contHistoryEntity.BinarySetting);
        }

    }
}
