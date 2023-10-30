drop table [dbo].[Departments]
drop table [dbo].[Clients]
drop table [dbo].[Accounts]
drop table [dbo].[Deposits]

create table [dbo].[Departments]
(
	[Id] int not null primary key identity, 
	[ParentId] int not null,
	[Name] nvarchar(50)
)

create table [dbo].[Clients]
(
	[Id] int not null primary key identity, 
	[DepartmentId] int not null, 
	[Name] nvarchar(50) not null
)

create table [dbo].[Accounts]
(
	[Id] int not null primary key identity,
	[ClientId] int not null,
	[Balance] decimal (10, 4) not null, 
	[CreateDate] datetime not null
)

create table [dbo].[Deposits]
(
	[Id] int not null primary key identity,
	[ClientId] int not null,
	[Name] nvarchar(50),
	[Balance] decimal(10,4),
	[CreateDate] datetime not null,
	[IsWithCapitalization] bit not null
)


set identity_insert [dbo].[Departments] on;

insert into [dbo].[Departments] ([Id], [ParentId], [Name]) 
values (1, 0, N'Департамент_01'),
(2, 0, N'Департамент_02'),
(3, 1, N'Департамент_03');

set identity_insert [dbo].[Departments] off;

set identity_insert [dbo].[Clients] on;

insert into [dbo].[Clients] ([Id], [DepartmentId], [Name]) 
values (1, 1, N'Клиент_01'),
(2, 1, N'Клиент_02'),
(3, 2, N'Клиент_03');

set identity_insert [dbo].[Clients] off;

set identity_insert [dbo].[Accounts] on;

insert into [dbo].[Accounts] ([Id], [ClientId], [Balance], [CreateDate]) 
values (1, 1, 10000,  GETDATE()),
(2, 2, 10000,  GETDATE()),
(3, 3, 10000,  GETDATE());

set identity_insert [dbo].[Accounts] off;

set identity_insert [dbo].[Deposits] on;

insert into [dbo].[Deposits] ([Id], [ClientId], [Name], [Balance], [CreateDate], [IsWithCapitalization]) 
values (1, 1, N'Вклад открытый 3 месяца назад', 1000, DATEADD(MONTH,-3, GETDATE()), 0),
(2, 1, N'Вклад открытый 12 месяцев назад', 1000, DATEADD(MONTH,-12, GETDATE()), 0),
(3, 1, N'С капитализацией открытый 3 месяца назад', 1000, DATEADD(MONTH,-3, GETDATE()), 1),
(4, 1, N'С капитализацией открытый 6 месяца назад', 1000, DATEADD(MONTH,-6, GETDATE()), 1);

set identity_insert [dbo].[Deposits] off;

--select * from [dbo].[Departments]