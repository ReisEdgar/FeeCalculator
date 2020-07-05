using System.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using FileRepository.Models;
using System.IO;

namespace FileRepository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IConfiguration _configuration;

        public TransactionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IEnumerable<TransactionFee> GetTransactionFees()
        {

            var filePath = _configuration["tempFeeSaveFilePath"];

            using (StreamReader file = new StreamReader(filePath))
            {
                string line;

                try
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        line = line.TrimEnd().TrimStart();
                        if (line == "")
                        {
                            continue;
                        }

                        var parts = line.Split(' ');
                        yield return CreateTransactionFee(parts);
                    }
                }
                finally
                {
                    file.Close();
                }
            }
        }
        public IEnumerable<PaymentTransaction> GetTransactions()
        {

            var filePath = _configuration["transactionsFilePath"];

            using (StreamReader file = new StreamReader(filePath))
            {
                string line;

                try
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        line = line.TrimEnd().TrimStart();
                        if (line == "")
                        {
                            continue;
                        }

                        var parts = line.Split(' ');
                        yield return CreateTransaction(parts);
                    }
                }
                finally
                {
                    file.Close();
                }
            }
        }

        private PaymentTransaction CreateTransaction(string[] transactionData)
        {
            try
            {
                return new PaymentTransaction
                {
                    PaymentDate = DateTime.Parse(transactionData[0]),
                    MerchantName = transactionData[1],
                    PaymentAmount = Double.Parse(transactionData[2]),
                };
            }
            catch (Exception ex)
            {
                // Log error....
                return null;
            }
        }
        private TransactionFee CreateTransactionFee(string[] feeData)
        {
            try
            {
                return new TransactionFee
                {
                    PaymentDate = DateTime.Parse(feeData[0]),
                    MerchantName = feeData[1],
                    FeeAmount = Double.Parse(feeData[2]),
                };
            }
            catch (Exception ex)
            {
                // Log error....
                return null;
            }
        }
        public void SaveTransactionFees(IEnumerable<TransactionFee> fees)
        {
            var filePath = _configuration["tempFeeSaveFilePath"];
            using (StreamWriter file = new StreamWriter(filePath))
            {
                try
                {
                    foreach (var fee in fees)
                    {
                        var stringFee = $"{fee.PaymentDate} {fee.MerchantName} {fee.FeeAmount}";
                        file.Write(stringFee);
                    }
                }
                finally
                {
                    file.Close();
                }
            }
        }
    }
}
