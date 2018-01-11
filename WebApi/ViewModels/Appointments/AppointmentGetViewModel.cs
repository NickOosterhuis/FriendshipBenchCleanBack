using System;
using WebApi.Models;
using WebApi.ViewModels.Clients;
using WebApi.ViewModels.HealthWorkers;

namespace WebApi.ViewModels.Appointments
{
    public class AppointmentGetViewModel
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public AppointmentStatus Status { get; set; }

        public Bench Bench { get; set; }

        public ClientViewModel Client { get; set; }

        public HealthWorkerViewModel Healthworker { get; set; }
    }
}