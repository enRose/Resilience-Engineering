using System;
using System.Collections.Generic;
using retry.ViewModels;

namespace retry.Entities
{
    public class PersonalLoan
    {
        public int Id { get; set; }
        public IEnumerable<Content> App { get; set; }
    }
}
