using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.ViewModels.Clients;

namespace WebApi.ViewModels.Questionnaires
{
    public class QuestionnaireWithAnswersViewModel
    {
        public DateTime Time { get; set; }

        public ClientViewModel Client { get; set; }

        public bool Redflag { get; set; }

        public List<AnswerGetViewModel> Answers { get; set; }
    }
}
