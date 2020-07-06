using Logic.Models;
using Presentation.Models;

namespace Presentation.Mappers
{
    public interface ITransactionFeeMapper
    {
        TransactionFeeDisplayModel MapTransactionFee(TransactionFeeModel fee);
    }
}