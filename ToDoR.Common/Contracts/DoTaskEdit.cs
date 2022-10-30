namespace ToDoR.Common.Contracts
{
    public class DoTaskEdit
    {
        public Guid Id { get; set; }

        public Guid? TaskGroupId { get; set; }

        public string Name { get; set; }

        public string? Note { get; set; }

        public DateTime? DueDate { get; set; }

        public TaskStatusEnum Status { get; set; }
    }
}
