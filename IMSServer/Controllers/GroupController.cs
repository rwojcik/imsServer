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
            return _repository.GetAll().Select(groupModel => new GroupViewModel
            {
                GroupId = groupModel.Id,
                CreatedAt = groupModel.CreatedAt,
                CreatedBy = groupModel.CreatedBy,
                Description = groupModel.Description,
                DevicesIds = groupModel.Devices.Select(device => device.Id).ToList(),
                Name = groupModel.Name,
                UpdatedAt = groupModel.UpdatedAt,
                UpdatedBy = groupModel.CreatedBy,
            });
        }

        // GET: api/Group/5
        [ResponseType(typeof(GroupViewModel))]
        public async Task<IHttpActionResult> GetGroupModel(long id)
        {
            var groupModel = await _repository.FindAsync(id);
            if (groupModel == null)
            {
                return NotFound();
            }

            var groupViewModel = new GroupViewModel
            {
                GroupId = groupModel.Id,
                CreatedAt = groupModel.CreatedAt,
                CreatedBy = groupModel.CreatedBy,
                Description = groupModel.Description,
                DevicesIds = groupModel.Devices.Select(device => device.Id).ToList(),
                Name = groupModel.Name,
                UpdatedAt = groupModel.UpdatedAt,
                UpdatedBy = groupModel.CreatedBy,
            };

            return Ok(groupViewModel);
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

            var groupModel = new GroupModel
            {
                Id = groupVm.GroupId,
                Description = groupVm.Description,
                Name = groupVm.Name,
            };

            try
            {
                if (await _repository.UpdateAsync(groupModel) == null)
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

            var groupModel = new GroupModel
            {
                Name = groupVm.Name,
                Description = groupVm.Description,
            };
            
            try
            {
                await _repository.AddAsync(groupModel);
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }

            return CreatedAtRoute("DefaultApi", new { id = groupModel.Id }, groupModel);
        }

        // DELETE: api/Group/5
        [ResponseType(typeof(GroupModel))]
        public async Task<IHttpActionResult> DeleteGroupModel(long id)
        {
            var groupModel = await _repository.RemoveAsync(id);

            if (groupModel == null)
            {
                return NotFound();
            }

            var groupViewModel = new GroupViewModel
            {
                GroupId = groupModel.Id,
                CreatedAt = groupModel.CreatedAt,
                CreatedBy = groupModel.CreatedBy,
                Description = groupModel.Description,
                DevicesIds = groupModel.Devices.Select(device => device.Id).ToList(),
                Name = groupModel.Name,
                UpdatedAt = groupModel.UpdatedAt,
                UpdatedBy = groupModel.CreatedBy
            };

            return Ok(groupViewModel);
        }
    }
}