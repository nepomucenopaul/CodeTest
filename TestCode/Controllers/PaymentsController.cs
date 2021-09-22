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
    [ODataRoutePrefix("Payments")]
    public class PaymentsController : ODataController
    {

        CodeTestContext CodeTestContext;

        public PaymentsController(CodeTestContext context)
        {
        CodeTestContext = context;
        }

        private bool PaymentExists(int key)
        {
            return CodeTestContext.Payments.Any(p => p.Id == key);
        }


        [ODataRoute]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Payment>), Status200OK)]
        [EnableQuery]
        [Authorize]
        public IQueryable<Payment> Get()
        {
            try
            {
                return CodeTestContext.Payments;
            }
                catch (Exception e)
            {

                throw e;
            }
           }

            [ODataRoute("({key})")]
            [Produces("application/json")]
            [ProducesResponseType(typeof(Payment), Status200OK)]
            [ProducesResponseType(Status404NotFound)]
            [EnableQuery]
            [Authorize]
            public SingleResult<Payment> Get([FromODataUri] int key)
            {

                try
                {
                    IQueryable<Payment> result = CodeTestContext.Payments.Where(p => p.Id == key);
                    return SingleResult.Create(result);

                }
                catch (Exception e)
                {

                    throw e;
                }

            }

            [ODataRoute]
            [ProducesResponseType(typeof(Payment), Status201Created)]
            [ProducesResponseType(Status400BadRequest)]
            [Authorize]
            public async Task<IActionResult> Post([FromBody] Payment Payment)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            CodeTestContext.Payments.Add(Payment);

                await CodeTestContext.SaveChangesAsync();

                return Created(Payment);
            }


            [ODataRoute("({key})")]
            [Produces("application/json")]
            [ProducesResponseType(typeof(Payment), Status200OK)]
            [ProducesResponseType(Status204NoContent)]
            [ProducesResponseType(Status400BadRequest)]
            [ProducesResponseType(Status404NotFound)]
            [Authorize]
            public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody] Delta<Payment> Payment)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var entity = await CodeTestContext.Payments.FindAsync(key);
                if (entity == null)
                {
                    return NotFound();
                }
                Payment.Patch(entity);
                try
                {
                    await CodeTestContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(key))
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


            var Payment = await CodeTestContext.Payments.FindAsync(key);

            if (Payment == null)
            {
                return NotFound();
            }

                //check if IsDeleted field is present
            var softDeleteCheck = Payment.GetType().GetProperty("IsDeleted");

            if (softDeleteCheck == null)
            {
                    // property does not exist proceed with hard delete
                    CodeTestContext.Payments.Remove(Payment);
            }
            else
            {
                // property exists proceed with soft delete
                Payment.IsDeleted = true;
            }

            await CodeTestContext.SaveChangesAsync();
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}
