using FileRepository.Models;
using Logic.Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Mappers
{
    public class TransactionMapper : ITransactionMapper
    {
        public TransactionDto MapTransaction(TransactionModel transaction)
        {
            return new TransactionDto
            {
                MerchantName = transaction.MerchantName,
                PaymentAmount = transaction.PaymentAmount,
                PaymentDate = transaction.PaymentDate
            };
        }
    }
}
