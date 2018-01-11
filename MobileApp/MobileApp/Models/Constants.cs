using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileApp.Models
{
    public class Constants
    {

        public static bool isDev = true;

        public static string loginUrl = "http://10.0.2.2:54618/api/account/signin";
        public static string appointmentsUrl = "http://10.0.2.2:54618/api/appointments";
        public static string registerUrl = "http://10.0.2.2:54618/api/account/register/client";
        public static string questionsUrl = "http://10.0.2.2:54618/api/questions";
        public static string questionnaireUrl = "http://10.0.2.2:54618/api/questionnaires";
        public static string answerUrl = "http://10.0.2.2:54618/api/answers";
        public static string healthWorkerUrl = "http://10.0.2.2:54618/api/healthworkers";
        public static string tokenUrl = "http://10.0.2.2:54618/api/account/generatetoken";
        public static string getCurrentUserUrl = "http://10.0.2.2:54618/api/account/currentUser";
        public static string editClientUrl = "http://10.0.2.2:54618/api/Account/edit";
        public static string addHealthworkerToClientUrl = "http://10.0.2.2:54618/api/Account/addHealthworker";

    }
}
