using Configuration;
using FileRepository;
using FileRepository.Models;
using Logic;
using Logic.Mappers;
using Logic.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTe
{
    public class UnitTest
    {

        [Fact]
        public void ApplyDiscount_ExistingMerchant_DiscoutApplied()
        {
            var transactionFeeDiscountService = new TransactionFeeDiscountService(new ConfigProvider());
            var fee = new TransactionFeeModel()
            {
                FeeAmount = 1,
                MerchantName = DiscountMerchants.Telia
            };
            var discountedFee = transactionFeeDiscountService.ApplyTransactionDiscount(fee);
            Assert.Equal(0.9, discountedFee.FeeAmount);
        }
        [Fact]
        public void ApplyDiscount_NonExistingMerchant_FeeUnchanged()
        {
            var transactionFeeDiscountService = new TransactionFeeDiscountService(new ConfigProvider());
            var fee = new TransactionFeeModel()
            {
                FeeAmount = 1,
                MerchantName = "Merchant"
            };
            var discountedFee = transactionFeeDiscountService.ApplyTransactionDiscount(fee);
            Assert.Equal(1, discountedFee.FeeAmount);
        }
        [Theory, MemberData(nameof(ApplyFee_NormalTransaction_FeeApplied_Data))]

        public void ApplyFee_NormalTransaction_FeeApplied(IEnumerable<PaymentTransaction> transactions, double expected)
        {
            var mock = new Mock<ITransactionRepository>();

            mock.Setup(x => x.GetTransactions()).Returns(transactions);

            var transactionFeeService = new TransactionFeeService(new TransactionFeeDiscountService(new ConfigProvider()), mock.Object, new TransactionMapper(), new ConfigProvider());
            var fees = transactionFeeService.GetStandardFeeForeachTransaction();
            Assert.Equal(expected, fees.First().FeeAmount);


        }
        [Theory, MemberData(nameof(SumOfFees_NormalFees_FeesSumed_Data))]

        public void SumOfFees_NormalFees_FeesSumed(IEnumerable<TransactionFee> fees, double expected1, double expected2)
        {
            var mock = new Mock<ITransactionRepository>();

            mock.Setup(x => x.GetTransactionFees()).Returns(fees);

            var transactionFeeService = new TransactionFeeService(new TransactionFeeDiscountService(new ConfigProvider()), mock.Object, new TransactionMapper(), new ConfigProvider());
            foreach (var sumFeeForeachMonth in transactionFeeService.GetSumOfFeesForeachMonth())
            {
                Assert.Equal(expected1, sumFeeForeachMonth["Merchant"]);
                Assert.Equal(expected2, sumFeeForeachMonth["Merchant2"]);
            }
        }
        [Theory, MemberData(nameof(ApplyInvoiceFee_SumOfFeesIsZero_FeeNotApplied_Data))]

        public void ApplyInvoiceFee_SumOfFeesIsZeroAndNonZero_FeeNotAppliedAndNotApplied(IEnumerable<TransactionFee> fees, double expected1, double expected2)
        {
            var mock = new Mock<ITransactionRepository>();

            mock.Setup(x => x.GetTransactionFees()).Returns(fees);

            var transactionFeeService = new TransactionFeeService(new TransactionFeeDiscountService(new ConfigProvider()), mock.Object, new TransactionMapper(), new ConfigProvider());
            var sumFeeForeachMonth = transactionFeeService.GetSumOfFeesForeachMonth();
            var feeEnumerator = fees.GetEnumerator();
            feeEnumerator.MoveNext();

            foreach (var sumOfFees in sumFeeForeachMonth)
            {
                var finalFees = transactionFeeService.ApplyInvoiceFeeForSingleMonth(feeEnumerator, sumOfFees).ToList();
                Assert.Equal(finalFees.First(x => x.MerchantName == "Merchant").FeeAmount, expected1);
                Assert.Equal(finalFees.First(x => x.MerchantName == "Merchant2").FeeAmount, expected2);
            }
        }


        private static List<TransactionFee> TransactionFeeDataset = new List<TransactionFee>() {
                    new TransactionFee() { MerchantName = "Merchant", PaymentDate = new DateTime(2018,1,1), FeeAmount = 1 },
                    new TransactionFee() { MerchantName = "Merchant2", PaymentDate = new DateTime(2018,1,1), FeeAmount = 1 },
                    new TransactionFee() { MerchantName = "Merchant", PaymentDate = new DateTime(2018,1,2), FeeAmount = 1 },
                    new TransactionFee() { MerchantName = "Merchant2", PaymentDate = new DateTime(2018,1,2), FeeAmount = 1 },
                    new TransactionFee() { MerchantName = "Merchant", PaymentDate = new DateTime(2018,1,3), FeeAmount = 1 },
                    new TransactionFee() { MerchantName = "Merchant2", PaymentDate = new DateTime(2018,1,3), FeeAmount = -2 },

                    new TransactionFee() { MerchantName = "Merchant", PaymentDate = new DateTime(2018,2,1), FeeAmount = 1 },
                    new TransactionFee() { MerchantName = "Merchant2", PaymentDate = new DateTime(2018,2,1), FeeAmount = 1 },
                    new TransactionFee() { MerchantName = "Merchant", PaymentDate = new DateTime(2018,2,2), FeeAmount = 1 },
                    new TransactionFee() { MerchantName = "Merchant2", PaymentDate = new DateTime(2018,2,2), FeeAmount = 1 },
                    new TransactionFee() { MerchantName = "Merchant", PaymentDate = new DateTime(2018,2,3), FeeAmount = 1 },
                    new TransactionFee() { MerchantName = "Merchant2", PaymentDate = new DateTime(2018,2,3), FeeAmount = -2 },
                };
        public static IEnumerable<object[]> SumOfFees_NormalFees_FeesSumed_Data
        {
            get
            {
                return new List<object[]>
                {
                new object[] { TransactionFeeDataset, 3,0}
                };
            }
        }
        public static IEnumerable<object[]> ApplyInvoiceFee_SumOfFeesIsZero_FeeNotApplied_Data
        {
            get
            {
                return new List<object[]>
                {
                new object[] { TransactionFeeDataset, 30,1}
                };
            }
        }
        public static IEnumerable<object[]> ApplyFee_NormalTransaction_FeeApplied_Data
        {
            get
            {
                return new List<object[]>
                {
                new object[] { new List<PaymentTransaction>() { new PaymentTransaction() { MerchantName = DiscountMerchants.Telia, PaymentDate = new DateTime(), PaymentAmount = 100 } }, 0.9 },
                new object[] { new List<PaymentTransaction>() { new PaymentTransaction() { MerchantName = "Merchant", PaymentDate = new DateTime(), PaymentAmount = 100 } }, 1 }
                };
            }
        }
    }

}
