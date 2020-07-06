using Infrastructure;
using Presentation;

namespace FeeCalculator
{
    public class Program
    {
        static void Main(string[] args)
        {
            DependencyInjectionService.RegisterDependencies();

            var displayService = DependencyInjectionService.Resolve<ITransactionFeeDisplay>();
            displayService.DisplayTransactionFees();
        }
    }
}
