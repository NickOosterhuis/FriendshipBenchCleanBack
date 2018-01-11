using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileApp.Models
{
    public class Healthworker
    {
        public string Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Gender { get; set; }

        public DateTime Birthday { get; set; }

        public string Email { get; set; }

        public string Phonenumber { get; set; }

        public string Name
        {
            get { return string.Format("{0} {1}", Firstname, Lastname); }
        }

        public string DayOfBirth
        {
            get { return String.Format("{0:dd/MM/yyyy}", Birthday); }
        }

        public string GenderDisplayString
        {
            get {
                return char.ToUpper(Gender[0]) + Gender.Substring(1);
            }
        }
    }
}
