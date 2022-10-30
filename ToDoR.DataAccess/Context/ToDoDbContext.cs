using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ToDoR.DataAccess.Context
{

    public partial class ToDoDbContext : DbContext
    {

        public ToDoDbContext() :
            base()
        {
            OnCreated();
        }

        public ToDoDbContext(DbContextOptions<ToDoDbContext> options) :
            base(options)
        {
            OnCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured ||
                (!optionsBuilder.Options.Extensions.OfType<RelationalOptionsExtension>().Any(ext => !string.IsNullOrEmpty(ext.ConnectionString) || ext.Connection != null) &&
                 !optionsBuilder.Options.Extensions.Any(ext => !(ext is RelationalOptionsExtension) && !(ext is CoreOptionsExtension))))
            {
            }
            CustomizeConfiguration(ref optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }

        partial void CustomizeConfiguration(ref DbContextOptionsBuilder optionsBuilder);

        public virtual DbSet<Common.Contracts.User> Users
        {
            get;
            set;
        }

        public virtual DbSet<Common.Contracts.TaskGroup> TaskGroups
        {
            get;
            set;
        }

        public virtual DbSet<Common.Contracts.DoTask> DoTasks
        {
            get;
            set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            this.UserMapping(modelBuilder);
            this.CustomizeUserMapping(modelBuilder);

            this.TaskGroupMapping(modelBuilder);
            this.CustomizeTaskGroupMapping(modelBuilder);

            this.DoTaskMapping(modelBuilder);
            this.CustomizeDoTaskMapping(modelBuilder);

            RelationshipsMapping(modelBuilder);
            CustomizeMapping(ref modelBuilder);
        }

        #region User Mapping

        private void UserMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoR.Common.Contracts.User>().ToTable(@"users");
            modelBuilder.Entity<ToDoR.Common.Contracts.User>().Property(x => x.Id).HasColumnName(@"id").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.User>().Property(x => x.Name).HasColumnName(@"name").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.User>().Property(x => x.PasswordHash).HasColumnName(@"password_hash").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.User>().Property(x => x.CreatedAt).HasColumnName(@"created_at").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.User>().Property(x => x.DeletedAt).HasColumnName(@"deleted_at").ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.User>().HasKey(@"Id");
        }

        partial void CustomizeUserMapping(ModelBuilder modelBuilder);

        #endregion

        #region TaskGroup Mapping

        private void TaskGroupMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoR.Common.Contracts.TaskGroup>().ToTable(@"task_groups");
            modelBuilder.Entity<ToDoR.Common.Contracts.TaskGroup>().Property(x => x.Id).HasColumnName(@"id").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.TaskGroup>().Property(x => x.UserId).HasColumnName(@"user_id").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.TaskGroup>().Property(x => x.Name).HasColumnName(@"name").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.TaskGroup>().HasKey(@"Id");
        }

        partial void CustomizeTaskGroupMapping(ModelBuilder modelBuilder);

        #endregion

        #region DoTask Mapping

        private void DoTaskMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoR.Common.Contracts.DoTask>().ToTable(@"do_tasks");
            modelBuilder.Entity<ToDoR.Common.Contracts.DoTask>().Property(x => x.Id).HasColumnName(@"id").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.DoTask>().Property(x => x.UserId).HasColumnName(@"user_id").ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.DoTask>().Property(x => x.TaskGroupId).HasColumnName(@"task_group_id").ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.DoTask>().Property(x => x.Name).HasColumnName(@"name").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.DoTask>().Property(x => x.Note).HasColumnName(@"note").ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.DoTask>().Property(x => x.DueDate).HasColumnName(@"due_date").ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.DoTask>().Property(x => x.Status).HasColumnName(@"status").ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.DoTask>().Property(x => x.CreatedAt).HasColumnName(@"created_at").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.DoTask>().Property(x => x.DeletedAt).HasColumnName(@"deleted_at").ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.DoTask>().HasKey(@"Id");
        }

        partial void CustomizeDoTaskMapping(ModelBuilder modelBuilder);

        #endregion

        private void RelationshipsMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoR.Common.Contracts.User>().HasMany(x => x.Tasks).WithOne(op => op.Users).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"UserId").IsRequired(true);

            modelBuilder.Entity<ToDoR.Common.Contracts.TaskGroup>().HasMany(x => x.Tasks).WithOne(op => op.TaskGroups).HasForeignKey(@"TaskGroupId").IsRequired(false);

            modelBuilder.Entity<ToDoR.Common.Contracts.DoTask>().HasOne(x => x.Users).WithMany(op => op.Tasks).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"UserId").IsRequired(true);
            modelBuilder.Entity<ToDoR.Common.Contracts.DoTask>().HasOne(x => x.TaskGroups).WithMany(op => op.Tasks).HasForeignKey(@"TaskGroupId").IsRequired(false);
        }

        partial void CustomizeMapping(ref ModelBuilder modelBuilder);

        public bool HasChanges()
        {
            return ChangeTracker.Entries().Any(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Added || e.State == Microsoft.EntityFrameworkCore.EntityState.Modified || e.State == Microsoft.EntityFrameworkCore.EntityState.Deleted);
        }

        partial void OnCreated();
    }
}
