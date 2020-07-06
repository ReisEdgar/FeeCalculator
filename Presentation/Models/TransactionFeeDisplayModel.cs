using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.Models
{
    public class TransactionFeeDisplayModel
    {
        public DateTime PaymentDate { get; set; }
        public string MerchantName { get; set; }
        public double FeeAmount { get; set; }
    }
}
