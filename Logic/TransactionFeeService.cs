using Configuration;
using FileRepository;
using FileRepository.Models;
using Logic.Mappers;
using Logic.Models;
using Logic.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{
    public class TransactionFeeService : ITransactionFeeService
    {
        private readonly ITransactionDiscountService _transactionDiscountService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionMapper _transactionMapper;

        public TransactionFeeService(ITransactionDiscountService transactionDiscountService, ITransactionRepository transactionRepository, ITransactionMapper transactionMapper)
        {
            _transactionDiscountService = transactionDiscountService;
            _transactionRepository = transactionRepository;
            _transactionMapper = transactionMapper;
        }

        public IEnumerable<IEnumerable<TransactionFeeModel>> GetMonthlyTransactionFees()
        {
            var fees = GetStandardFeeForeachTransaction();
            return ApplyInvoiceFeesForeachMonth(fees);
        }

        public IEnumerable<IEnumerable<TransactionFeeModel>> ApplyInvoiceFeesForeachMonth(IEnumerable<TransactionFeeModel> fees)
        {
            SaveTransactionFeesToFile(fees);
            var totalMonthlyFees = GetTotalMonthlyFees();
            var entityFees = _transactionRepository.GetTransactionFees();
            var feeEnumerator = entityFees.GetEnumerator();
            feeEnumerator.MoveNext();
            foreach (var totalFeesForSingleMonth in totalMonthlyFees)
            {
                yield return ApplyInvoiceFeeForSingleMonth(feeEnumerator, totalFeesForSingleMonth);
            }
        }

        private IEnumerable<TransactionFeeModel> ApplyInvoiceFeeForSingleMonth(IEnumerator<TransactionFee> feeEnumerator, Dictionary<string, double> totalMonthlyFees)
        {
            var merchantWithAppliedInvoiceFee = new Dictionary<string, bool>();
            var fee = _transactionMapper.MapTransactionFee(feeEnumerator.Current);
            var month = new DateTime(fee.PaymentDate.Year, fee.PaymentDate.Month, 1);

            var currentMonth = new DateTime(fee.PaymentDate.Year, fee.PaymentDate.Month, 1);
            while (month == currentMonth)
            {
                var invoiceTaxApplied = merchantWithAppliedInvoiceFee.ContainsKey(fee.MerchantName);
                if (!invoiceTaxApplied)
                {
                    var totalMonthlyFee = totalMonthlyFees[fee.MerchantName];
                    merchantWithAppliedInvoiceFee.Add(fee.MerchantName, true);
                    fee.FeeAmount += totalMonthlyFee > 0 ? Double.Parse(ConfigProvider.FixedInvoiceFee) : 0;
                }
                yield return fee;
                var hasNext = feeEnumerator.MoveNext();
                if (!hasNext)
                    break;
                fee = _transactionMapper.MapTransactionFee(feeEnumerator.Current);
                currentMonth = new DateTime(fee.PaymentDate.Year, fee.PaymentDate.Month, 1);
            }
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
                if (transaction == null)
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
            var transactionFeePercentage = Double.Parse(ConfigProvider.TransactionFeePercentage);
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
            foreach (var fee in _transactionRepository.GetTransactionFees())
            {
                var month = new DateTime(fee.PaymentDate.Year, fee.PaymentDate.Month, 1);
                if (firstIteration)
                {
                    previousMonth = month;
                    firstIteration = false;
                }
                else
                {
                    if (month != previousMonth)
                    {
                        yield return merchantMonthlyTotalFees;
                        previousMonth = month;
                        merchantMonthlyTotalFees = new Dictionary<string, double>();
                    }
                }
                var exists = merchantMonthlyTotalFees.ContainsKey(fee.MerchantName);

                if (exists)
                {
                    merchantMonthlyTotalFees[fee.MerchantName] += fee.FeeAmount;
                }
                else
                {
                    merchantMonthlyTotalFees.Add(fee.MerchantName, fee.FeeAmount);
                }
            }
            yield return merchantMonthlyTotalFees;
        }
    }
}
