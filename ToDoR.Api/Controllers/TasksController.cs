using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoR.Api.Models;
using ToDoR.Common.Contracts;
using ToDoR.DataAccess.Interfaces;

namespace ToDoR.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IDoTaskProvider _dbProvider;

        public TasksController(IDoTaskProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        /// <summary>
        /// Returns all items in the to-do list
        /// </summary>
        /// <returns>to-do list</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ActionResult<List<TaskModel>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TaskModel>>> GetAll()
        {
            var tasks = await _dbProvider.Requests.ToListAsync();

            var tasksModel = tasks.Select(t =>
                new TaskModel()
                {
                    Id = t.Id,
                    CreatedAt = t.CreatedAt,
                    DueDate = t.DueDate,
                    Note = t.Note,
                    Status = t.Status,
                    Name = t.Name,
                });

            return Ok(tasksModel);
        }

        /// <summary>
        /// Returns all items in the to-do list by filter
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>to-do list</returns>
        [HttpPost("byFilter")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ActionResult<List<TaskModel>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TaskModel>>> GetByFilter(TaskFilterModel filter)
        {
            if (filter is null)
            {
                return BadRequest();
            }

            var tasks = _dbProvider.Requests;

            if (filter.ShowDeleted != true)
            {
                tasks = tasks.Where(s => s.DeletedAt == null);
            }

            tasks = tasks.Where(c => filter.Status.Contains(c.Status));

            var tasksModel = tasks.Select(t =>
                new TaskModel()
                {
                    Id = t.Id,
                    CreatedAt = t.CreatedAt,
                    DueDate = t.DueDate,
                    Note = t.Note,
                    Status = t.Status,
                    Name = t.Name,
                });

            return Ok(tasksModel);
        }

        /// <summary>
        /// Adds a new record to the database
        /// </summary>
        /// <param name="taskAdd">New task</param>
        /// <returns>Status</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        public async Task<ActionResult> Add(DoTaskAdd taskAdd)
        {
            if (taskAdd is null)
            {
                return BadRequest();
            }

            if (taskAdd.Name.Length < 10)
            {
                return BadRequest("Minimum length 10 characters");
            }

            await _dbProvider.Add(taskAdd);

            return Ok();
        }

        /// <summary>
        /// Changes the record
        /// </summary>
        /// <param name="model">The record</param>
        /// <returns>Status</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        public async Task<IActionResult> Edit(TaskEditModel model)
        {
            if (model is null)
            {
                return BadRequest();
            }

            var task = await _dbProvider.Requests.FirstOrDefaultAsync(m => m.Id == model.Id);

            if (task is null)
            {
                return NotFound();
            }

            await _dbProvider.Update(new DoTaskEdit()
            {
                Id = task.Id,
                Name = model.Name,
                Note = model.Note,
                DueDate = model.DueDate,
            });

            return Ok();
        }

        /// <summary>
        /// Marks a task as completed
        /// </summary>
        /// <param name="id">Record ID</param>
        /// <returns>Status</returns>
        [HttpPut("complete/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        public async Task<IActionResult> MarkComplete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var task = await _dbProvider.Requests.FirstOrDefaultAsync(m => m.Id == id);

            if (task is null)
            {
                return BadRequest();
            }

            await _dbProvider.ChangeStatus(task.Id, TaskStatusEnum.Done);

            return Ok();
        }

        /// <summary>
        /// Marks a task as not completed
        /// </summary>
        /// <param name="id">Record ID</param>
        /// <returns>Status</returns>
        [HttpPut("notComplete/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        public async Task<IActionResult> MarkNotComplete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var task = await _dbProvider.Requests.FirstOrDefaultAsync(m => m.Id == id);

            if (task is null)
            {
                return BadRequest();
            }

            await _dbProvider.ChangeStatus(task.Id, TaskStatusEnum.New);

            return Ok();
        }

        /// <summary>
        /// Deletes a record
        /// </summary>
        /// <param name="id">Record ID</param>
        /// <returns>Status</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ActionResult<DoTask>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            await _dbProvider.Delete(id);

            return Ok();
        }
    }
}
