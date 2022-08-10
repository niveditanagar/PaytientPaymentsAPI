using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaytientPaymentsAPI.Data;
using PaytientPaymentsAPI.Models;
using PaytientPaymentsAPI.Repository.IRepository;
using PaytientPaymentsAPI.Services.IServices;

namespace PaytientPaymentsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }
        
        // POST: api/Payments/one-time-payment
        [HttpPost("one-time-payment")]
        public async Task<IActionResult> OneTimePayment([FromBody] AddOneTimePaymentRequestModel addPaymentRequest)
        {
            //return Ok(await _repo.Post(addPaymentRequest));
            return Ok();
        }

        //POST: api/payments/create-balance
        [HttpPost]
        [Route("create-balance")]
        public async Task<IActionResult> CreateBalance([FromBody] AddCreateBalanceRequestModel createBalanceRequest)
        {
            return Ok(await paymentService.CreateBalance(createBalanceRequest.PersonId, createBalanceRequest.Balance));
        }

        
        
    }
}
