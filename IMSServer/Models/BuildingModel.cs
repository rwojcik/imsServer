using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMSServer.Models
{
    public class BuildingModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long BuildingId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public string UpdatedBy { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        
        public virtual ICollection<DeviceModel> Devices { get; set; } 
    }
}
