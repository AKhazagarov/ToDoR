using ToDoR.Common.Contracts;

namespace ToDoR.Api.Models
{
    public class TaskModel
    {
        public Guid Id { get; set; }

        public Guid? TaskGroup { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        public DateTime? DueDate { get; set; }

        public TaskStatusEnum Status { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
