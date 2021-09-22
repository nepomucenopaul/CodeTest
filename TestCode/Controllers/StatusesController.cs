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
        [ODataRoutePrefix("Statuses")]
        public class StatusesController : ODataController
        {

            CodeTestContext CodeTestContext;

            public StatusesController(CodeTestContext context)
            {
            CodeTestContext = context;
            }

            private bool StatusExists(int key)
            {
                return CodeTestContext.Statuses.Any(p => p.Id == key);
            }


        [ODataRoute]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Status>), Status200OK)]
        [EnableQuery]
        [Authorize]
        public IQueryable<Status> Get()

        {
            try
            {
                return CodeTestContext.Statuses;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        [ODataRoute("({key})")]
            [Produces("application/json")]
            [ProducesResponseType(typeof(Status), Status200OK)]
            [ProducesResponseType(Status404NotFound)]
            [EnableQuery]
        [Authorize]
        public SingleResult<Status> Get([FromODataUri] int key)
            {

                try
                {
                    IQueryable<Status> result = CodeTestContext.Statuses.Where(p => p.Id == key);
                    return SingleResult.Create(result);

                }
                catch (Exception e)
                {

                    throw e;
                }

            }

            [ODataRoute]
            [ProducesResponseType(typeof(Status), Status201Created)]
            [ProducesResponseType(Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Status Status)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            CodeTestContext.Statuses.Add(Status);

                await CodeTestContext.SaveChangesAsync();

                return Created(Status);
            }


            [ODataRoute("({key})")]
            [Produces("application/json")]
            [ProducesResponseType(typeof(Status), Status200OK)]
            [ProducesResponseType(Status204NoContent)]
            [ProducesResponseType(Status400BadRequest)]
            [ProducesResponseType(Status404NotFound)]
            [Authorize]
            public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody] Delta<Status> Status)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var entity = await CodeTestContext.Statuses.FindAsync(key);
                if (entity == null)
                {
                    return NotFound();
                }
                Status.Patch(entity);
                try
                {
                    await CodeTestContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusExists(key))
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


                var Status = await CodeTestContext.Statuses.FindAsync(key);

                if (Status == null)
                {
                    return NotFound();
                }

                //check if IsDeleted field is present
                var softDeleteCheck = Status.GetType().GetProperty("IsDeleted");

                if (softDeleteCheck == null)
                {
                    // property does not exist proceed with hard delete
                    CodeTestContext.Statuses.Remove(Status);
                }
                else
                {
                    // property exists proceed with soft delete
                    Status.IsDeleted = true;
                }

                await CodeTestContext.SaveChangesAsync();
                return StatusCode((int)HttpStatusCode.NoContent);
            }
        }
}
