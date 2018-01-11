using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class ClientUser : User
    {
        public string StreetName { get; set; }

        public string HouseNumber { get; set; }

        public string Province { get; set; }

        public string District { get; set; }

        public string HealthWorker_Id { get; set; }
    }
}
