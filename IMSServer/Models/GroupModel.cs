using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMSServer.Models
{
    public class GroupModel : ModelBase
    {
        [Required, Index("UniqueGroupName", IsUnique = true), StringLength(250)]
        public string Name { get; set; }

        public string Description { get; set; }
        
        public virtual ICollection<DeviceModel> Devices { get; set; } 
    }
}
