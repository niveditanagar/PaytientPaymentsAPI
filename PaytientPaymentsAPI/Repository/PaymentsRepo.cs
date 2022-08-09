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
            var payment = _dbContext.Payments.Where(x => x.PersonId == addPaymentRequest.PersonId).OrderByDescending(x => x.ScheduleDate).FirstOrDefault();

            payment.PaymentAmount = addPaymentRequest.PaymentAmount;
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

        //public async Task<PaymentsModel> Post(AddOneTimePaymentRequestModel addPaymentRequest)
        //{
        //DateTime dueDate = DateTime.Now.AddDays(15);
        //    if (dueDate.DayOfWeek == DayOfWeek.Saturday)
        //    {
        //        dueDate = dueDate.AddDays(2);
        //    }
        //    else if (dueDate.DayOfWeek == DayOfWeek.Sunday)
        //    {
        //        dueDate = dueDate.AddDays(1);
        //    }

        //    var paymentsModel = new PaymentsModel()
        //    {
        //        Balance = addPaymentRequest.Balance,
        //        PaymentAmount = addPaymentRequest.PaymentAmount,
        //        PersonId = addPaymentRequest.PersonId,
        //        ScheduleDate = dueDate,
        //        PaymentDate = DateTime.Now
        //    };

        //    await _dbContext.Payments.AddAsync(paymentsModel);
        //    await _dbContext.SaveChangesAsync();

        //    return paymentsModel;

        //}

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
