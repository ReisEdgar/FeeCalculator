using System.Collections.Generic;

namespace Configuration
{
    public interface IConfigProvider
    {
        Dictionary<string, string> ConfigurationProperties { get; }
    }
}