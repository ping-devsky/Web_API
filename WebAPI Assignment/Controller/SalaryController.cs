using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Model;

namespace WebApplication1.Controller
{
    public class SalaryData
    {
        public List<SalaryModel> salarydata { get; set; }
    }
    public class SalaryController : ApiController
    {

        private readonly string jsonFile = @"C:\Users\aakash\OneDrive - Neal Analytics LLC\Desktop\Aakash_WebAPI\WebAPI Assignment\JsonData\Salary.json";


        // GET api/<controller>
        public List<SalaryModel> Get()
        {
            var json = File.ReadAllText(jsonFile);
            SalaryData jsonObject = JsonConvert.DeserializeObject<SalaryData>(json);
            return jsonObject.salarydata;
        }

        // GET api/<controller>/5
        public SalaryModel Get(int id)
        {
            var json = File.ReadAllText(jsonFile);
            var jsonObject = JsonConvert.DeserializeObject<SalaryData>(json);

            return jsonObject.salarydata.ToArray().FirstOrDefault(x => x.employee_id == id);
        }

        // POST api/<controller>
        public List<SalaryModel> Post([FromBody] JObject value)
        {
            var json = File.ReadAllText(jsonFile);
            var jsonObject = JObject.Parse(json);
            var newSalaryData = (JArray)jsonObject.GetValue("salarydata") as JArray;

            newSalaryData.Add(value);
            jsonObject["salarydata"] = newSalaryData;

            string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject,
                               Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(jsonFile, newJsonResult);
            return this.Get();
        }

        // PUT api/<controller>/5
        public SalaryData Put(int id, [FromBody] SalaryModel value)
        {
            var json = File.ReadAllText(jsonFile);
            var jsonObject = JsonConvert.DeserializeObject<SalaryData>(json);

            var oldSalaryData = jsonObject.salarydata.FirstOrDefault(x => x.employee_id == id);
            if (oldSalaryData != null)
            {
                oldSalaryData.basic_salary = value.basic_salary;
                oldSalaryData.hra = value.hra;
                oldSalaryData.pf = value.pf;

                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject,
                                   Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFile, newJsonResult);

            }

            return jsonObject;
        }

        // DELETE api/<controller>/5
        public List<SalaryModel> Delete(int id)
        {
            var json = File.ReadAllText(jsonFile);
            var jsonObject = JObject.Parse(json);
            var salaryData = (JArray)jsonObject.GetValue("salarydata") as JArray;

            var deleteSalaryData = salaryData.FirstOrDefault(x => x["employee_id"].Value<int>() == id);
            if (deleteSalaryData != null)
            {
                salaryData.Remove(deleteSalaryData);
                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject,
                              Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFile, newJsonResult);
            }
            return this.Get();
        }



        
    }
}