using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Models
{
    public class Bench
    {
        public int Id { get; set; }

        public string Streetname { get; set; }

        public string Housenumber { get; set; }

        public string Province { get; set; }

        public string District { get; set; }

        public string Location
        {
            get { return Streetname + " " + Housenumber + ", " + District + ", " + Province; }
        }
    }
}
