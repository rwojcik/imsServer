using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using IMSServer.Models;
using IMSServer.Repositories;
using IMSServer.ViewModels;
using Microsoft.AspNet.Identity;

namespace IMSServer.Controllers
{
    [Authorize]
    public class DeviceController : ApiController
    {
        private readonly DeviceRepository _deviceRepository;

        public DeviceController()
        {
            var userName = User?.Identity?.GetUserName() ?? "Anonymous";
            
            _deviceRepository = new DeviceRepository(userName);
        }

        // GET: api/Device
        public IEnumerable<DeviceViewModel> GetDeviceModels()
        {
            var devices = _deviceRepository.GetAll().Select(device => device.CreateDeviceViewModel());

            return devices;
        }

        // GET: api/Device/5
        [ResponseType(typeof(DeviceViewModel))]
        public async Task<IHttpActionResult> GetDeviceModel(long id)
        {
            var deviceModel = await _deviceRepository.FindAsync(id);
            if (deviceModel == null)
            {
                return NotFound();
            }

            var deviceVm = deviceModel.CreateDeviceViewModel();

            return Ok(deviceVm);
        }

        // PUT: api/Device/5
        // update
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDeviceModel(long id, UpdateDeviceViewModel updateDeviceViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != updateDeviceViewModel.DeviceId)
            {
                return BadRequest("Ids do not match!");
            }

            //_dbContext.Entry(updateDeviceViewModel).State = EntityState.Modified;

            var deviceModel = _deviceRepository.Get(id);

            if (deviceModel == null)
            {
                return NotFound();
            }

            deviceModel.Name = updateDeviceViewModel.Name;
            deviceModel.Description = updateDeviceViewModel.Description;
            deviceModel.GroupId = updateDeviceViewModel.GroupId;

            await _deviceRepository.UpdateAsync(deviceModel);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Device
        // add
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostDeviceModel(AddDeviceViewModel addDeviceViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingDevice = await _deviceRepository.GetFirstAsync(device => device.Name == addDeviceViewModel.Name);
            if (existingDevice != null)
                return BadRequest("Name is taken");

            var deviceModel = addDeviceViewModel.CreateDeviceModel();

            await _deviceRepository.AddAsync(deviceModel);

            return CreatedAtRoute("DefaultApi", new { id = deviceModel.Id }, deviceModel.CreateDeviceViewModel());
        }

        // DELETE: api/Device/5
        [ResponseType(typeof(DeviceViewModel))]
        public async Task<IHttpActionResult> DeleteDeviceModel(long id)
        {
            DeviceModel deviceModel = await _deviceRepository.RemoveAsync(id);
            if (deviceModel == null)
            {
                return NotFound();
            }

            return Ok(deviceModel.CreateDeviceViewModel());
        }
    }
}