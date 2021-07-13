using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Model
{
     //salary Model Class
    public class SalaryModel
    {
        public int employee_id { get; set; }
        public int basic_salary { get; set; }
        public int hra { get; set; }
        public int pf { get; set; }

    }
}