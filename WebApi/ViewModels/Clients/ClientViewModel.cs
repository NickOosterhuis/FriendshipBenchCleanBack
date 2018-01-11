using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels.Clients
{
    public class ClientViewModel
    {
        public string id { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Birthday")]
        public DateTime BirthDay { get; set; }

        [Display(Name = "Street")]
        public string StreetName { get; set; }

        [Display(Name = "Housenumber")]
        public string HouseNumber { get; set; }

        [Display(Name = "Province")]
        public string Province { get; set; }

        [Display(Name = "District")]
        public string District { get; set; }
    }
}
