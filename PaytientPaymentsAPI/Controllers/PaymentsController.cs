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
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentsRepo _repo;

        public PaymentsController(IPaymentsRepo repo)
        {
            _repo = repo;
        }
        
        // POST: api/Payments/one-time-payment
        [HttpPost("one-time-payment")]
        public async Task<IActionResult> Post([FromBody] AddOneTimePaymentRequestModel addPaymentRequest)
        {
            return Ok(await _repo.Post(addPaymentRequest));
        }

        //POST: api/payments/create-balance
        [HttpPost]
        [Route("create-balance")]
        public async Task<IActionResult> PostBalance([FromBody] AddCreateBalanceRequestModel createBalanceRequest)
        {
            return Ok(await _repo.PostBalance(createBalanceRequest));
        }

        
        
    }
}
