using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Facebook.API.Data;
using Facebook.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Facebook.API.Controllers
{   //http://localhost:5000/api/values
    [Authorize]   //dodajemy autoryzacje tutaj nie w auth controller bo tamte metody potrzebne są wszytskim a te tylko po zalogowaniu, dotyczy kazdej metody 
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
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetValuesAll()
        {
            var values = await contextRO.Values.ToListAsync();
            return Ok(values);
        }

        // GET api/values/5
       [AllowAnonymous]  //do testu pozwala na dostęp bez autoryzacji
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            var value = await contextRO.Values.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> AddValue([FromBody] Value value)
        {
            contextRO.Values.Add(value);
            await contextRO.SaveChangesAsync();
            return Ok(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditValue(int id, [FromBody] Value value)
        {
            var data = await contextRO.Values.FindAsync(id);
            data.Name = value.Name;
            contextRO.Values.Update(data);
            await contextRO.SaveChangesAsync();
            return Ok(data);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteValue(int id)
        {
            var data = await contextRO.Values.FindAsync(id);

            if (data == null)
                return NoContent();

            contextRO.Values.Remove(data);
            await contextRO.SaveChangesAsync();
            return Ok(data);
        }
    }
}

