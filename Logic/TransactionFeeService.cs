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
            var fees = GetStandardFeeForeachTransaction();
            SaveTransactionFeesToFile(fees);

        }
        private void SaveTransactionFeesToFile(IEnumerable<TransactionFeeModel> fees)
        {
            var entityFees = _transactionMapper.MapTransactionFees(fees);
            _transactionRepository.SaveTransactionFees(entityFees);
        }

        private IEnumerable<TransactionFeeModel> GetStandardFeeForeachTransaction()
        {
            foreach (var transaction in _transactionRepository.GetTransactions())
            {
                if(transaction == null)
                {
                    // Log error details
                    continue;
                }

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
        private IEnumerable<Dictionary<string, double>> GetTotalMonthlyFees()
        {
            var merchantMonthlyTotalFees = new Dictionary<string, double>();
            var previousMonth = new DateTime();
            var firstIteration = true;
            foreach (var fee in _transactionRepository.GetTempTransactionFees())
            {
                var exists = merchantMonthlyTotalFees.ContainsKey(fee.MerchantName);
                var month = new DateTime(fee.PaymentDate.Year, fee.PaymentDate.Month, 1);
                if (firstIteration)
                {
                    previousMonth = month;
                    firstIteration = false;
                }
                else
                {
                    if(month != previousMonth)
                    {
                        yield return merchantMonthlyTotalFees;
                        previousMonth = month;
                        merchantMonthlyTotalFees = new Dictionary<string, double>();
                    }
                }

                if (exists)
                {
                    merchantMonthlyTotalFees[fee.MerchantName] += fee.FeeAmount;
                }
                else
                {
                    merchantMonthlyTotalFees.Add(fee.MerchantName, 0);
                }
            }
        }
    }
}
