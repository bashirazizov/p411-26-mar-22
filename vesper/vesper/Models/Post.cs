using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace vesper.Models
{
    public class Post:BaseEntity
    {
        public string Img { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Subtitle { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
