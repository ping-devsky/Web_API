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
    public class LeaveData
    {
        public List<LeaveModel> leavedata { get; set; }
    }
    public class LeaveController : ApiController
    {

        private string jsonFile = @"C:\Users\aakash\OneDrive - Neal Analytics LLC\Desktop\Aakash_WebAPI\WebAPI Assignment\JsonData\Leave.json";

        // GET api/<controller>
        public List<LeaveModel> Get()
        {
            var json = File.ReadAllText(jsonFile);
            LeaveData jsonObject = JsonConvert.DeserializeObject<LeaveData>(json);
            return jsonObject.leavedata;
        }

        // GET api/<controller>/5
        public LeaveModel Get(int id)
        {
            var json = File.ReadAllText(jsonFile);
            var jsonObject = JsonConvert.DeserializeObject<LeaveData>(json);

            return jsonObject.leavedata.ToArray().FirstOrDefault(x => x.emp_id == id);
        }

        // GET Pending Leave api/<controller>/5
        [HttpGet]
        public string GetTakenPendingLeave(int id)
        {
            var json = File.ReadAllText(jsonFile);
            var jsonObject = JsonConvert.DeserializeObject<LeaveData>(json);
            int taken= Convert.ToInt32(jsonObject.leavedata.ToArray().FirstOrDefault(x => x.emp_id == id).taken_leaves);
            int total= Convert.ToInt32(jsonObject.leavedata.ToArray().FirstOrDefault(x => x.emp_id == id).total_leaves);

            return "Total Pending Leave is : " + (total - taken) +"\n Total Taken Leave is : "+ taken;
        }

        // GET Pending Leave api/<controller>/5
        public string GetPendingLeave(int id)
        {
            var json = File.ReadAllText(jsonFile);
            var jsonObject = JsonConvert.DeserializeObject<LeaveData>(json);
            int taken = Convert.ToInt32(jsonObject.leavedata.ToArray().FirstOrDefault(x => x.emp_id == id).taken_leaves);
            int total = Convert.ToInt32(jsonObject.leavedata.ToArray().FirstOrDefault(x => x.emp_id == id).total_leaves);

            return "Total Pending Leave is : " + (total - taken);
        }

        // GET Taken Leave api/<controller>/5
        [HttpGet]
        public string GetTakenLeave(int id)
        {
            var json = File.ReadAllText(jsonFile);
            var jsonObject = JsonConvert.DeserializeObject<LeaveData>(json);

            return "Total taken leave is : "+jsonObject.leavedata.ToArray().FirstOrDefault(x => x.emp_id == id).taken_leaves;
        }

        // POST api/<controller>
        public List<LeaveModel> Post([FromBody] JObject value)
        {
            var json = File.ReadAllText(jsonFile);
            var jsonObject = JObject.Parse(json);
            var newLeaveData = (JArray)jsonObject.GetValue("leavedata") as JArray;

            newLeaveData.Add(value);
            jsonObject["leavedata"] = newLeaveData;
            
            string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject,
                               Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(jsonFile, newJsonResult);
            return this.Get();
        }

        // PUT api/<controller>/5
        public LeaveData Put(int id, [FromBody] LeaveModel value)
        {
            var json = File.ReadAllText(jsonFile);
            var jsonObject = JsonConvert.DeserializeObject<LeaveData>(json);

            var oldLeaveData = jsonObject.leavedata.FirstOrDefault(x => x.emp_id == id);
            if (oldLeaveData != null)
            {
                oldLeaveData.total_leaves = value.total_leaves;
                oldLeaveData.taken_leaves = value.taken_leaves;


                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject,
                                   Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFile, newJsonResult);

            }

            return jsonObject;
        }

        // DELETE api/<controller>/5
        public List<LeaveModel> Delete(int id)
        {
            var json = File.ReadAllText(jsonFile);
            var jsonObject = JObject.Parse(json);
            var leaveData = (JArray)jsonObject.GetValue("leavedata") as JArray;

            var deleteLeaveData = leaveData.FirstOrDefault(x => x["emp_id"].Value<int>() == id);
            if (deleteLeaveData != null)
            {
                leaveData.Remove(deleteLeaveData);
                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject,
                              Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFile, newJsonResult);
            }
            return this.Get();
        }
    }
}