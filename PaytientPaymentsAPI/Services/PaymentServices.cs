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
            DateTime dueDate = GetScheduleDate(DateTime.Now);

            var paymentsModel = new PaymentsModel()
            {
                PersonId = personId,
                Balance = balance,
                ScheduleDate = dueDate
            };

            await paymentRepo.AddPaymentAsync(paymentsModel);

            return paymentsModel;
        }

        public async Task<PaymentsModel> PostPayment(decimal paymentAmount, int personId)
        {
            decimal matchAmount = GetMatchAmount(paymentAmount);

            var payment = await paymentRepo.GetLatestPaymentAsync(personId);
            payment.PaymentAmount = paymentAmount + matchAmount;
            payment.PaymentDate = DateTime.Now;
           
            DateTime dueDate = GetScheduleDate(payment.ScheduleDate);
            
            var newBalance = new PaymentsModel()
            {
                Balance = (payment.Balance - payment.PaymentAmount),
                PersonId = personId,
                ScheduleDate = dueDate,
            };

            await paymentRepo.AddPaymentAsync(newBalance);

            return newBalance;
        }

        private DateTime GetScheduleDate(DateTime date)
        {
            DateTime dueDate = date.AddDays(15);
            if (dueDate.DayOfWeek == DayOfWeek.Saturday)
            {
                dueDate = dueDate.AddDays(2);
            }
            else if (dueDate.DayOfWeek == DayOfWeek.Sunday)
            {
                dueDate = dueDate.AddDays(1);
            }

            return dueDate;
        }

        //calculating percentage for match
        private decimal GetMatchAmount(decimal paymentAmount)
        {
            decimal matchPercentage = 0;
            if (paymentAmount < 10)
            {
                matchPercentage = (paymentAmount / 100) * 1;
            }
            else if (paymentAmount < 50)
            {
                matchPercentage = (paymentAmount / 100) * 3;
            }
            else
            {
                matchPercentage = (paymentAmount / 100) * 5;
            }

            return matchPercentage;
        }
    }
}
