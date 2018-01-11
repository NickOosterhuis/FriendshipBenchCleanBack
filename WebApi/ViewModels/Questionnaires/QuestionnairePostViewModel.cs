using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels.Questionnaires
{
    public class QuestionnairePostViewModel
    {
        public string Client_id { get; set; }

        public DateTime Time { get; set; }

        public bool Redflag { get; set; }

    }
}
