using System;
using System.Collections.Generic;

namespace retry.Entities
{
    public class PersonalLoan
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public IEnumerable<Content> App { get; set; }
    }

    public class Content
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
