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
        
        // POST: api/Persons
        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody] AddPersonRequest addPersonRequest)
        {
            return Ok(await _repo.CreatePerson(addPersonRequest.LastName, addPersonRequest.FirstName));
        }
        
        
    }
}
