namespace ToDoR.Common.Contracts
{
    /// <summary>
    /// User
    /// </summary>
    /// <remarks>Not used</remarks>
    public partial class User {

        public User()
        {
            this.Tasks = new List<DoTask>();
            OnCreated();
        }

        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string PasswordHash { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual DateTime? DeletedAt { get; set; }

        public virtual IList<DoTask> Tasks { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }
}
