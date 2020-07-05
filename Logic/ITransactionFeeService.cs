using Logic.Models;
using Logic.Models.Dto;

namespace Logic
{
    public interface ITransactionFeeService
    {
        TransactionFeeModel GetTransactionFee(TransactionDto transaction);
    }
}