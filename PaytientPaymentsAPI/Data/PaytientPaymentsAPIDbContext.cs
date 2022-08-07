using Microsoft.EntityFrameworkCore;
using PaytientPaymentsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaytientPaymentsAPI.Data
{
    public class PaytientPaymentsAPIDbContext : DbContext
    {
        public PaytientPaymentsAPIDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<PaymentsModel> Payments { get; set; }
        public DbSet<PersonsModel> Persons { get; set; }
    }
}
