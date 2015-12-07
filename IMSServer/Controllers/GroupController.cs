using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using IMSServer.Models;
using IMSServer.Repositories;
using IMSServer.ViewModels;

namespace IMSServer.Controllers
{
    public class GroupController : ApiController
    {
        private readonly GroupRepository _repository;

        public GroupController()
        {
            _repository = new GroupRepository();
        }

        // GET: api/Group
        public IEnumerable<GroupViewModel> GetGroupModels()
        {
            return _repository.GetAll().Select(building => new GroupViewModel
            {
                BuildingId = building.Id,
                CreatedAt = building.CreatedAt,
                CreatedBy = building.CreatedBy,
                Description = building.Description,
                DevicesIds = building.Devices.Select(device => device.Id).ToList(),
                Name = building.Name,
                UpdatedAt = building.UpdatedAt,
                UpdatedBy = building.CreatedBy,
            });
        }

        // GET: api/Group/5
        [ResponseType(typeof(GroupViewModel))]
        public async Task<IHttpActionResult> GetGroupModel(long id)
        {
            var buildingModel = await _repository.FindAsync(id);
            if (buildingModel == null)
            {
                return NotFound();
            }

            var buildingVm = new GroupViewModel
            {
                BuildingId = buildingModel.Id,
                CreatedAt = buildingModel.CreatedAt,
                CreatedBy = buildingModel.CreatedBy,
                Description = buildingModel.Description,
                DevicesIds = buildingModel.Devices.Select(device => device.Id).ToList(),
                Name = buildingModel.Name,
                UpdatedAt = buildingModel.UpdatedAt,
                UpdatedBy = buildingModel.CreatedBy,
            };

            return Ok(buildingVm);
        }

        // PUT: api/Group/5
        // Update
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGroupModel(long id, UpdateGroupViewModel groupVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != groupVm.GroupId)
            {
                return BadRequest("Ids do not match");
            }

            var buildingModel = new GroupModel
            {
                Id = groupVm.GroupId,
                Description = groupVm.Description,
                Name = groupVm.Name,
            };

            try
            {
                if (await _repository.UpdateAsync(buildingModel) == null)
                {
                    return NotFound();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Group
        // Add
        [ResponseType(typeof(GroupModel))]
        public async Task<IHttpActionResult> PostGroupModel(AddGroupViewModel groupVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var buildingModel = new GroupModel
            {
                Name = groupVm.Name,
                Description = groupVm.Description,
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

        // DELETE: api/Group/5
        [ResponseType(typeof(GroupModel))]
        public async Task<IHttpActionResult> DeleteGroupModel(long id)
        {
            var buildingModel = await _repository.RemoveAsync(id);

            if (buildingModel == null)
            {
                return NotFound();
            }

            var buildingVm = new GroupViewModel
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