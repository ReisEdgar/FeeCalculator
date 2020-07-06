using System.Collections.Generic;
using Logic.Models;

namespace Logic
{
    public interface ITransactionFeeService
    {
        IEnumerable<IEnumerable<TransactionFeeModel>> GetMonthlyTransactionFees();
    }
}