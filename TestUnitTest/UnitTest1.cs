using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TestCode.Controllers;
using TestCode.Data;

namespace TestUnitTest
{
    [TestFixture]
    public class UnitTest1
    {
        CodeTestContext CodeTestContext;

        [SetUp]
        public void SetUp()
        {
            CodeTestContext = new CodeTestContext();
        }

        [Test]
        public void TestAccountsGet()
        {
            var controller = new AccountsController(CodeTestContext);
            var result = controller.Get();
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetPaymentGet()
        {
            var controller = new PaymentsController(CodeTestContext);
            var result = controller.Get();
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetStatusGet()
        {
            var controller = new StatusesController(CodeTestContext);
            var result = controller.Get();
            Assert.IsNotNull(result);
        }
        [Test]
        [TestCase(1)]
        public void CheckIfAccountHasPayment(int id)
        {
            var testAccounts = CodeTestContext.Accounts.Where(x=> x.Id == id).Include(x=> x.Payments).FirstOrDefault();
            Assert.That(testAccounts.Payments != null, "There is no Payment associated with Account");
        }

        [Test]
        public void GetOpenAccounts()
        {
            int? StatusId = CodeTestContext.Statuses.Where(x => x.Name == "Open").FirstOrDefault().Id;
            var testAccounts = CodeTestContext.Accounts.Where(x => x.StatusId == StatusId).ToList();
            Assert.IsNotNull(testAccounts);
            Assert.That(StatusId != null, "There is no --Open-- status");
        }

    }
}
