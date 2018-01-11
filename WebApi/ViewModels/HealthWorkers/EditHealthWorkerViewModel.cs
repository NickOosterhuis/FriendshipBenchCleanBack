using System;

namespace WebApi.ViewModels.HealthWorkers
{
    public class EditHealthWorkerViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        public DateTime BirthDay { get; set; }
    }
}