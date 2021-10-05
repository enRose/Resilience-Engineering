using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace retry.ViewModels
{
    public class PersonalLoanViewModel
    {
        public IEnumerable<Content> App { get; set; }
    }

    public class Content
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
