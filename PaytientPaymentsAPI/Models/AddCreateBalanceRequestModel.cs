using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaytientPaymentsAPI.Models
{
    public class AddCreateBalanceRequestModel
    {
        public int PersonId { get; set; }

        public decimal Balance { get; set; }
        
    }
}
