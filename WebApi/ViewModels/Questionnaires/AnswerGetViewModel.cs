using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels.Questionnaires
{
    public class AnswerGetViewModel
    {
        public int QuestionId { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }
    }
}
