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
        public TransactionFee MapTransactionFee(TransactionFeeModel transaction)
        {
            return new TransactionFee
            {
                MerchantName = transaction.MerchantName,
                FeeAmount = transaction.FeeAmount,
                PaymentDate = transaction.PaymentDate
            };
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
