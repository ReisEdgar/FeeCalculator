using System.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using FileRepository.Models;

namespace FileRepository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IConfiguration _configuration;

        public TransactionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<TransactionModel> GetTransactions()
        {

            var filePath = _configuration["transactionsFilePath"];

            System.IO.StreamReader file = new System.IO.StreamReader(filePath);
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

        private TransactionModel CreateTransaction(string[] transactionData)
        {
            try
            {
                return new TransactionModel
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
    }
}
