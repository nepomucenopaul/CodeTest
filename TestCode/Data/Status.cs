using System;
using System.Collections.Generic;

#nullable disable

namespace TestCode.Data
{
    public partial class Status
    {
        public Status()
        {
            Accounts = new HashSet<Account>();
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
