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
        /// Returns all tasks
        /// </summary>
        /// <returns></returns>
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
                    TaskGroup = t.TaskGroupId,
                });

            return Ok(tasksModel);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ActionResult<TaskModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<TaskModel>> GetById(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasks = await _dbProvider.Requests.FirstOrDefaultAsync(m => m.Id == id);

            if (tasks == null)
            {
                return NotFound();
            }

            return Ok(tasks);
        }

        /// <summary>
        /// Returns all tasks
        /// </summary>
        /// <returns></returns>
        [HttpPost("byFilter")]
        [ProducesResponseType(typeof(ActionResult<List<TaskModel>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TaskModel>>> GetByFilter(TaslFilterModel filter)
        {
            var tasks = _dbProvider.Requests;

            if (filter.isDeleted != true)
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
                    TaskGroup = t.TaskGroupId,
                });

            return Ok(tasksModel);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        public async Task<IActionResult> Add(DoTaskAdd taskAdd)
        {
            if (taskAdd is null)
            {
                return BadRequest("Злёбне крыса");
            }

            if (taskAdd.Name.Length < 10)
            {
                return BadRequest("Minimum length 10 characters");
            }

            await _dbProvider.Add(taskAdd);

            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
                TaskGroupId = model.TaskGroupId,
                Name = model.Name,
                Note = model.Note,
                DueDate = model.DueDate,
            });

            return Ok();
        }

        [HttpPut("complete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> MarkComplete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var task = await _dbProvider.Requests.FirstOrDefaultAsync(m => m.Id == id);

            if (task is null)
            {
                return NotFound();
            }

            await _dbProvider.ChangeStatus(task.Id, TaskStatusEnum.Done);

            return Ok();
        }

        [HttpPut("notComplete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> MarkNotComplete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var task = await _dbProvider.Requests.FirstOrDefaultAsync(m => m.Id == id);

            if (task is null)
            {
                return NotFound();
            }

            await _dbProvider.ChangeStatus(task.Id, TaskStatusEnum.New);

            return Ok();
        }

        // GET: Tasks/Delete/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ActionResult<DoTask>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            await _dbProvider.Delete(id);

            return Ok();
        }
    }
}
