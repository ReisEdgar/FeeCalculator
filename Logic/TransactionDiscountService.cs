using Logic.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    public class TransactionDiscountService : ITransactionDiscountService
    {
        private readonly IConfiguration _configuration;

        public TransactionDiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public TransactionFeeModel ApplyTransactionDiscount(TransactionFeeModel fee)
        {
            fee = ApplyMerchantSpecificDiscount(fee);
            return fee;
        }
        private TransactionFeeModel ApplyMerchantSpecificDiscount(TransactionFeeModel fee)
        {
            switch (fee.MerchantName)
            {
                case Merchants.Telia:
                    fee.FeeAmount *= (1 - Double.Parse(_configuration["TeliaGenericDiscount"]));
                    break;
                case Merchants.CircleK:
                    fee.FeeAmount *= (1 - Double.Parse(_configuration["CircleKGenericDiscount"]));
                    break;
                case Merchants.Netto:
                    fee.FeeAmount *= (1 - Double.Parse(_configuration["NettoGenericDiscount"]));
                    break;
                case Merchants.SevenEleven:
                    fee.FeeAmount *= (1 - Double.Parse(_configuration["SevenElevenGenericDiscount"]));
                    break;
                default:
                    break;
            }
            return fee;
        }
    }
}
