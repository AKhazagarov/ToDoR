namespace ToDoR.Common.Contracts
{
    public partial class TaskGroup {

        public TaskGroup()
        {
            this.Tasks = new List<DoTask>();
            OnCreated();
        }

        public virtual Guid Id { get; set; }

        public virtual Guid UserId { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<DoTask> Tasks { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
