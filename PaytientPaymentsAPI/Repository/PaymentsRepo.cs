using PaytientPaymentsAPI.Data;
using PaytientPaymentsAPI.Models;
using PaytientPaymentsAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaytientPaymentsAPI.Repository
{
    public class PaymentsRepo : IPaymentsRepo
    {
        private readonly PaytientPaymentsAPIDbContext _dbContext;

        public PaymentsRepo(PaytientPaymentsAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaymentsModel> Post(AddOneTimePaymentRequestModel addPaymentRequest)
        {
            var paymentsModel = new PaymentsModel()
            {
                Balance = addPaymentRequest.Balance,
                PaymentAmount = addPaymentRequest.PaymentAmount,
                PersonId = addPaymentRequest.PersonId
            };

            await _dbContext.Payments.AddAsync(paymentsModel);
            await _dbContext.SaveChangesAsync();

            return paymentsModel;

        }

        public async Task<PaymentsModel> PostBalance(AddCreateBalanceRequestModel addCreateBalanceRequest)
        {
            DateTime dueDate = DateTime.Now.AddDays(15);
            if (dueDate.DayOfWeek == DayOfWeek.Saturday)
            {
                dueDate = dueDate.AddDays(2);
            } else if (dueDate.DayOfWeek == DayOfWeek.Sunday)
            {
                dueDate = dueDate.AddDays(1);
            }

            var paymentsModel = new PaymentsModel()
            {
                PersonId = addCreateBalanceRequest.PersonId,
                Balance = addCreateBalanceRequest.Balance,
                ScheduleDate = dueDate
            };

            await _dbContext.Payments.AddAsync(paymentsModel);
            await _dbContext.SaveChangesAsync();

            return paymentsModel;

        }
    }
}
