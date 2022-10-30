using ToDoR.Common.Contracts;

namespace ToDoR.DataAccess.Interfaces
{
    /// <summary>
    /// ToDo list interface
    /// </summary>
    public interface IDoTaskProvider
    {
        /// <summary>
        /// Get data using conditions
        /// </summary>
        IQueryable<DoTask> Requests { get; }

        /// <summary>
        /// Adds a job to the database
        /// </summary>
        /// <param name="request">A Task</param>
        Task Add(DoTaskAdd request);

        /// <summary>
        /// Updates a task in the database
        /// </summary>
        /// <param name="update">New Task Values</param>
        Task Update(DoTaskEdit update);

        /// <summary>
        /// Deletes a task in the database
        /// </summary>
        /// <param name="id">Task identifier</param>
        Task Delete(Guid id);

        /// <summary>
        /// Changes the status for the current task
        /// </summary>
        /// <param name="id">Task identifier</param>
        /// <param name="status">New status</param>
        Task ChangeStatus(Guid id, TaskStatusEnum status);
    }
}