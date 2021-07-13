using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Model
{

    //Leave Model Class
    public class LeaveModel
    {
        public int emp_id { get; set; }
        public int total_leaves { get; set; }
        public int taken_leaves { get; set; }
    }
}