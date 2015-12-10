using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using IMSServer.Models;
using IMSServer.Repositories;
using IMSServer.ViewModels;
using Microsoft.AspNet.Identity;

namespace IMSServer.Controllers
{
    [Authorize]
    public class GroupController : ApiController
    {
        private readonly GroupRepository _repository;

        public GroupController()
        {

            var userName = User?.Identity?.Name ?? "Anonymous";

            _repository = new GroupRepository(userName);
        }

        // GET: api/Group
        public IEnumerable<GroupViewModel> GetGroupModels()
        {
            return _repository.GetAll().ToList().Select(groupModel => groupModel.CreateGroupViewModel());
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

            return Ok(groupModel.CreateGroupViewModel());
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

            var groupModel = await _repository.FindAsync(id);

            if (groupModel == null)
            {
                return NotFound();
            }

            //jeżeli zmienia sięnazwę to trzeba sprawdzić czy nowa nie jest zajęta
            if (groupVm.Name != groupModel.Name) 
            {
                var checkName = await _repository.GetFirstAsync(dev => dev.Name == groupVm.Name);
                if(checkName != null)
                    return BadRequest("Name is taken");
            }
            groupModel.Name = groupVm.Name;
            groupModel.Description = groupVm.Description;

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
        [ResponseType(typeof(GroupViewModel))]
        public async Task<IHttpActionResult> PostGroupModel(AddGroupViewModel groupVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingGroup = await _repository.GetFirstAsync(gr => gr.Name == groupVm.Name);

            if (existingGroup != null)
            {
                return BadRequest("Name is taken");
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

            return CreatedAtRoute("DefaultApi", new { id = groupModel.Id }, groupModel.CreateGroupViewModel());
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
            
            return Ok(groupModel.CreateGroupViewModel());
        }
    }
}