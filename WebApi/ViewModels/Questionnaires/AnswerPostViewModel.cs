using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class AnswerPostViewModel
    {

        public string Answer { get; set; }

        public int Question_id { get; set; }

        public int Questionnaire_id { get; set; }

    }
}
