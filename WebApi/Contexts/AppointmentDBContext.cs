using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Contexts
{
    public class AppointmentDBContext : DbContext
    {
        public AppointmentDBContext(DbContextOptions<AppointmentDBContext> options) : base(options) { }

        public virtual DbSet<Bench> Benches { get; set; }

        public virtual DbSet<Appointment> Appointments { get; set; }

        public virtual DbSet<AppointmentStatus> AppointmentStatuses { get; set; }


    }
}
