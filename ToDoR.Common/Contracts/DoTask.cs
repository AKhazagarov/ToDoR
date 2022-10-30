namespace ToDoR.Common.Contracts
{
    /// <summary>
    /// To-do list entry
    /// </summary>
    public partial class DoTask
    {
        public DoTask()
        {
            OnCreated();
        }

        /// <summary>
        /// Identifier
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        public virtual Guid UserId { get; set; }

        /// <summary>
        /// Group ID
        /// </summary>
        public virtual Guid? TaskGroupId { get; set; }

        /// <summary>
        /// Task name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Task note
        /// </summary>
        public virtual string? Note { get; set; }

        /// <summary>
        /// Deadline
        /// </summary>
        public virtual DateTime? DueDate { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public virtual TaskStatusEnum Status { get; set; }

        /// <summary>
        /// Record creation date
        /// </summary>
        public virtual DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date the record was deleted
        /// </summary>
        public virtual DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Link to user
        /// </summary>
        /// <remarks>Not used</remarks>
        public virtual User Users { get; set; }

        /// <summary>
        /// Link to user Task Group
        /// </summary>
        /// <remarks>Not used</remarks>
        public virtual TaskGroup TaskGroups { get; set; }

        #region Extensibility Method Definitions
        partial void OnCreated();
        #endregion
    }
}
