using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileApp.ViewModels
{
    public class QuestionnaireGetViewModel
    {
        public DateTime Time { get; set; }

        public string Client_id { get; set; }

        public List<AnswerGetViewModel> Answers { get; set; }

        public bool Redflag { get; set; }

        public string DateTime
        {
            get { return String.Format("{0:dd/MM/yyyy, hh:mm tt}", Time); }
        }

        public string RedflagString
        {
            get { return Redflag ? "Yes" : "No"; }
        }
    }
}
