using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Bench
    {
        [Key]
        public int Id { get; set; }

        public string Streetname { get; set; }

        public string Housenumber { get; set; }

        public string Province { get; set; }

        public string District { get; set; }
                
    }
}
