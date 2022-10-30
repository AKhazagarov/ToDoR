namespace ToDoR.Common.Contracts
{
    /// <summary>
    /// Contract for adding a new entity to the database
    /// </summary>
    public class DoTaskAdd
    {
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
    }
}
