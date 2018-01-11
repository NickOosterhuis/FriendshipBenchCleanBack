using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.ViewModels
{
    class QuestionnaireViewModel
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

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
