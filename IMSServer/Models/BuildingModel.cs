using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMSServer.Models
{
    public class BuildingModel : ModelBase
    {
        [Required, Index("UniqueBuildingName", IsUnique = true)]
        public string Name { get; set; }

        public string Description { get; set; }
        
        public virtual ICollection<DeviceModel> Devices { get; set; } 
    }
}
