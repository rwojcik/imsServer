using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IMSServer.Models;

namespace IMSServer.Repositories
{
    public class BuildingRepository : IRepository<BuildingModel>
    {
        private readonly IMSServerContext _dbContext;
        private readonly string _userName;

        public BuildingRepository(IMSServerContext dbContext, string userName)
        {
            _dbContext = dbContext;
            _userName = userName;
        }

        public BuildingModel Get(long id)
        {
            return _dbContext.BuildingModels.FirstOrDefault(x => x.Id == id);
        }

        public Task<BuildingModel> FindAsync(long id)
        {
            return _dbContext.BuildingModels.FindAsync(id);
        }

        public IEnumerable<BuildingModel> GetAll(Expression<Func<BuildingModel, bool>> predicate)
        {
            return _dbContext.BuildingModels.Where(predicate);
        }

        public IEnumerable<BuildingModel> GetAll()
        {
            return _dbContext.BuildingModels.AsEnumerable();
        }
        
        public BuildingModel GetFirst(Expression<Func<BuildingModel, bool>> predicate)
        {
            return _dbContext.BuildingModels.FirstOrDefault(predicate);
        }

        public Task<BuildingModel> GetFirstAsync(Expression<Func<BuildingModel, bool>> predicate)
        {
            return _dbContext.BuildingModels.FirstOrDefaultAsync(predicate);
        }

        public BuildingModel Add(BuildingModel entity)
        {
            if (entity == null) return null;

            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = _userName;
            entity.CreatedBy = _userName;

            var newEntity = _dbContext.BuildingModels.Add(entity);
            _dbContext.SaveChanges();
            return newEntity;
        }

        public async Task<BuildingModel> AddAsync(BuildingModel entity)
        {
            if (entity == null) return null;

            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = _userName;
            entity.CreatedBy = _userName;

            var newEntity = _dbContext.BuildingModels.Add(entity);

            await _dbContext.SaveChangesAsync();

            return newEntity;
        }

        public BuildingModel Remove(long id)
        {
            var entity = _dbContext.BuildingModels.FirstOrDefault(x => x.Id == id);
            if (entity == null) return null;
            _dbContext.BuildingModels.Remove(entity);

            _dbContext.SaveChanges();

            return entity;
        }

        public async Task<BuildingModel> RemoveAsync(long id)
        {
            var entity = await _dbContext.BuildingModels.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return null;
            _dbContext.BuildingModels.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public BuildingModel Update(BuildingModel entity)
        {
            if (entity == null) return null;
            var old = _dbContext.BuildingModels.FirstOrDefault(x => x.Id == entity.Id);
            if (old == null) return null;

            old.Name = entity.Name;
            old.Description = entity.Description;
            old.UpdatedAt = DateTime.Now;
            old.UpdatedBy = _userName;

            _dbContext.SaveChanges();

            return old;
        }

        public async Task<BuildingModel> UpdateAsync(BuildingModel entity)
        {
            if (entity == null) return null;
            var old = await _dbContext.BuildingModels.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (old == null) return null;

            old.Name = entity.Name;
            old.Description = entity.Description;
            old.UpdatedAt = DateTime.Now;
            old.UpdatedBy = _userName;

            await _dbContext.SaveChangesAsync();

            return old;
        }
    }
}
