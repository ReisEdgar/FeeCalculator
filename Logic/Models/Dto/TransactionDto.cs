using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Models.Dto
{
    public class TransactionDto
    {
        public DateTime PaymentDate { get; set; }
        public string MerchantName { get; set; }
        public double PaymentAmount { get; set; }
    }
}
