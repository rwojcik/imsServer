﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IMSServer.Models;

namespace IMSServer.Repositories
{
    public class GroupRepository : IRepository<GroupModel>
    {
        private readonly IMSServerContext _dbContext;
        private readonly string _userName;

        public GroupRepository(string userName)
        {
            _userName = userName;
            _dbContext = new IMSServerContext();
        }

        public GroupModel Get(long id)
        {
            return _dbContext.GroupModels.FirstOrDefault(x => x.Id == id);
        }

        public Task<GroupModel> FindAsync(long id)
        {
            return _dbContext.GroupModels.FindAsync(id);
        }

        public IEnumerable<GroupModel> GetAll(Expression<Func<GroupModel, bool>> predicate)
        {
            return _dbContext.GroupModels.Include(gr => gr.Devices).Where(predicate);
        }

        public IEnumerable<GroupModel> GetAll()
        {
            return _dbContext.GroupModels.Include(gr => gr.Devices);
        }
        
        public GroupModel GetFirst(Expression<Func<GroupModel, bool>> predicate)
        {
            return _dbContext.GroupModels.FirstOrDefault(predicate);
        }

        public async Task<GroupModel> GetFirstAsync(Expression<Func<GroupModel, bool>> predicate)
        {
            return await _dbContext.GroupModels.FirstOrDefaultAsync(predicate);
        }

        public GroupModel Add(GroupModel entity)
        {
            if (entity == null) return null;
            entity.AuditEntity(_userName);
            var newEntity = _dbContext.GroupModels.Add(entity);
            _dbContext.SaveChanges();
            return newEntity;
        }

        public async Task<GroupModel> AddAsync(GroupModel entity)
        {
            if (entity == null) return null;
            entity.AuditEntity(_userName);

            var newEntity = _dbContext.GroupModels.Add(entity);

            await _dbContext.SaveChangesAsync();

            return newEntity;
        }

        public GroupModel Remove(long id)
        {
            var entity = _dbContext.GroupModels.FirstOrDefault(x => x.Id == id);
            if (entity == null) return null;
            _dbContext.GroupModels.Remove(entity);

            _dbContext.SaveChanges();

            return entity;
        }

        public async Task<GroupModel> RemoveAsync(long id)
        {
            var entity = await FindAsync(id);
            if (entity == null) return null;
            _dbContext.GroupModels.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public GroupModel Update(GroupModel entity)
        {
            if (entity == null) return null;
            var old = _dbContext.GroupModels.FirstOrDefault(x => x.Id == entity.Id);
            if (old == null) return null;

            old.Name = entity.Name;
            old.Description = entity.Description;
            old.AuditEntity(_userName);

            _dbContext.SaveChanges();

            return old;
        }

        public async Task<GroupModel> UpdateAsync(GroupModel entity)
        {
            if (entity == null) return null;
            var old = await FindAsync(entity.Id);
            if (old == null) return null;

            old.Name = entity.Name;
            old.Description = entity.Description;
            old.AuditEntity(_userName);

            await _dbContext.SaveChangesAsync();

            return old;
        }
    }
}
