using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace retry.Entities
{
    public class PersonalLoan
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public App App { get; set; }
        public IEnumerable<Account> Accounts { get; set; }
    }

    public class App
    {
        public string Id { get; set; }
        public string Data { get; set; }
    }

    public class Account
    {
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
