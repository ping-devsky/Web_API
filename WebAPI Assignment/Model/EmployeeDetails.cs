using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Model
{
    public class EmployeeDetails
    {
        public int id { get; set; }
        public string employee_name { get; set; }
        public string department { get; set; }
        public int employee_age { get; set; }
        public int total_leaves { get; set; }
        public int taken_leaves { get; set; }
        public int basic_salary { get; set; }
        public int hra { get; set; }
        public int pf { get; set; }


    }
}