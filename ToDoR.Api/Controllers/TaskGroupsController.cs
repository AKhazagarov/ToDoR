using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoR.Common.Contracts;
using ToDoR.DataAccess.Context;

namespace ToDoR.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskGroupsController : ControllerBase
    {
        private readonly ToDoDbContext _context;

        public TaskGroupsController(ToDoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ActionResult<List<User>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TaskGroup>>> GetAll()
        {
              return Ok(await _context.TaskGroups.ToListAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ActionResult<TaskGroup>), StatusCodes.Status200OK)]
        public async Task<ActionResult<TaskGroup>> GetById(Guid? id)
        {
            if (id == null || _context.TaskGroups == null)
            {
                return NotFound();
            }

            var taskGroups = await _context.TaskGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskGroups == null)
            {
                return NotFound();
            }

            return Ok(taskGroups);
        }

        // POST: TaskGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        public async Task<IActionResult> Create([Bind("Id,UserId,Name")] TaskGroup taskGroups)
        {
            if (ModelState.IsValid)
            {
                taskGroups.Id = Guid.NewGuid();
                _context.Add(taskGroups);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        // POST: TaskGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> Edit([Bind("Id,UserId,Name")] TaskGroup taskGroups)
        {
            if (taskGroups.Id != taskGroups.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskGroups);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskGroupsExists(taskGroups.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return Ok();
        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TaskGroups == null)
            {
                return Problem("Entity set 'ToDoContext.TaskGroups'  is null.");
            }
            var taskGroups = await _context.TaskGroups.FindAsync(id);
            if (taskGroups != null)
            {
                _context.TaskGroups.Remove(taskGroups);
            }
            
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool TaskGroupsExists(Guid id)
        {
          return _context.TaskGroups.Any(e => e.Id == id);
        }
    }
}
