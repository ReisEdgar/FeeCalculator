using System;
using System.Collections.Generic;

namespace Configuration
{
    public class ConfigProvider : IConfigProvider
    {
        public Dictionary<string, string> ConfigurationProperties { get; } = new Dictionary<string, string>()
        {
            { "feeSaveFilePath", "C:\\Users\\Ivan\\source\\repos\\FeeCalculator\\fees.txt" },
            { "transactionsFilePath", "C:\\Users\\Ivan\\source\\repos\\FeeCalculator\\transactions.txt" },
            { "teliaGenericDiscount", "0,1" },
            { "circleKGenericDiscount", "0,2" },
            { "fixedInvoiceFee", "29" },
            { "transactionFeePercentage", "0,01" }
        };

    }
}
