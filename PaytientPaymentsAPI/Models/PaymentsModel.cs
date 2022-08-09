using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaytientPaymentsAPI.Models
{
    public class PaymentsModel
    {
        public int Id { get; set; }

        public decimal Balance { get; set; }

        public decimal PaymentAmount { get; set; }

        public DateTime ScheduleDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        public int PersonId { get; set; }
    }
}
