ALTER TABLE [dbo].[Employees] ADD
    [ProfileImagePath] nvarchar(max) NULL,
    [IsActive] bit NOT NULL CONSTRAINT [DF_Employees_IsActive] DEFAULT 1,
    [IsDeleted] bit NOT NULL CONSTRAINT [DF_Employees_IsDeleted] DEFAULT 0,
    [AddedOn] datetime2 NOT NULL CONSTRAINT [DF_Employees_AddedOn] DEFAULT SYSUTCDATETIME(),
    [AddedBy] int NULL,
    [ModifiedOn] datetime2 NULL,
    [ModifiedBy] int NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] int NULL;

ALTER TABLE [dbo].[Departments] ADD
    [IsActive] bit NOT NULL CONSTRAINT [DF_Departments_IsActive] DEFAULT 1,
    [IsDeleted] bit NOT NULL CONSTRAINT [DF_Departments_IsDeleted] DEFAULT 0,
    [AddedOn] datetime2 NOT NULL CONSTRAINT [DF_Departments_AddedOn] DEFAULT SYSUTCDATETIME(),
    [AddedBy] int NULL,
    [ModifiedOn] datetime2 NULL,
    [ModifiedBy] int NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] int NULL;

ALTER TABLE [dbo].[Clients] ADD
    [IsActive] bit NOT NULL CONSTRAINT [DF_Clients_IsActive] DEFAULT 1,
    [IsDeleted] bit NOT NULL CONSTRAINT [DF_Clients_IsDeleted] DEFAULT 0,
    [AddedOn] datetime2 NOT NULL CONSTRAINT [DF_Clients_AddedOn] DEFAULT SYSUTCDATETIME(),
    [AddedBy] int NULL,
    [ModifiedOn] datetime2 NULL,
    [ModifiedBy] int NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] int NULL;

ALTER TABLE [dbo].[Interns] ADD
    [IsActive] bit NOT NULL CONSTRAINT [DF_Interns_IsActive] DEFAULT 1,
    [IsDeleted] bit NOT NULL CONSTRAINT [DF_Interns_IsDeleted] DEFAULT 0,
    [AddedOn] datetime2 NOT NULL CONSTRAINT [DF_Interns_AddedOn] DEFAULT SYSUTCDATETIME(),
    [AddedBy] int NULL,
    [ModifiedOn] datetime2 NULL,
    [ModifiedBy] int NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] int NULL;

ALTER TABLE [dbo].[AssignedTasks] ADD
    [IsActive] bit NOT NULL CONSTRAINT [DF_AssignedTasks_IsActive] DEFAULT 1,
    [IsDeleted] bit NOT NULL CONSTRAINT [DF_AssignedTasks_IsDeleted] DEFAULT 0,
    [AddedOn] datetime2 NOT NULL CONSTRAINT [DF_AssignedTasks_AddedOn] DEFAULT SYSUTCDATETIME(),
    [AddedBy] int NULL,
    [ModifiedOn] datetime2 NULL,
    [ModifiedBy] int NULL,
    [DeletedOn] datetime2 NULL,
    [DeletedBy] int NULL;
