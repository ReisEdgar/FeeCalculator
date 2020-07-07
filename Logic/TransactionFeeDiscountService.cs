using Configuration;
using Logic.Models;
using System;

namespace Logic
{
    public class TransactionFeeDiscountService : ITransactionFeeDiscountService
    {

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
                    fee.FeeAmount *= (1 - Double.Parse(ConfigProvider.TeliaGenericDiscount));
                    break;
                case DiscountMerchants.CircleK:
                    fee.FeeAmount *= (1 - Double.Parse(ConfigProvider.CircleKGenericDiscount));
                    break;
                default:
                    break;
            }
            return fee;
        }
    }
}
