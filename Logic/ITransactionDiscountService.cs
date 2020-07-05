using Logic.Models;

namespace Logic
{
    public interface ITransactionDiscountService
    {
        TransactionFeeModel ApplyTransactionDiscount(TransactionFeeModel fee);
    }
}