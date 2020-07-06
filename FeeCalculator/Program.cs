using FileRepository;
using Infrastructure;
using Logic;
using Logic.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation;
using System;
using System.Linq;

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
