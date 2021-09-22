using System;
using System.Collections.Generic;

#nullable disable

namespace TestCode.Data
{
    public partial class Payment
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTimeOffset PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public int StatusId { get; set; }
        public string AdditionalComment { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Account Account { get; set; }
        public virtual Status Status { get; set; }
    }
}
