using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Model;

namespace WebApplication1.View
{
    public class EmployeeData
    {
        public IList<EmployeeModel> empdata { get; set; }
    }
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get_Employee_Details().wait();
        }

    }
}