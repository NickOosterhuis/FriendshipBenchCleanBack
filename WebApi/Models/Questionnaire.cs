using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Questionnaire
    {
        [Key]
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public string Client_id { get; set; }

        public bool Redflag { get; set; }
    }
}
