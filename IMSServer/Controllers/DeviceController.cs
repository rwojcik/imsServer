using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using IMSServer.Models;
using IMSServer.ViewModels;

namespace IMSServer.Controllers
{
    public class DeviceController : ApiController
    {
        private readonly IMSServerContext _dbContext = new IMSServerContext();

        // GET: api/Device
        public IQueryable<DeviceModel> GetDeviceModels()
        {
            return _dbContext.DeviceModels;
        }

        // GET: api/Device/5
        [ResponseType(typeof(DeviceViewModel))]
        public async Task<IHttpActionResult> GetDeviceModel(long id)
        {
            DeviceModel deviceModel = await _dbContext.DeviceModels.FindAsync(id);
            if (deviceModel == null)
            {
                return NotFound();
            }

            var deviceVm = new DeviceViewModel
            {
                
            };

            return Ok(deviceVm);
        }

        // PUT: api/Device/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDeviceModel(long id, DeviceModel deviceModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != deviceModel.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(deviceModel).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Device
        [ResponseType(typeof(DeviceModel))]
        public async Task<IHttpActionResult> PostDeviceModel(DeviceModel deviceModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbContext.DeviceModels.Add(deviceModel);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DeviceModelExists(deviceModel.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = deviceModel.Id }, deviceModel);
        }

        // DELETE: api/Device/5
        [ResponseType(typeof(DeviceModel))]
        public async Task<IHttpActionResult> DeleteDeviceModel(long id)
        {
            DeviceModel deviceModel = await _dbContext.DeviceModels.FindAsync(id);
            if (deviceModel == null)
            {
                return NotFound();
            }

            _dbContext.DeviceModels.Remove(deviceModel);
            await _dbContext.SaveChangesAsync();

            return Ok(deviceModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DeviceModelExists(long id)
        {
            return _dbContext.DeviceModels.Count(e => e.Id == id) > 0;
        }
    }
}