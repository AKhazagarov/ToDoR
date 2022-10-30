using ToDoR.Common.Contracts;

namespace ToDoR.Api.Models
{
    public class TaskFilterModel
    {
        /// <summary>
        /// Status list
        /// </summary>
        public List<TaskStatusEnum> Status { get; set; }

        /// <summary>
        /// Show deleted items
        /// </summary>
        public bool ShowDeleted { get; set; } = false;
    }
}