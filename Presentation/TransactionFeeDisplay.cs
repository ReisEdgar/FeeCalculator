﻿using Logic;
using Presentation.Mappers;
using System;
using System.Globalization;

namespace Presentation
{
    public class TransactionFeeDisplay : ITransactionFeeDisplay
    {
        private readonly ITransactionFeeService _transactionFeeService;
        private readonly ITransactionFeeMapper _transactionFeeMapper;
        public TransactionFeeDisplay(ITransactionFeeService transactionFeeService, ITransactionFeeMapper transactionFeeMapper)
        {
            _transactionFeeService = transactionFeeService;
            _transactionFeeMapper = transactionFeeMapper;
        }
        public void DisplayTransactionFees()
        {
            var monthlyFees = _transactionFeeService.GetMonthlyTransactionFees();
            foreach (var feesForMonth in monthlyFees)
            {
                foreach (var fee in feesForMonth)
                {
                    var feeForDisplay = _transactionFeeMapper.MapTransactionFee(fee);
                    Console.WriteLine($"{fee.PaymentDate.ToShortDateString(),-10} {fee.MerchantName,-10} {fee.FeeAmount.ToString("0.00", new CultureInfo("en-US")),-10}");
                }
                Console.WriteLine("");
            }
        }
    }
}
