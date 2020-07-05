using FileRepository;
using Logic.Mappers;
using Logic.Models;
using Logic.Models.Dto;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Logic
{
    public class TransactionFeeService : ITransactionFeeService
    {
        private readonly IConfiguration _configuration;
        private readonly ITransactionDiscountService _transactionDiscountService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionMapper _transactionMapper;

        public TransactionFeeService(IConfiguration configuration, ITransactionDiscountService transactionDiscountService, ITransactionRepository transactionRepository, ITransactionMapper transactionMapper)
        {
            _configuration = configuration;
            _transactionDiscountService = transactionDiscountService;
            _transactionRepository = transactionRepository;
            _transactionMapper = transactionMapper;
        }
        public IEnumerable<TransactionFeeModel> GetAllTransactionFees()
        {
            foreach(var transaction in _transactionRepository.GetTransactions())
            {
                var transactionDto = _transactionMapper.MapTransaction(transaction);
                var fee = GetSingleTransactionFee(transactionDto);
                yield return fee;
            }
        }
        private TransactionFeeModel GetSingleTransactionFee(TransactionDto transaction)
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
