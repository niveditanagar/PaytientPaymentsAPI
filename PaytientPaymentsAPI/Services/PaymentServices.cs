using PaytientPaymentsAPI.Models;
using PaytientPaymentsAPI.Repository.IRepository;
using PaytientPaymentsAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaytientPaymentsAPI.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentsRepo paymentRepo;

        public PaymentService(IPaymentsRepo paymentRepo)
        {
            this.paymentRepo = paymentRepo;
        }

        public async Task<PaymentsModel> CreateBalance(int personId, decimal balance)
        {
            DateTime dueDate = DateTime.Now.AddDays(15);
            if (dueDate.DayOfWeek == DayOfWeek.Saturday)
            {
                dueDate = dueDate.AddDays(2);
            }
            else if (dueDate.DayOfWeek == DayOfWeek.Sunday)
            {
                dueDate = dueDate.AddDays(1);
            }

            var paymentsModel = new PaymentsModel()
            {
                PersonId = personId,
                Balance = balance,
                ScheduleDate = dueDate
            };

            await paymentRepo.AddPaymentAsync(paymentsModel);

            return paymentsModel;
        }
    }
}
