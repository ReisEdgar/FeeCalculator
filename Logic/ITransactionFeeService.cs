using Logic.Models;
using Logic.Models.Dto;
using System.Collections.Generic;

namespace Logic
{
    public interface ITransactionFeeService
    {
        IEnumerable<TransactionFeeModel> GetAllTransactionFees();
    }
}