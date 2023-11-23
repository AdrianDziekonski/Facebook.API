using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Facebook.API.Data;
using Facebook.API.Models;

namespace Facebook.API.Controllers
{   //http://localhost:5000/api/values
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext contextRO;
        public ValuesController(DataContext context)
        {
            contextRO = context;
            
        }
        // GET api/values
        [HttpGet]
        public IActionResult GetValuesAll()
        {
            var values=contextRO.Values.ToList();
            return Ok(values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult GetValue(int id)
        {
           var value=contextRO.Values.FirstOrDefault(x=>x.Id==id);
           return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public IActionResult AddValue([FromBody] Value value)
        {
            contextRO.Values.Add(value);
            contextRO.SaveChanges();
            return Ok(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult EditValue(int id, [FromBody] Value value)
        {
            var data=contextRO.Values.Find(id);
            data.Name=value.Name;
            contextRO.Values.Update(data);
            contextRO.SaveChanges();
            return Ok(data);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult DeleteValue(int id)
        {
            var data =contextRO.Values.Find(id);

            if(data==null)
            return NoContent();
            
            contextRO.Values.Remove(data);
            contextRO.SaveChanges();
            return Ok(data);
        }
    }
}
