using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace retry.ViewModels
{
    public class PersonalLoanVm
    {
        [JsonPropertyName("app")]
        public AppVm App { get; set; }

        [JsonPropertyName("accounts")]
        public IEnumerable<AccountVm> Accounts { get; set; }
    }

    public class AppVm
    {
        [JsonPropertyName("data")]
        public string Data { get; set; }
    }

    public class AccountVm
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
