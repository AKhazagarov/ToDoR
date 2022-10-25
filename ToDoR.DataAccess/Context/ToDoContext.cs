﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 25.10.2022 9:03:05
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ToDoR.DataAccess.Context
{

    public partial class ToDoContext : DbContext
    {

        public ToDoContext() :
            base()
        {
            OnCreated();
        }

        public ToDoContext(DbContextOptions<ToDoContext> options) :
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

        public virtual DbSet<ToDoR.Common.Contracts.Users> Users
        {
            get;
            set;
        }

        public virtual DbSet<ToDoR.Common.Contracts.TaskGroups> TaskGroups
        {
            get;
            set;
        }

        public virtual DbSet<ToDoR.Common.Contracts.Tasks> Tasks
        {
            get;
            set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            this.UsersMapping(modelBuilder);
            this.CustomizeUsersMapping(modelBuilder);

            this.TaskGroupsMapping(modelBuilder);
            this.CustomizeTaskGroupsMapping(modelBuilder);

            this.TasksMapping(modelBuilder);
            this.CustomizeTasksMapping(modelBuilder);

            RelationshipsMapping(modelBuilder);
            CustomizeMapping(ref modelBuilder);
        }

        #region Users Mapping

        private void UsersMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoR.Common.Contracts.Users>().ToTable(@"users");
            modelBuilder.Entity<ToDoR.Common.Contracts.Users>().Property(x => x.Id).HasColumnName(@"id").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.Users>().Property(x => x.Name).HasColumnName(@"name").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.Users>().Property(x => x.PasswordHash).HasColumnName(@"password_hash").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.Users>().Property(x => x.CreatedAt).HasColumnName(@"created_at").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.Users>().Property(x => x.DeletedAt).HasColumnName(@"deleted_at").ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.Users>().HasKey(@"Id");
        }

        partial void CustomizeUsersMapping(ModelBuilder modelBuilder);

        #endregion

        #region TaskGroups Mapping

        private void TaskGroupsMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoR.Common.Contracts.TaskGroups>().ToTable(@"task_groups");
            modelBuilder.Entity<ToDoR.Common.Contracts.TaskGroups>().Property(x => x.Id).HasColumnName(@"id").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.TaskGroups>().Property(x => x.UserId).HasColumnName(@"user_id").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.TaskGroups>().Property(x => x.Name).HasColumnName(@"name").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.TaskGroups>().HasKey(@"Id");
        }

        partial void CustomizeTaskGroupsMapping(ModelBuilder modelBuilder);

        #endregion

        #region Tasks Mapping

        private void TasksMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoR.Common.Contracts.Tasks>().ToTable(@"tasks");
            modelBuilder.Entity<ToDoR.Common.Contracts.Tasks>().Property(x => x.Id).HasColumnName(@"id").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.Tasks>().Property(x => x.UserId).HasColumnName(@"user_id").ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.Tasks>().Property(x => x.TaskGroupId).HasColumnName(@"task_group_id").ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.Tasks>().Property(x => x.Task).HasColumnName(@"task").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.Tasks>().Property(x => x.Note).HasColumnName(@"note").ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.Tasks>().Property(x => x.DueDate).HasColumnName(@"due_date").ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.Tasks>().Property(x => x.Repeat).HasColumnName(@"repeat").ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.Tasks>().Property(x => x.Status).HasColumnName(@"status").ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.Tasks>().Property(x => x.CreatedAt).HasColumnName(@"created_at").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.Tasks>().Property(x => x.DeletedAt).HasColumnName(@"deleted_at").ValueGeneratedNever();
            modelBuilder.Entity<ToDoR.Common.Contracts.Tasks>().HasKey(@"Id");
        }

        partial void CustomizeTasksMapping(ModelBuilder modelBuilder);

        #endregion

        private void RelationshipsMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoR.Common.Contracts.Users>().HasMany(x => x.Tasks).WithOne(op => op.Users).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"UserId").IsRequired(true);

            modelBuilder.Entity<ToDoR.Common.Contracts.TaskGroups>().HasMany(x => x.Tasks).WithOne(op => op.TaskGroups).HasForeignKey(@"TaskGroupId").IsRequired(true);

            modelBuilder.Entity<ToDoR.Common.Contracts.Tasks>().HasOne(x => x.Users).WithMany(op => op.Tasks).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"UserId").IsRequired(true);
            modelBuilder.Entity<ToDoR.Common.Contracts.Tasks>().HasOne(x => x.TaskGroups).WithMany(op => op.Tasks).HasForeignKey(@"TaskGroupId").IsRequired(true);
        }

        partial void CustomizeMapping(ref ModelBuilder modelBuilder);

        public bool HasChanges()
        {
            return ChangeTracker.Entries().Any(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Added || e.State == Microsoft.EntityFrameworkCore.EntityState.Modified || e.State == Microsoft.EntityFrameworkCore.EntityState.Deleted);
        }

        partial void OnCreated();
    }
}