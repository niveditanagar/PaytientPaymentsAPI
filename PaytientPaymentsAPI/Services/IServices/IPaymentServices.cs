using PaytientPaymentsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaytientPaymentsAPI.Services.IServices
{
    public interface IPaymentService
    {
        Task<PaymentsModel> CreateBalance(int personId, decimal balance);
    }
}
