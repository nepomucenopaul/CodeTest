using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestCode.Data;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace TestCode.Controllers
{
    [ApiVersion("1.0")]
    [ODataRoutePrefix("Accounts")]
    public class AccountsController : ODataController
    {

        CodeTestContext CodeTestContext;

        public AccountsController(CodeTestContext context)
        {
            CodeTestContext = context;
        }

        private bool AccountExists(int key)
        {
            return CodeTestContext.Accounts.Any(p => p.Id == key);
        }


        [ODataRoute]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Account>), Status200OK)]
        [Authorize]
        [EnableQuery]
        public IQueryable<Account> Get()

        {
            try
            {
                return CodeTestContext.Accounts;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        [ODataRoute("({key})")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Account), Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [Authorize]
        [EnableQuery]
        public SingleResult<Account> Get([FromODataUri] int key)
        {

            try
            {
                IQueryable<Account> result = CodeTestContext.Accounts.Where(p => p.Id == key);
                return SingleResult.Create(result);

            }
            catch (Exception e)
            {

                throw e;
            }

        }

        [ODataRoute]
        [ProducesResponseType(typeof(Account), Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Account Account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CodeTestContext.Accounts.Add(Account);

            await CodeTestContext.SaveChangesAsync();

            return Created(Account);
        }


        [ODataRoute("({key})")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Account), Status200OK)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody] Delta<Account> Account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await CodeTestContext.Accounts.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            Account.Patch(entity);
            try
            {
                await CodeTestContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(entity);
        }

        [ODataRoute("({key})")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        [Authorize]
        public async Task<ActionResult> Delete([FromODataUri] int key)
        {


            var Account = await CodeTestContext.Accounts.FindAsync(key);

            if (Account == null)
            {
                return NotFound();
            }

            //check if IsDeleted field is present
            var softDeleteCheck = Account.GetType().GetProperty("IsDeleted");

            if (softDeleteCheck == null)
            {
                // property does not exist proceed with hard delete
                CodeTestContext.Accounts.Remove(Account);
            }
            else
            {
                // property exists proceed with soft delete
                Account.IsDeleted = true;
            }

            await CodeTestContext.SaveChangesAsync();
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}
