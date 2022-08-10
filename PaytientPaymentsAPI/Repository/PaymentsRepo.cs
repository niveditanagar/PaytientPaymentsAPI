using PaytientPaymentsAPI.Data;
using PaytientPaymentsAPI.Models;
using PaytientPaymentsAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PaytientPaymentsAPI.Repository
{
    public class PaymentsRepo : IPaymentsRepo
    {
        private readonly PaytientPaymentsAPIDbContext _dbContext;

        public PaymentsRepo(PaytientPaymentsAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaymentsModel> AddPaymentAsync(PaymentsModel paymentsModel)
        {
            await _dbContext.Payments.AddAsync(paymentsModel);
            await _dbContext.SaveChangesAsync();

            return paymentsModel;
        }
        
        public async Task<PaymentsModel> GetLatestPaymentAsync(int personId)
        {
            var payment = await _dbContext.Payments
                .Where(x => x.PersonId == personId)
                .OrderByDescending(x => x.ScheduleDate)
                .FirstOrDefaultAsync();

            return payment;
        }
    }
}
