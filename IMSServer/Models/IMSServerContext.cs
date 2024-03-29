﻿using System.Data.Entity;

namespace IMSServer.Models
{
    public class IMSServerContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public IMSServerContext() : base("name=IMSServerContext")
        {
            Database.SetInitializer<IMSServerContext>(new DropCreateDatabaseIfModelChanges<IMSServerContext>());
        }

        public DbSet<GroupModel> GroupModels { get; set; }

        public DbSet<DeviceModel> DeviceModels { get; set; }
    }
}
