using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Model
{
    //Employee Model Class
    public class EmployeeModel
    {
        public int id { get; set; }
        public string employee_name { get; set; }
        public string department { get; set; }
        public int employee_age { get; set; }
    }
}