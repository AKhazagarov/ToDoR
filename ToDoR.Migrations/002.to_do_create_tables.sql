-- Create a necessary tables
CREATE TABLE task_groups (
   id UNIQUEIDENTIFIER NOT NULL,
   user_id UNIQUEIDENTIFIER NOT NULL,
   name NVARCHAR(4000) NOT NULL,
   CONSTRAINT PK_task_groups PRIMARY KEY (id)
)
GO

CREATE TABLE users (
   id UNIQUEIDENTIFIER NOT NULL,
   name NVARCHAR(4000) NOT NULL,
   password_hash NVARCHAR(4000) NOT NULL,
   created_at DATETIME2 NOT NULL,
   deleted_at DATETIME2,
   CONSTRAINT PK_users PRIMARY KEY (id)
)
GO

CREATE TABLE do_tasks (
   id UNIQUEIDENTIFIER NOT NULL,
   user_id UNIQUEIDENTIFIER,
   task_group_id UNIQUEIDENTIFIER,
   name NVARCHAR(4000) NOT NULL,
   note NVARCHAR(4000),
   due_date DATETIME2,
   status INT,
   created_at DATETIME2 NOT NULL,
   deleted_at DATETIME2,
   CONSTRAINT PK_do_tasks PRIMARY KEY (id),
   CONSTRAINT FK_do_tasks_users_0 FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE,
   CONSTRAINT FK_do_tasks_task_groups_1 FOREIGN KEY (task_group_id) REFERENCES task_groups (id)
)
GO

