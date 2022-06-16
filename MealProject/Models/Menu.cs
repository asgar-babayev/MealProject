using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MealProject.Models
{
    public class Menu
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Descriprion { get; set; }
        [Required]
        public double Price { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
