using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using IMSServer.Models;
using IMSServer.Repositories;
using IMSServer.ViewModels;

namespace IMSServer.Controllers
{
    public class BuildingController : ApiController
    {
        private readonly BuildingRepository _repository;

        public BuildingController()
        {
            _repository = new BuildingRepository(new IMSServerContext(), User.Identity.Name);
        }

        // GET: api/Building
        public IEnumerable<BuildingViewModel> GetBuildingModels()
        {
            return _repository.GetAll().Select(building => new BuildingViewModel
            {
                BuildingId = building.Id,
                CreatedAt = building.CreatedAt,
                CreatedBy = building.CreatedBy,
                Description = building.Description,
                DevicesIds = building.Devices.Select(device => device.Id).ToList(),
                Name = building.Name,
                UpdatedAt = building.UpdatedAt,
                UpdatedBy = building.CreatedBy
            });
        }

        // GET: api/Building/5
        [ResponseType(typeof(BuildingViewModel))]
        public async Task<IHttpActionResult> GetBuildingModel(long id)
        {
            var buildingModel = await _repository.FindAsync(id);
            if (buildingModel == null)
            {
                return NotFound();
            }

            var buildingVm = new BuildingViewModel
            {
                BuildingId = buildingModel.Id,
                CreatedAt = buildingModel.CreatedAt,
                CreatedBy = buildingModel.CreatedBy,
                Description = buildingModel.Description,
                DevicesIds = buildingModel.Devices.Select(device => device.Id).ToList(),
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

            var buildingModel = await _repository.FindAsync(buildingVm.BuildingId);

            if (buildingModel == null) return NotFound();

            buildingModel.Description = buildingVm.Description;
            buildingModel.Description = buildingVm.Description;

            try
            {
                await _repository.UpdateAsync(buildingModel);
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
            };


            try
            {
                await _repository.AddAsync(buildingModel);
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }

            return CreatedAtRoute("DefaultApi", new { id = buildingModel.Id }, buildingModel);
        }

        // DELETE: api/Building/5
        [ResponseType(typeof(BuildingModel))]
        public async Task<IHttpActionResult> DeleteBuildingModel(long id)
        {
            var buildingModel = await _repository.RemoveAsync(id);

            if (buildingModel == null)
            {
                return NotFound();
            }

            var buildingVm = new BuildingViewModel
            {
                BuildingId = buildingModel.Id,
                CreatedAt = buildingModel.CreatedAt,
                CreatedBy = buildingModel.CreatedBy,
                Description = buildingModel.Description,
                DevicesIds = buildingModel.Devices.Select(device => device.Id).ToList(),
                Name = buildingModel.Name,
                UpdatedAt = buildingModel.UpdatedAt,
                UpdatedBy = buildingModel.CreatedBy
            };

            return Ok(buildingVm);
        }
    }
}