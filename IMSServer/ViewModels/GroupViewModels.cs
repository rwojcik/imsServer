using System;
using System.Collections.Generic;

namespace IMSServer.ViewModels
{
    public class GroupViewModel
    {
        public long BuildingId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public string CreatedBy { get; set; }
        
        public string UpdatedBy { get; set; }

        public ICollection<long> DevicesIds { get; set; }
    }
    
    public class AddGroupViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class UpdateGroupViewModel
    {
        public long GroupId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
