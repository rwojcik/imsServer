﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMSServer.Models
{
    public abstract class ModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string CreatedBy { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public string UpdatedBy { get; set; }
    }

    public static class ModelOps
    {
        public static void AuditEntity(this ModelBase model, string userName)
        {
            if (string.IsNullOrEmpty(model.CreatedBy))
            {
                model.CreatedBy = userName;
                model.CreatedAt = DateTime.Now;
            }

            model.UpdatedBy = userName;
            model.UpdatedAt = DateTime.Now;
        }
    }
}
