using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Models
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public int StatusId { get; set; }

        public int BenchId { get; set; }

        public string ClientId { get; set; }

        public string HealthworkerId { get; set; }
    }
}
