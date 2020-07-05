﻿using Logic.Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Models
{
    public class TransactionFeeModel
    {
        public DateTime PaymentDate { get; set; }
        public string MerchantName { get; set; }
        public double FeeAmount { get; set; }
    }
}
