namespace ToDoR.Common.Contracts
{
    /// <summary>
    /// Contract for change of record
    /// </summary>
    public class DoTaskEdit
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Task name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Task note
        /// </summary>
        public string? Note { get; set; }

        /// <summary>
        /// Deadline
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public TaskStatusEnum Status { get; set; }
    }
}
