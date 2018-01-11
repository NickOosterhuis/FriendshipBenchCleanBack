using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public int StatusId { get; set; }

        public int BenchId { get; set; }

        public string ClientId { get; set; }

        public string HealthworkerId { get; set; }
    }
}
