using Configuration;
using Logic.Models;
using System;

namespace Logic
{
    public class TransactionFeeDiscountService : ITransactionFeeDiscountService
    {
        private readonly IConfigProvider _configProvider;
        public TransactionFeeDiscountService(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
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
                case DiscountMerchants.Telia:
                    fee.FeeAmount *= (1 - Double.Parse(_configProvider.ConfigurationProperties["teliaGenericDiscount"]));
                    break;
                case DiscountMerchants.CircleK:
                    fee.FeeAmount *= (1 - Double.Parse(_configProvider.ConfigurationProperties["circleKGenericDiscount"]));
                    break;
                default:
                    break;
            }
            return fee;
        }
    }
}
