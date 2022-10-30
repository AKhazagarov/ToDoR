using System.Data.Entity;
using ToDoR.Common.Contracts;
using ToDoR.DataAccess.Context;
using ToDoR.DataAccess.Interfaces;

namespace ToDoR.DataAccess.Providers
{
    public class DoTaskProvider : IDoTaskProvider
    {
        private readonly ToDoDbContext _context;

        public DoTaskProvider(ToDoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<DoTask> Requests
        {
            get
            {
                return _context.DoTasks.AsNoTracking();
            }
        }

        public async Task Add(DoTaskAdd request)
        {
            var newTask = new DoTask()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Name = request.Name,
                Note = request.Note,
                DueDate = request.DueDate,
                Status = TaskStatusEnum.New,
                CreatedAt = DateTime.Now,
            };

            await _context.DoTasks.AddAsync(newTask);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeStatus(Guid id, TaskStatusEnum status)
        {
            var task = await _context.DoTasks.FindAsync(id);

            if (task != null)
            {
                task.Status = status;
                _context.Update(task);

                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(Guid id)
        {
            var task = await _context.DoTasks.FindAsync(id);

            if (task != null)
            {
                task.DeletedAt = DateTime.Now;
                _context.Update(task);

                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(DoTaskEdit update)
        {
            var task = await _context.DoTasks.FindAsync(update.Id);

            if (task != null)
            {
                task.Name = update.Name;
                task.Note = update.Note;
                task.DueDate = update.DueDate;
                task.Status = update.Status;

                _context.Update(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}