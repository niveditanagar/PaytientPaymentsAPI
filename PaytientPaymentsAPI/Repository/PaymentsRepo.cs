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
            //calculating percentage for match:
            decimal matchPercentage = 0;
            if(addPaymentRequest.PaymentAmount < 10)
            {
                matchPercentage = (addPaymentRequest.PaymentAmount / 100) * 1;
            } else if(addPaymentRequest.PaymentAmount < 50)
            {
                matchPercentage = (addPaymentRequest.PaymentAmount / 100) * 3;
            } else
            {
                matchPercentage = (addPaymentRequest.PaymentAmount / 100) * 5;
            }


            var payment = _dbContext.Payments.Where(x => x.PersonId == addPaymentRequest.PersonId).OrderByDescending(x => x.ScheduleDate).FirstOrDefault();

            payment.PaymentAmount = addPaymentRequest.PaymentAmount + matchPercentage;
            payment.PaymentDate = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            DateTime dueDate = payment.ScheduleDate.AddDays(15);
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
                Balance = (payment.Balance - payment.PaymentAmount),
                PersonId = addPaymentRequest.PersonId,
                ScheduleDate = dueDate,
            };

            await _dbContext.Payments.AddAsync(paymentsModel);
            await _dbContext.SaveChangesAsync();

            return paymentsModel;

        }

        public async Task<PaymentsModel> AddPaymentAsync(PaymentsModel paymentsModel)
        {
            await _dbContext.Payments.AddAsync(paymentsModel);
            await _dbContext.SaveChangesAsync();

            return paymentsModel;
        }
    }
}
