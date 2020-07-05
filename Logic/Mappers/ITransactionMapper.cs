using FileRepository.Models;
using Logic.Models.Dto;

namespace Logic.Mappers
{
    public interface ITransactionMapper
    {
        TransactionDto MapTransaction(PaymentTransaction transaction);
    }
}