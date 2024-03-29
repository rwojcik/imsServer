﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IMSServer.Models;

namespace IMSServer.Repositories
{
    public class DeviceRepository : IRepository<DeviceModel>
    {
        private readonly IMSServerContext _dbContext;
        private readonly string _userName;

        public DeviceRepository(string userName)
        {
            _userName = userName;
            _dbContext = new IMSServerContext();
        }

        public DeviceModel Get(long id)
        {
            return _dbContext.DeviceModels.Include(dev => dev.DeviceHistory).FirstOrDefault(d => d.Id == id);
        }

        public Task<DeviceModel> FindAsync(long id)
        {
            return _dbContext.DeviceModels.FirstOrDefaultAsync(d => d.Id == id);
        }

        public IEnumerable<DeviceModel> GetAll(Expression<Func<DeviceModel, bool>> predicate)
        {
            return _dbContext.DeviceModels.Where(predicate).AsEnumerable();
        }

        public IEnumerable<DeviceModel> GetAll()
        {
            return _dbContext.DeviceModels.AsEnumerable();
        }

        public DeviceModel GetFirst(Expression<Func<DeviceModel, bool>> predicate)
        {
            return _dbContext.DeviceModels.FirstOrDefault(predicate);
        }

        public Task<DeviceModel> GetFirstAsync(Expression<Func<DeviceModel, bool>> predicate)
        {
            return _dbContext.DeviceModels.FirstOrDefaultAsync(predicate);
        }

        public DeviceModel Add(DeviceModel entity)
        {
            if (entity == null) return null;

            entity.AuditEntity(_userName);

            var newEntity = _dbContext.DeviceModels.Add(entity);
            
            _dbContext.SaveChanges();
            
            var historyEntry = newEntity.CreateDeviceHistoryModel();

            newEntity.DeviceHistory.Add(historyEntry);

            _dbContext.SaveChanges();

            return newEntity;
        }

        public async Task<DeviceModel> AddAsync(DeviceModel entity)
        {
            if (entity == null) return null;

            entity.AuditEntity(_userName);

            var newEntity = _dbContext.DeviceModels.Add(entity);

            await _dbContext.SaveChangesAsync();

            var historyEntry = newEntity.CreateDeviceHistoryModel();

            if(newEntity.DeviceHistory == null)
                newEntity.DeviceHistory = new List<DeviceHistoryModel>(0);

            newEntity.DeviceHistory.Add(historyEntry);

            await _dbContext.SaveChangesAsync();

            return newEntity;
        }

        public DeviceModel Remove(long id)
        {
            var entity = _dbContext.DeviceModels.FirstOrDefault(x => x.Id == id);
            if (entity == null) return null;
            _dbContext.DeviceModels.Remove(entity);

            _dbContext.SaveChanges();

            return entity;
        }

        public async Task<DeviceModel> RemoveAsync(long id)
        {
            var entity = await FindAsync(id);
            if (entity == null) return null;
            _dbContext.DeviceModels.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public DeviceModel Update(DeviceModel entity)
        {
            if (entity == null) return null;
            var old = Get(entity.Id);
            if (old == null) return null;

            old.UpdateModelGeneric(entity);
            old.AuditEntity(_userName);
            old.DeviceHistory.Add(old.CreateDeviceHistoryModel());
            
            _dbContext.SaveChanges();

            return old;
        }

        public async Task<DeviceModel> UpdateAsync(DeviceModel entity)
        {
            if (entity == null) return null;
            var old = await FindAsync(entity.Id);
            if (old == null) return null;

            old.UpdateModelGeneric(entity);
            old.AuditEntity(_userName);
            old.DeviceHistory.Add(old.CreateDeviceHistoryModel());

            await _dbContext.SaveChangesAsync();

            return old;
        }
    }
}
