namespace ToDoR.Common.Contracts
{
    public partial class DoTask {

        public DoTask()
        {
            OnCreated();
        }

        public virtual Guid Id { get; set; }

        public virtual Guid UserId { get; set; }

        public virtual Guid? TaskGroupId { get; set; }

        public virtual string Name { get; set; }

        public virtual string? Note { get; set; }

        public virtual DateTime? DueDate { get; set; }

        public virtual TaskStatusEnum Status { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual DateTime? DeletedAt { get; set; }

        public virtual User Users { get; set; }

        public virtual TaskGroup TaskGroups { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
