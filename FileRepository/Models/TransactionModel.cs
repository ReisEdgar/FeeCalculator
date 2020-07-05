using System;
using System.Collections.Generic;
using System.Text;

namespace FileRepository.Models
{
    public class TransactionModel
    {
        public DateTime PaymentDate { get; set; }
        public string MerchantName { get; set; }
        public double PaymentAmount { get; set; }
    }
}
