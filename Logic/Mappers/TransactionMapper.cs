using Logic.Models.Dto;
using Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using FileRepository.Models;

namespace Logic.Mappers
{
    public class TransactionMapper : ITransactionMapper
    {
        public TransactionDto MapTransaction(PaymentTransaction transaction)
        {
            return new TransactionDto
            {
                MerchantName = transaction.MerchantName,
                PaymentAmount = transaction.PaymentAmount,
                PaymentDate = transaction.PaymentDate
            };
        }
        public TransactionFee MapTransactionFee(TransactionFeeModel fee)
        {
            return new TransactionFee
            {
                MerchantName = fee.MerchantName,
                FeeAmount = fee.FeeAmount,
                PaymentDate = fee.PaymentDate
            };
        }
        public IEnumerable<TransactionFee> MapTransactionFees(IEnumerable<TransactionFeeModel> fees)
        {
            foreach (var fee in fees)
            {
                yield return new TransactionFee
                {
                    MerchantName = fee.MerchantName,
                    FeeAmount = fee.FeeAmount,
                    PaymentDate = fee.PaymentDate
                };
            }
        }
        public TransactionFeeModel MapTransactionFee(TransactionFee transaction)
        {
            return new TransactionFeeModel
            {
                MerchantName = transaction.MerchantName,
                FeeAmount = transaction.FeeAmount,
                PaymentDate = transaction.PaymentDate
            };
        }
    }
}
