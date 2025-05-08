using Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO
{
    public class CarDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Brand { get; set; } = String.Empty;
        public int Year { get; set; }
    }
}
