using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaytientPaymentsAPI.Models
{
    public class AddOneTimePaymentRequestModel
    {
        public decimal PaymentAmount { get; set; }

        public int PersonId { get; set; }
    }
}
