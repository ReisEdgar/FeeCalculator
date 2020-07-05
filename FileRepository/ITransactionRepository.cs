using System.Collections.Generic;
using FileRepository.Models;

namespace FileRepository
{
    public interface ITransactionRepository
    {
        IEnumerable<TransactionModel> GetTransactions();
    }
}