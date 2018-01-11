using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels.Account
{
    public class EditUserViewModel
    {        
        public string StreetName { get; set; }

        
        public string HouseNumber { get; set; }

        
        public string Province { get; set; }

        
        public string District { get; set; }
    }
}
