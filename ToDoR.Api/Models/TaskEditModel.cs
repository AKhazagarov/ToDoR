namespace ToDoR.Api.Models
{
    public class TaskEditModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Note { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
