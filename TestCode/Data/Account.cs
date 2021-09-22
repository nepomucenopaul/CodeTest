using System;
using System.Collections.Generic;

#nullable disable

namespace TestCode.Data
{
    public partial class Account
    {
        public Account()
        {
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public decimal OpeningBalance { get; set; }
        public int StatusId { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
       
        public virtual Status Status { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
