using System.Collections.Generic;
using FileRepository.Models;

namespace FileRepository
{
    public interface ITransactionRepository
    {
        IEnumerable<PaymentTransaction> GetTransactions();
        IEnumerable<TransactionFee> GetTransactionFees();
        void SaveTransactionFees(IEnumerable<TransactionFee> fees);

    }
}