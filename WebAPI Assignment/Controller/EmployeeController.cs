using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.IO;
using Newtonsoft.Json;
using WebApplication1.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace WebApplication1.Controller
{
    //The Link that you should put in URl
    // For fetching all employees  https://localhost:44350/api/Employee/
    //For fetching employee by id https://localhost:44350/api/Employee/Get/1

    public class EmployeeData
    {
        public List<EmployeeModel> employeedata { get; set; }
    }
    public class EmployeeController : ApiController
    {
        private string jsonFile = @"C:\Users\aakash\OneDrive - Neal Analytics LLC\Desktop\Aakash_WebAPI\WebAPI Assignment\JsonData\Employee.json";

        public JObject GetMethod1()
        {
            var json = File.ReadAllText(jsonFile);
            object jsonObject = JsonConvert.DeserializeObject(json);
            
            return (JObject)jsonObject;
        }

        [HttpGet] //since we have a different function name we need to give it a tag
        public void Employee()
        {
            var json = File.ReadAllText(jsonFile);
            var jsonObject = JsonConvert.DeserializeObject<EmployeeData>(json);

            //    return View(jsonObject);
        }

        // GET api/<controller>
        public List<EmployeeModel> Get()
        {
            var json = File.ReadAllText(jsonFile);
            EmployeeData jsonObject = JsonConvert.DeserializeObject<EmployeeData>(json);
            return jsonObject.employeedata;
        }       
         
        // GET api/<controller>/5
        public EmployeeModel Get(int id)
        {
            var json = File.ReadAllText(jsonFile);
            var jsonObject = JsonConvert.DeserializeObject<EmployeeData>(json);
           
            return jsonObject.employeedata.ToArray().FirstOrDefault(x => x.id == id);
        }

        
        // POST api/<controller>
        public List<EmployeeModel> Post([FromBody] JObject value)
        {
            var json = File.ReadAllText(jsonFile);
            //var jsonObject = JsonConvert.DeserializeObject<EmployeeData>(json);
            //jsonObject.employeedata.ToArray().Append(value);
            var jsonObject = JObject.Parse(json);
            var newEmpData = jsonObject.GetValue("employeedata") as JArray;

            value["id"] = newEmpData.LongCount() + 1; //longcount return the  total record in the table,since our id starts from 1 we are doing +1

            newEmpData.Add(value);
            jsonObject["employeedata"] = newEmpData;
            
            string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject,
                               Newtonsoft.Json.Formatting.Indented);        
            File.WriteAllText(jsonFile, newJsonResult);
            return this.Get();
        }

        // PUT api/<controller>/5
        public EmployeeData Put(int id, [FromBody] EmployeeModel value)
        {
            var json = File.ReadAllText(jsonFile);
            var jsonObject = JsonConvert.DeserializeObject<EmployeeData>(json);
     
            var oldEmpData=jsonObject.employeedata.FirstOrDefault(x => x.id == id);
            if(oldEmpData!=null)
            {
                oldEmpData.employee_name = value.employee_name;
                oldEmpData.employee_age = value.employee_age;
                oldEmpData.department = value.department;


                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject,
                                   Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFile, newJsonResult);

            }

            return jsonObject;
        }

        // DELETE api/<controller>/5
        public List<EmployeeModel> Delete(int id)
        {
            var json = File.ReadAllText(jsonFile);
            //var jsonObject = JsonConvert.DeserializeObject<EmployeeData>(json);
            var jsonObject = JObject.Parse(json);
            var empData = (JArray)jsonObject.GetValue("employeedata") as JArray;

            var deleteEmpData = empData.FirstOrDefault(x => x["id"].Value<int>() == id);
            if(deleteEmpData != null)
            {
                empData.Remove(deleteEmpData);
                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject,
                              Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFile, newJsonResult);
            }
            return this.Get();
           
        }

        //Fetch all the details of an employee(including leave and salary details) via employee id.
        //Get Method
        [HttpGet]
        public EmployeeDetails getEmpDetails(int id)
        {

            string csvfile = @"C:\Users\aakash\OneDrive - Neal Analytics LLC\Desktop\Aakash_WebAPI\WebAPI Assignment\CSV Files\Getdata_Excel.csv";
            string leavejsonFile = @"C:\Users\aakash\OneDrive - Neal Analytics LLC\Desktop\Aakash_WebAPI\WebAPI Assignment\JsonData\Leave.json";
            string salaryjsonFile = @"C:\Users\aakash\OneDrive - Neal Analytics LLC\Desktop\Aakash_WebAPI\WebAPI Assignment\JsonData\Salary.json";

            var json = File.ReadAllText(jsonFile);
            var leavejson = File.ReadAllText(leavejsonFile);
            var salaryjson = File.ReadAllText(salaryjsonFile);

            EmployeeDetails finalarr = new EmployeeDetails();

            //getting employee details
            var empjsonObject = JsonConvert.DeserializeObject<EmployeeData>(json);
            var emparr= empjsonObject.employeedata.ToArray().FirstOrDefault(x => x.id == id);
            finalarr.id = emparr.id;
            finalarr.employee_name = emparr.employee_name;
            finalarr.employee_age = emparr.employee_age;
            finalarr.department = emparr.department;
            //getting leave details of that employee
            var leavejsonObject = JsonConvert.DeserializeObject<LeaveData>(leavejson);
            var leavearr = leavejsonObject.leavedata.ToArray().FirstOrDefault(x => x.emp_id == id);
            finalarr.taken_leaves = leavearr.taken_leaves;
            finalarr.total_leaves = leavearr.total_leaves;
            //getting salary details of that employee
            var salaryjsonObject = JsonConvert.DeserializeObject<SalaryData>(salaryjson);
            var salaryarr = salaryjsonObject.salarydata.ToArray().FirstOrDefault(x => x.employee_id == id);
            finalarr.hra = salaryarr.hra;
            finalarr.basic_salary = salaryarr.basic_salary;
            finalarr.pf = salaryarr.pf;

            using(System.IO.StreamWriter file = new System.IO.StreamWriter(csvfile,true))
            {
                file.WriteLine("ID :: "+finalarr.id+" , Employee Name :: "+finalarr.employee_name+" , Employee Age :: "+finalarr.employee_age+" , Department :: "+finalarr.department
                        +" , Take Leaves :: "+finalarr.taken_leaves+" , Total Leaves :: "+finalarr.total_leaves+" , HRA :: "+finalarr.hra+" , Basic Salary :: "+finalarr.basic_salary+" , PF :: "+finalarr.pf);
            }

            return finalarr;

        }
        //Post Method
        [HttpPost]
        public EmployeeDetails getEmployeeDetailsbyID([FromBody] int id)
        {
            string csvfile = @"C:\Users\aakash\OneDrive - Neal Analytics LLC\Desktop\Aakash_WebAPI\WebAPI Assignment\CSV Files\Postdata_Excel.csv";
            string leavejsonFile = @"C:\Users\aakash\OneDrive - Neal Analytics LLC\Desktop\Aakash_WebAPI\WebAPI Assignment\JsonData\Leave.json";
            string salaryjsonFile = @"C:\Users\aakash\OneDrive - Neal Analytics LLC\Desktop\Aakash_WebAPI\WebAPI Assignment\JsonData\Salary.json";


            var json = File.ReadAllText(jsonFile);
            var leavejson = File.ReadAllText(leavejsonFile);
            var salaryjson = File.ReadAllText(salaryjsonFile);

            EmployeeDetails finalarr = new EmployeeDetails();

            //getting employee details
            var empjsonObject = JsonConvert.DeserializeObject<EmployeeData>(json);
            var emparr = empjsonObject.employeedata.ToArray().FirstOrDefault(x => x.id == id);
            finalarr.id = emparr.id;
            finalarr.employee_name = emparr.employee_name;
            finalarr.employee_age = emparr.employee_age;
            finalarr.department = emparr.department;
            //getting leave details of that employee
            var leavejsonObject = JsonConvert.DeserializeObject<LeaveData>(leavejson);
            var leavearr = leavejsonObject.leavedata.ToArray().FirstOrDefault(x => x.emp_id == id);
            finalarr.taken_leaves = leavearr.taken_leaves;
            finalarr.total_leaves = leavearr.total_leaves;
            //getting salary details of that employee
            var salaryjsonObject = JsonConvert.DeserializeObject<SalaryData>(salaryjson);
            var salaryarr = salaryjsonObject.salarydata.ToArray().FirstOrDefault(x => x.employee_id == id);
            finalarr.hra = salaryarr.hra;
            finalarr.basic_salary = salaryarr.basic_salary;
            finalarr.pf = salaryarr.pf;

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(csvfile, true))
            {
                file.WriteLine("ID :: " + finalarr.id + " , Employee Name :: " + finalarr.employee_name + " , Employee Age :: " + finalarr.employee_age + " , Department :: " + finalarr.department
                        + " , Take Leaves :: " + finalarr.taken_leaves + " , Total Leaves :: " + finalarr.total_leaves + " , HRA :: " + finalarr.hra + " , Basic Salary :: " + finalarr.basic_salary + " , PF :: " + finalarr.pf);
            }

            return finalarr;
        }

        //Finding max salary of employee by Department using LINQ
        [HttpGet]
        public IEnumerable maxSalary()
        {
            string salaryjsonFile = @"C:\Users\aakash\OneDrive - Neal Analytics LLC\Desktop\Aakash_WebAPI\WebAPI Assignment\JsonData\Salary.json";
            var salaryjson = File.ReadAllText(salaryjsonFile);
            
            //getting salary details of that employee
            var salaryjsonObject = JsonConvert.DeserializeObject<SalaryData>(salaryjson);
            var salarr = salaryjsonObject.salarydata;

            var depMaxSalary = new JObject();

            var res = from emp in this.Get()
                      join sal in salarr on emp.id equals sal.employee_id group new {emp,sal} by emp.department into grp
                      select new
                      {
                          Dep= grp.FirstOrDefault().emp.department,
                          Salary = grp.Max(g=>(g.sal.basic_salary+g.sal.hra-g.sal.pf))
                      };

            return res;
          
        }
    }

}