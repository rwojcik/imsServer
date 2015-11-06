using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using IMSServer.Models;
using IMSServer.ViewModels;

namespace IMSServer.Controllers
{
    public class BuildingController : ApiController
    {
        private readonly IMSServerContext _dbContext = new IMSServerContext();

        // GET: api/Building
        public IQueryable<BuildingViewModel> GetBuildingModels()
        {
            return _dbContext.BuildingModels.Select(building => new BuildingViewModel
            {
                BuildingId = building.BuildingId,
                CreatedAt = building.CreatedAt,
                CreatedBy = building.CreatedBy,
                Description = building.Description,
                DevicesIds = building.Devices.Select(device => device.DeviceId).ToList(),
                Name = building.Name,
                UpdatedAt = building.UpdatedAt,
                UpdatedBy = building.CreatedBy
            });
        }

        // GET: api/Building/5
        [ResponseType(typeof(BuildingViewModel))]
        public async Task<IHttpActionResult> GetBuildingModel(long id)
        {
            var buildingModel = await _dbContext.BuildingModels.FindAsync(id);
            if (buildingModel == null)
            {
                return NotFound();
            }

            var buildingVm = new BuildingViewModel
            {
                BuildingId = buildingModel.BuildingId,
                CreatedAt = buildingModel.CreatedAt,
                CreatedBy = buildingModel.CreatedBy,
                Description = buildingModel.Description,
                DevicesIds = buildingModel.Devices.Select(device => device.DeviceId).ToList(),
                Name = buildingModel.Name,
                UpdatedAt = buildingModel.UpdatedAt,
                UpdatedBy = buildingModel.CreatedBy
            };

            return Ok(buildingVm);
        }

        // PUT: api/Building/5
        // Update
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBuildingModel(long id, UpdateBuildingViewModel buildingVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != buildingVm.BuildingId)
            {
                return BadRequest("Ids do not match");
            }

            var buildingModel = await _dbContext.BuildingModels.FindAsync(buildingVm.BuildingId);

            if (buildingModel == null) return NotFound();

            buildingModel.Description = buildingVm.Description;
            buildingModel.Description = buildingVm.Description;
            buildingModel.UpdatedAt = DateTime.Now;
            buildingModel.UpdatedBy = User.Identity.Name;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Building
        // Add
        [ResponseType(typeof(BuildingModel))]
        public async Task<IHttpActionResult> PostBuildingModel(AddBuildingViewModel buildingVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var buildingModel = new BuildingModel
            {
                Name = buildingVm.Name,
                Description = buildingVm.Description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = User.Identity.Name,
                UpdatedBy = User.Identity.Name,
            };

            _dbContext.BuildingModels.Add(buildingModel);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }

            return CreatedAtRoute("DefaultApi", new { id = buildingModel.BuildingId }, buildingModel);
        }

        // DELETE: api/Building/5
        [ResponseType(typeof(BuildingModel))]
        public async Task<IHttpActionResult> DeleteBuildingModel(long id)
        {
            var buildingModel = await _dbContext.BuildingModels.FindAsync(id);
            if (buildingModel == null)
            {
                return NotFound();
            }

            var buildingVm = new BuildingViewModel
            {
                BuildingId = buildingModel.BuildingId,
                CreatedAt = buildingModel.CreatedAt,
                CreatedBy = buildingModel.CreatedBy,
                Description = buildingModel.Description,
                DevicesIds = buildingModel.Devices.Select(device => device.DeviceId).ToList(),
                Name = buildingModel.Name,
                UpdatedAt = buildingModel.UpdatedAt,
                UpdatedBy = buildingModel.CreatedBy
            };

            _dbContext.BuildingModels.Remove(buildingModel);
            await _dbContext.SaveChangesAsync();

            return Ok(buildingVm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BuildingModelExists(long id)
        {
            return _dbContext.BuildingModels.Any(e => e.BuildingId == id);
        }
    }
}