using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using IMSServer.ViewModels;

namespace IMSServer.Models
{
    public class GroupModel : ModelBase
    {
        [Required, Index("UniqueGroupName", IsUnique = true), StringLength(250)]
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<DeviceModel> Devices { get; set; }
    }

    public static class GroupModelOps
    {
        public static GroupViewModel CreateGroupViewModel(this GroupModel groupModel)
        {
            return new GroupViewModel
            {
                GroupId = groupModel.Id,
                CreatedAt = groupModel.CreatedAt,
                CreatedBy = groupModel.CreatedBy,
                Description = groupModel.Description,
                DevicesIds = groupModel.Devices?.Select(device => device.Id).ToList() ?? new List<long>(0),
                Name = groupModel.Name,
                UpdatedAt = groupModel.UpdatedAt,
                UpdatedBy = groupModel.CreatedBy,
            };
        }
    }
}
