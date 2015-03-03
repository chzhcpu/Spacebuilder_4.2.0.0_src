IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spt_TopicMemberApplies_ApplyReason]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spt_TopicMemberApplies] DROP CONSTRAINT [DF_spt_TopicMemberApplies_ApplyReason]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spt_TopicMemberApplies]') AND type in (N'U'))
DROP TABLE [dbo].[spt_TopicMemberApplies]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spt_TopicMembers_IsManager]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spt_TopicMembers] DROP CONSTRAINT [DF_spt_TopicMembers_IsManager]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spt_TopicMembers]') AND type in (N'U'))
DROP TABLE [dbo].[spt_TopicMembers]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spt_Topics_TopicName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spt_Topics] DROP CONSTRAINT [DF_spt_Topics_TopicName]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spt_Topics_TopicKey]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spt_Topics] DROP CONSTRAINT [DF_spt_Topics_TopicKey]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spt_Topics_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spt_Topics] DROP CONSTRAINT [DF_spt_Topics_Description]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spt_Topics_UserId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spt_Topics] DROP CONSTRAINT [DF_spt_Topics_UserId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spt_Topics_Logo]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spt_Topics] DROP CONSTRAINT [DF_spt_Topics_Logo]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spt_Topics_IsPublic]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spt_Topics] DROP CONSTRAINT [DF_spt_Topics_IsPublic]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spt_Topics_EnableMemberInvite]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spt_Topics] DROP CONSTRAINT [DF_spt_Topics_EnableMemberInvite]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spt_Topics_AuditStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spt_Topics] DROP CONSTRAINT [DF_spt_Topics_AuditStatus]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spt_Topics_MemberCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spt_Topics] DROP CONSTRAINT [DF_spt_Topics_MemberCount]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spt_Topics_GrowthValue]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spt_Topics] DROP CONSTRAINT [DF_spt_Topics_GrowthValue]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spt_Topics_ThemeAppearance]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spt_Topics] DROP CONSTRAINT [DF_spt_Topics_ThemeAppearance]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spt_Topics_IP]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spt_Topics] DROP CONSTRAINT [DF_spt_Topics_IP]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spt_Topics_Announcement]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spt_Topics] DROP CONSTRAINT [DF_spt_Topics_Announcement]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spt_Topics_IsUseCustomStyle]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spt_Topics] DROP CONSTRAINT [DF_spt_Topics_IsUseCustomStyle]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spt_Topics]') AND type in (N'U'))
DROP TABLE [dbo].[spt_Topics]
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spt_Topics]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spt_Topics](
	[TopicId] [bigint] NOT NULL,
	[TopicName] [nvarchar](255) NOT NULL CONSTRAINT [DF_spt_Topics_TopicName]  DEFAULT (''),
	[TopicKey] [nvarchar](16) NOT NULL CONSTRAINT [DF_spt_Topics_TopicKey]  DEFAULT (''),
	[Description] [nvarchar](512) NOT NULL CONSTRAINT [DF_spt_Topics_Description]  DEFAULT (''),
	[AreaCode] [varchar](8) NOT NULL,
	[UserId] [bigint] NOT NULL CONSTRAINT [DF_spt_Topics_UserId]  DEFAULT ((0)),
	[Logo] [nvarchar](128) NOT NULL CONSTRAINT [DF_spt_Topics_Logo]  DEFAULT (''),
	[IsPublic] [tinyint] NOT NULL CONSTRAINT [DF_spt_Topics_IsPublic]  DEFAULT ((1)),
	[JoinWay] [smallint] NOT NULL,
	[EnableMemberInvite] [tinyint] NOT NULL CONSTRAINT [DF_spt_Topics_EnableMemberInvite]  DEFAULT ((1)),
	[AuditStatus] [smallint] NOT NULL CONSTRAINT [DF_spt_Topics_AuditStatus]  DEFAULT ((40)),
	[MemberCount] [int] NOT NULL CONSTRAINT [DF_spt_Topics_MemberCount]  DEFAULT ((0)),
	[GrowthValue] [int] NOT NULL CONSTRAINT [DF_spt_Topics_GrowthValue]  DEFAULT ((0)),
	[ThemeAppearance] [nvarchar](128) NOT NULL CONSTRAINT [DF_spt_Topics_ThemeAppearance]  DEFAULT (''),
	[DateCreated] [datetime] NOT NULL,
	[IP] [nvarchar](64) NOT NULL CONSTRAINT [DF_spt_Topics_IP]  DEFAULT (''),
	[Announcement] [nvarchar](512) NOT NULL CONSTRAINT [DF_spt_Topics_Announcement]  DEFAULT (''),
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
	[IsUseCustomStyle] [tinyint] NOT NULL CONSTRAINT [DF_spt_Topics_IsUseCustomStyle]  DEFAULT ((0)),
 CONSTRAINT [PK_spt_Topics] PRIMARY KEY CLUSTERED 
(
	[TopicId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spt_Topics]') AND name = N'IX_spt_Topics_AuditStatus')
CREATE NONCLUSTERED INDEX [IX_spt_Topics_AuditStatus] ON [dbo].[spt_Topics] 
(
	[AuditStatus] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spt_Topics]') AND name = N'IX_spt_Topics_GrowthValue')
CREATE NONCLUSTERED INDEX [IX_spt_Topics_GrowthValue] ON [dbo].[spt_Topics] 
(
	[GrowthValue] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spt_Topics]') AND name = N'IX_spt_Topics_MemberCount')
CREATE NONCLUSTERED INDEX [IX_spt_Topics_MemberCount] ON [dbo].[spt_Topics] 
(
	[MemberCount] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spt_Topics]') AND name = N'IX_spt_Topics_UserId')
CREATE NONCLUSTERED INDEX [IX_spt_Topics_UserId] ON [dbo].[spt_Topics] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_Topics', N'COLUMN',N'TopicName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'专题名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_Topics', @level2type=N'COLUMN',@level2name=N'TopicName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_Topics', N'COLUMN',N'TopicKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'专题标识（个性网址的关键组成部分）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_Topics', @level2type=N'COLUMN',@level2name=N'TopicKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_Topics', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'专题介绍' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_Topics', @level2type=N'COLUMN',@level2name=N'Description'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_Topics', N'COLUMN',N'AreaCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所在地区' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_Topics', @level2type=N'COLUMN',@level2name=N'AreaCode'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_Topics', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'群主' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_Topics', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_Topics', N'COLUMN',N'Logo'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'logo名称（带部分路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_Topics', @level2type=N'COLUMN',@level2name=N'Logo'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_Topics', N'COLUMN',N'IsPublic'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否公开' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_Topics', @level2type=N'COLUMN',@level2name=N'IsPublic'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_Topics', N'COLUMN',N'JoinWay'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加入方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_Topics', @level2type=N'COLUMN',@level2name=N'JoinWay'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_Topics', N'COLUMN',N'EnableMemberInvite'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否允许成员邀请（一直允许群管理员邀请）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_Topics', @level2type=N'COLUMN',@level2name=N'EnableMemberInvite'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_Topics', N'COLUMN',N'AuditStatus'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_Topics', @level2type=N'COLUMN',@level2name=N'AuditStatus'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_Topics', N'COLUMN',N'MemberCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成员数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_Topics', @level2type=N'COLUMN',@level2name=N'MemberCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_Topics', N'COLUMN',N'GrowthValue'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成长值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_Topics', @level2type=N'COLUMN',@level2name=N'GrowthValue'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_Topics', N'COLUMN',N'ThemeAppearance'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'设置的皮肤' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_Topics', @level2type=N'COLUMN',@level2name=N'ThemeAppearance'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_Topics', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_Topics', @level2type=N'COLUMN',@level2name=N'DateCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_Topics', N'COLUMN',N'IP'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_Topics', @level2type=N'COLUMN',@level2name=N'IP'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_Topics', N'COLUMN',N'Announcement'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公告' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_Topics', @level2type=N'COLUMN',@level2name=N'Announcement'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_Topics', N'COLUMN',N'IsUseCustomStyle'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否使用了自定义风格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_Topics', @level2type=N'COLUMN',@level2name=N'IsUseCustomStyle'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spt_TopicMembers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spt_TopicMembers](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TopicId] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[IsManager] [tinyint] NOT NULL CONSTRAINT [DF_spt_TopicMembers_IsManager]  DEFAULT ((0)),
	[JoinDate] [datetime] NOT NULL,
 CONSTRAINT [PK_spt_TopicMembers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spt_TopicMembers]') AND name = N'IK_spt_TopicMembers_TopicId')
CREATE NONCLUSTERED INDEX [IK_spt_TopicMembers_TopicId] ON [dbo].[spt_TopicMembers] 
(
	[TopicId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spt_TopicMembers]') AND name = N'IK_spt_TopicMembers_UserId')
CREATE NONCLUSTERED INDEX [IK_spt_TopicMembers_UserId] ON [dbo].[spt_TopicMembers] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_TopicMembers', N'COLUMN',N'TopicId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'专题Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_TopicMembers', @level2type=N'COLUMN',@level2name=N'TopicId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_TopicMembers', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_TopicMembers', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_TopicMembers', N'COLUMN',N'IsManager'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否群管理员' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_TopicMembers', @level2type=N'COLUMN',@level2name=N'IsManager'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_TopicMembers', N'COLUMN',N'JoinDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加入日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_TopicMembers', @level2type=N'COLUMN',@level2name=N'JoinDate'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spt_TopicMemberApplies]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spt_TopicMemberApplies](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TopicId] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ApplyReason] [nvarchar](255) NOT NULL CONSTRAINT [DF_spt_TopicMemberApplies_ApplyReason]  DEFAULT (''),
	[ApplyStatus] [smallint] NOT NULL,
	[ApplyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_spt_TopicMemberApplies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spt_TopicMemberApplies]') AND name = N'IK_spt_TopicMemberApplies_ApplyStatus')
CREATE NONCLUSTERED INDEX [IK_spt_TopicMemberApplies_ApplyStatus] ON [dbo].[spt_TopicMemberApplies] 
(
	[ApplyStatus] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spt_TopicMemberApplies]') AND name = N'IK_spt_TopicMemberApplies_TopicId_ApplyStatus')
CREATE NONCLUSTERED INDEX [IK_spt_TopicMemberApplies_TopicId_ApplyStatus] ON [dbo].[spt_TopicMemberApplies] 
(
	[TopicId] ASC,
	[ApplyStatus] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spt_TopicMemberApplies]') AND name = N'IK_spt_TopicMemberApplies_UserId')
CREATE NONCLUSTERED INDEX [IK_spt_TopicMemberApplies_UserId] ON [dbo].[spt_TopicMemberApplies] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_TopicMemberApplies', N'COLUMN',N'TopicId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'专题Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_TopicMemberApplies', @level2type=N'COLUMN',@level2name=N'TopicId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_TopicMemberApplies', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_TopicMemberApplies', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_TopicMemberApplies', N'COLUMN',N'ApplyReason'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请理由' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_TopicMemberApplies', @level2type=N'COLUMN',@level2name=N'ApplyReason'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_TopicMemberApplies', N'COLUMN',N'ApplyStatus'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_TopicMemberApplies', @level2type=N'COLUMN',@level2name=N'ApplyStatus'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spt_TopicMemberApplies', N'COLUMN',N'ApplyDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spt_TopicMemberApplies', @level2type=N'COLUMN',@level2name=N'ApplyDate'
