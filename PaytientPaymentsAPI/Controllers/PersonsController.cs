using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaytientPaymentsAPI.Data;
using PaytientPaymentsAPI.Models;
using PaytientPaymentsAPI.Repository.IRepository;

namespace PaytientPaymentsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonsRepo _repo;

        public PersonsController(IPersonsRepo repo)
        {
            _repo = repo;
        }

        // GET: api/Persons
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST: api/Persons
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddPersonRequestModel addPersonRequest)
        {
            return Ok(await _repo.Post(addPersonRequest));
        }

        // PUT: api/Persons/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
        
    }
}
