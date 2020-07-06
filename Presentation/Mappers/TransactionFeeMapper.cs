using Logic.Models;
using Presentation.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.Mappers
{
    public class TransactionFeeMapper : ITransactionFeeMapper
    {
        public TransactionFeeDisplayModel MapTransactionFee(TransactionFeeModel fee)
        {
            return new TransactionFeeDisplayModel
            {
                PaymentDate = fee.PaymentDate,
                MerchantName = fee.MerchantName,
                FeeAmount = fee.FeeAmount
            };
        }
    }
}
