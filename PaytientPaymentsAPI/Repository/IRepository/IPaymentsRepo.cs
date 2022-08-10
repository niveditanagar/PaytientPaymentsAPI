using PaytientPaymentsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaytientPaymentsAPI.Repository.IRepository
{
    public interface IPaymentsRepo
    {
        Task<PaymentsModel> AddPaymentAsync(PaymentsModel paymentsModel);

        Task<PaymentsModel> GetLatestPaymentAsync(int personId);
    }
}
