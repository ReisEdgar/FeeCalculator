using System.Configuration;
using System;
using System.Collections.Generic;
using FileRepository.Models;
using System.IO;
using Configuration;
using System.Text.RegularExpressions;

namespace FileRepository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IConfigProvider _configProvider;
        public TransactionRepository(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
        }
        public IEnumerable<TransactionFee> GetTransactionFees()
        {

            var filePath = _configProvider.ConfigurationProperties["feeSaveFilePath"];

            using (StreamReader file = new StreamReader(filePath))
            {
                string line;

                try
                {
                        while ((line = file.ReadLine()) != null)
                    {
                        line = line?.Trim();
                        if (line == "" || line == null)
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

            var filePath = _configProvider.ConfigurationProperties["transactionsFilePath"];

            using (StreamReader file = new StreamReader(filePath))
            {
                string line;

                try
                {
                    while (file.Peek() >= 0)
                    {
                        line = file.ReadLine();
                        line = Regex.Replace(line, @"\s+", " ");
                        if (line == "" || line == null)
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
            var filePath = _configProvider.ConfigurationProperties["feeSaveFilePath"];
            using (StreamWriter file = new StreamWriter(filePath))
            {
                try
                {
                    foreach (var fee in fees)
                    {
                        var stringFee = $"{fee.PaymentDate.ToShortDateString()} {fee.MerchantName} {fee.FeeAmount} \n";
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
