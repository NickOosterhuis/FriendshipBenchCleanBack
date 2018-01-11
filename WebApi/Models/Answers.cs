using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Answers
    {
        [Key]
        public int Id { get; set; }

        public string Answer { get; set; }

        public int Question_id { get; set; }

        public int Questionnaire_id { get; set; }

    }
}
