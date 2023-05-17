﻿using System.ComponentModel.DataAnnotations;

namespace task1.Models
{
    public  abstract class BaseEntity
    {        
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }    
        public DateTime UpdatedAt { get; set;}
        public bool IsDeleted { get; set; }
    }
}
