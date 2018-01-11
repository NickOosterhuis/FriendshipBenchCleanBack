using System;

namespace WebApi.ViewModels.Appointments
{
    public class AppointmentPostViewModel
    {

        public DateTime Time { get; set; }

        public int BenchId { get; set; }

        public string ClientId { get; set; }

        public string HealthworkerId { get; set; }
    }
}