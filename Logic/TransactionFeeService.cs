using Logic.Models;
using Logic.Models.Dto;
using Microsoft.Extensions.Configuration;
using System;

namespace Logic
{
    public class TransactionFeeService : ITransactionFeeService
    {
        private readonly IConfiguration _configuration;
        private readonly ITransactionDiscountService _transactionDiscountService;

        public TransactionFeeService(IConfiguration configuration, ITransactionDiscountService transactionDiscountService)
        {
            _configuration = configuration;
            _transactionDiscountService = transactionDiscountService;
        }
        public TransactionFeeModel GetTransactionFee(TransactionDto transaction)
        {
            var fee = ApplyTransactionFee(transaction);
            fee = _transactionDiscountService.ApplyTransactionDiscount(fee);
            return fee;
        }
        private TransactionFeeModel ApplyTransactionFee(TransactionDto transaction)
        {
            var transactionFeePercentage = Double.Parse(_configuration["transactionFeePercentage"]);
            var transactionFee = new TransactionFeeModel
            {
                MerchantName = transaction.MerchantName,
                PaymentDate = transaction.PaymentDate,
                FeeAmount = transaction.PaymentAmount * transactionFeePercentage
            };
            return transactionFee;
        }

    }
}
