-- Add a default user
INSERT INTO [dbo].[users]
           ([id]
           ,[name]
           ,[password_hash]
           ,[created_at]
           ,[deleted_at])
     VALUES
           ('00000000-0000-0000-0000-000000000000'
           ,'Grusha'
           ,'Password'
           ,GETDATE()
           ,null)
GO
