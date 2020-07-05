using System.Collections.Generic;
using FileRepository.Models;
using Logic.Models;
using Logic.Models.Dto;

namespace Logic.Mappers
{
    public interface ITransactionMapper
    {
        TransactionDto MapTransaction(PaymentTransaction transaction);
        TransactionFeeModel MapTransactionFee(TransactionFee transaction);
        TransactionFee MapTransactionFee(TransactionFeeModel fee);
        IEnumerable<TransactionFee> MapTransactionFees(IEnumerable<TransactionFeeModel> fees);
    }
}