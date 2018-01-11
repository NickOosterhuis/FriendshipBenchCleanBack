using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class AppointmentStatus
    {
        [Key]
        public int Id { get; set; }

        public string name { get; set; }
                
    }
}
