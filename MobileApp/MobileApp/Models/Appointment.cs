using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public AppointmentStatus Status { get; set; }

        public Bench Bench { get; set; }

        public Client Client { get; set; }

        public Healthworker Healthworker { get; set; }

        public string DateTime
        {
            get { return String.Format("{0:dd/MM/yyyy, hh:mm tt}", Time); }
        }
    }
}
