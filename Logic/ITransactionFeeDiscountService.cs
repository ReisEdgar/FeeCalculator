using Logic.Models;

namespace Logic
{
    public interface ITransactionFeeDiscountService
    {
        TransactionFeeModel ApplyTransactionDiscount(TransactionFeeModel fee);
    }
}