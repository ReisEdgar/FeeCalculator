using FileRepository;
using Infrastructure;
using Logic;
using Logic.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace FeeCalculator
{
    public class Program
    {
        static void Main(string[] args)
        {


            // /     var config = Configuration.Build();
            ///   var author = config.GetSection("author").Get<Person>();

            //  IoC.Register<ITransactionFeeService, TransactionFeeService>();
            // IoC.Register<ITransactionMapper, TransactionMapper>();
            //ITransactionMapper service = IoC.Resolve<ITransactionMapper>();
            DependencyInjectionService.RegisterDependencies();/*
            var serviceProvider = new ServiceCollection()
                .AddScoped<ITransactionMapper, TransactionMapper>()
                .AddScoped<ITransactionFeeService, TransactionFeeService>()
                .AddScoped<ITransactionDiscountService, TransactionDiscountService>()
                .AddScoped<ITransactionRepository, TransactionRepository>()
                .AddSingleton(Configuration)
                .BuildServiceProvider();*/
            var sample1Service = DependencyInjectionService.Resolve<ITransactionFeeService>();
            var f = sample1Service.GetMonthlyTransactionFees();
            foreach(var q in f)
            {
                foreach(var w in q)
                {
                    Console.WriteLine($"{w.MerchantName}  {w.PaymentDate}   {w.FeeAmount}");
                }
            }

            var g = 0;
        }
    }
}
