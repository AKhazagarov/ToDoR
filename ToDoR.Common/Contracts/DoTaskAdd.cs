namespace ToDoR.Common.Contracts
{
    public class DoTaskAdd
    {
        public Guid? UserId { get; set; }

        public Guid? TaskGroupId { get; set; }

        public string Name { get; set; }

        public string? Note { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
