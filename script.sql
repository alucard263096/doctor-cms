USE [Doctor]
GO
/****** Object:  UserDefinedFunction [dbo].[checkUserTrans]    Script Date: 12/20/2013 11:20:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[checkUserTrans]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[checkUserTrans]
GO
/****** Object:  StoredProcedure [dbo].[sp_get_master_id]    Script Date: 12/20/2013 11:20:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_get_master_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_get_master_id]
GO
/****** Object:  Table [dbo].[tb_event]    Script Date: 12/20/2013 11:20:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_event]') AND type in (N'U'))
DROP TABLE [dbo].[tb_event]
GO
/****** Object:  Table [dbo].[tb_function]    Script Date: 12/20/2013 11:20:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_function]') AND type in (N'U'))
DROP TABLE [dbo].[tb_function]
GO
/****** Object:  Table [dbo].[tb_seq]    Script Date: 12/20/2013 11:20:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_seq]') AND type in (N'U'))
DROP TABLE [dbo].[tb_seq]
GO
/****** Object:  Table [dbo].[tb_user]    Script Date: 12/20/2013 11:20:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_user]') AND type in (N'U'))
DROP TABLE [dbo].[tb_user]
GO
/****** Object:  Table [dbo].[tb_user_function]    Script Date: 12/20/2013 11:20:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_user_function]') AND type in (N'U'))
DROP TABLE [dbo].[tb_user_function]
GO
/****** Object:  Table [dbo].[tb_user_function]    Script Date: 12/20/2013 11:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_user_function]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tb_user_function](
	[user_id] [int] NOT NULL,
	[function_id] [int] NOT NULL,
	[status] [smallint] NOT NULL,
	[created_user] [int] NOT NULL,
	[created_date] [datetime] NOT NULL,
 CONSTRAINT [PK_tb_user_function] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC,
	[function_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
INSERT [dbo].[tb_user_function] ([user_id], [function_id], [status], [created_user], [created_date]) VALUES (1, 1, 0, 1, CAST(0x00009D680115A51C AS DateTime))
INSERT [dbo].[tb_user_function] ([user_id], [function_id], [status], [created_user], [created_date]) VALUES (1, 2, 0, 1, CAST(0x00009D680115A51C AS DateTime))
INSERT [dbo].[tb_user_function] ([user_id], [function_id], [status], [created_user], [created_date]) VALUES (1, 3, 0, 1, CAST(0x00009D680115A51C AS DateTime))
INSERT [dbo].[tb_user_function] ([user_id], [function_id], [status], [created_user], [created_date]) VALUES (1, 4, 0, 1, CAST(0x00009D680115A51C AS DateTime))
INSERT [dbo].[tb_user_function] ([user_id], [function_id], [status], [created_user], [created_date]) VALUES (1, 5, 0, 1, CAST(0x00009D680115A51C AS DateTime))
INSERT [dbo].[tb_user_function] ([user_id], [function_id], [status], [created_user], [created_date]) VALUES (1, 6, 0, 1, CAST(0x00009D680115A51C AS DateTime))
INSERT [dbo].[tb_user_function] ([user_id], [function_id], [status], [created_user], [created_date]) VALUES (1, 7, 0, 1, CAST(0x00009D680115A51C AS DateTime))
INSERT [dbo].[tb_user_function] ([user_id], [function_id], [status], [created_user], [created_date]) VALUES (1, 8, 0, 1, CAST(0x00009D680115A51C AS DateTime))
INSERT [dbo].[tb_user_function] ([user_id], [function_id], [status], [created_user], [created_date]) VALUES (3, 2, 0, 1, CAST(0x00009D57011DBCAC AS DateTime))
INSERT [dbo].[tb_user_function] ([user_id], [function_id], [status], [created_user], [created_date]) VALUES (3, 4, 0, 1, CAST(0x00009D57011DBCAC AS DateTime))
INSERT [dbo].[tb_user_function] ([user_id], [function_id], [status], [created_user], [created_date]) VALUES (3, 6, 0, 1, CAST(0x00009D57011DBCAC AS DateTime))
/****** Object:  Table [dbo].[tb_user]    Script Date: 12/20/2013 11:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_user]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tb_user](
	[user_id] [int] NOT NULL,
	[login_id] [nvarchar](12) NOT NULL,
	[password] [nvarchar](500) NOT NULL,
	[user_name] [nvarchar](50) NOT NULL,
	[email] [nvarchar](100) NOT NULL,
	[remarks] [nvarchar](1000) NULL,
	[status] [smallint] NOT NULL,
	[created_user] [int] NOT NULL,
	[created_date] [datetime] NOT NULL,
	[updated_user] [int] NOT NULL,
	[updated_date] [datetime] NOT NULL,
 CONSTRAINT [PK_tb_user] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
INSERT [dbo].[tb_user] ([user_id], [login_id], [password], [user_name], [email], [remarks], [status], [created_user], [created_date], [updated_user], [updated_date]) VALUES (1, N'admin', N'21232F297A57A5A743894AE4A801FC3', N'System Administrator', N'sunyam@tedgeco.com', N'', 0, 1, CAST(0x00009D5700F608DB AS DateTime), 1, CAST(0x00009D680115A505 AS DateTime))
INSERT [dbo].[tb_user] ([user_id], [login_id], [password], [user_name], [email], [remarks], [status], [created_user], [created_date], [updated_user], [updated_date]) VALUES (2, N'onlinereg', N'21232F297A57A5A743894AE4A801FC3', N'Online Registration', N'sunyam@tedgeco.com', NULL, 0, 1, CAST(0x00009D5700F608DB AS DateTime), 1, CAST(0x00009D5700F608DB AS DateTime))
INSERT [dbo].[tb_user] ([user_id], [login_id], [password], [user_name], [email], [remarks], [status], [created_user], [created_date], [updated_user], [updated_date]) VALUES (3, N'cccc', N'D1696816BC1A7AFE92F1C8787AC222C3', N'bbbbbbbbbbbbbbb', N'alucard263096@126.com', N'fffffffffff', 0, 1, CAST(0x00009D57011A1FEF AS DateTime), 1, CAST(0x00009D57015C30FB AS DateTime))
/****** Object:  Table [dbo].[tb_seq]    Script Date: 12/20/2013 11:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_seq]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tb_seq](
	[table_name] [nchar](100) NULL,
	[curval] [int] NULL
) ON [PRIMARY]
END
GO
INSERT [dbo].[tb_seq] ([table_name], [curval]) VALUES (N'tb_event                                                                                            ', 1)
INSERT [dbo].[tb_seq] ([table_name], [curval]) VALUES (N'tb_doctor                                                                                           ', 1)
/****** Object:  Table [dbo].[tb_function]    Script Date: 12/20/2013 11:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_function]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tb_function](
	[function_id] [int] NOT NULL,
	[function_name] [nvarchar](50) NOT NULL,
	[function_link] [nvarchar](100) NOT NULL,
	[parent_id] [int] NOT NULL,
	[function_type] [smallint] NOT NULL,
	[function_group] [int] NOT NULL,
	[seq] [int] NOT NULL,
	[status] [smallint] NOT NULL,
 CONSTRAINT [PK_tb_function] PRIMARY KEY CLUSTERED 
(
	[function_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
INSERT [dbo].[tb_function] ([function_id], [function_name], [function_link], [parent_id], [function_type], [function_group], [seq], [status]) VALUES (1, N'活动管理', N'event_list.aspx', 0, 9, 1, 10, 0)
INSERT [dbo].[tb_function] ([function_id], [function_name], [function_link], [parent_id], [function_type], [function_group], [seq], [status]) VALUES (2, N'医生管理', N'doctor_list.aspx', 0, 9, 2, 20, 0)
INSERT [dbo].[tb_function] ([function_id], [function_name], [function_link], [parent_id], [function_type], [function_group], [seq], [status]) VALUES (3, N'会员管理', N'member_list.aspx', 0, 9, 3, 30, 0)
INSERT [dbo].[tb_function] ([function_id], [function_name], [function_link], [parent_id], [function_type], [function_group], [seq], [status]) VALUES (6, N'用户管理', N'user_list.aspx', 0, 9, 6, 60, 0)
/****** Object:  Table [dbo].[tb_event]    Script Date: 12/20/2013 11:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_event]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tb_event](
	[eventId] [int] NOT NULL,
	[title] [nvarchar](300) NOT NULL,
	[summary] [nvarchar](300) NOT NULL,
	[content] [nvarchar](3000) NOT NULL,
	[imageUrl] [nvarchar](300) NOT NULL,
	[publishedDate] [datetime] NOT NULL,
	[status] [int] NOT NULL,
	[created_user] [int] NOT NULL,
	[created_date] [datetime] NOT NULL,
	[updated_user] [int] NOT NULL,
	[updated_date] [datetime] NOT NULL,
 CONSTRAINT [PK_tb_event] PRIMARY KEY CLUSTERED 
(
	[eventId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  StoredProcedure [dbo].[sp_get_master_id]    Script Date: 12/20/2013 11:20:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_get_master_id]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_get_master_id]
@table_name varchar(100)	
AS
BEGIN
	declare @select_count int
	select @select_count=count(*) from tb_seq where table_name=@table_name
	
	IF @select_count=0
	BEGIN
		insert into tb_seq values (@table_name,0)
	END
	
	update tb_seq set curval=curval+1 where table_name=@table_name
	
	select curval from tb_seq where table_name=@table_name
	
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[checkUserTrans]    Script Date: 12/20/2013 11:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[checkUserTrans]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'Create FUNCTION [dbo].[checkUserTrans]
(
 -- Add the parameters for the function here
 @user_id int
)
RETURNS int --0 for Don''t Have, geater than 0 Have
AS
BEGIN
 -- Declare the return variable here
 DECLARE @count int
 -- Add the T-SQL statements to compute the return value here
 SELECT @count = COUNT(*) FROM tb_user WHERE created_user=@user_id OR updated_user=@user_id
 if (@count > 0)
  return @count
 SELECT @count = COUNT(*) FROM tb_user_function WHERE created_user=@user_id
 if (@count > 0)
  return @count
 -- Return the result of the function
 RETURN @count
END
' 
END
GO
