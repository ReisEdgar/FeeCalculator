using System;

namespace Configuration
{
    public class ConfigProvider
    {
        public static string FeeSaveFilePath { get; } = "C:\\Users\\Ivan\\source\\repos\\FeeCalculator\\fees.txt";
        public static string TransactionsFilePath { get; } = "C:\\Users\\Ivan\\source\\repos\\FeeCalculator\\transactions.txt";
        public static string TeliaGenericDiscount { get; } = "0,1";
        public static string CircleKGenericDiscount { get; } = "0,2";
        public static string FixedInvoiceFee { get; } = "29";
        public static string TransactionFeePercentage { get; } = "0,01";
    }
}
