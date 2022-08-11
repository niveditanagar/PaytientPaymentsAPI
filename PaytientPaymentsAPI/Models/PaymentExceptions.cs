using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaytientPaymentsAPI.Models
{
    public class PaymentException : Exception
    {
        public PaymentException(string message) : base(message)
        {

        }
    }
}
