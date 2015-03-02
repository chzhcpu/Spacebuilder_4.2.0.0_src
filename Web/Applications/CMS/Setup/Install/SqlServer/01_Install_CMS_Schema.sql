/****** Object:  Table [dbo].[spb_cms_Addon_Links]    Script Date: 07/26/2013 15:08:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_cms_Addon_Links]') AND type in (N'U'))
DROP TABLE [dbo].[spb_cms_Addon_Links]
GO
/****** Object:  Table [dbo].[spb_cms_Addon_News]    Script Date: 07/26/2013 15:08:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_cms_Addon_News]') AND type in (N'U'))
DROP TABLE [dbo].[spb_cms_Addon_News]
GO
/****** Object:  Table [dbo].[spb_cms_ContentAttachments]    Script Date: 07/26/2013 15:08:51 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_cms_ContentAttachments_FileName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_cms_ContentAttachments] DROP CONSTRAINT [DF_spb_cms_ContentAttachments_FileName]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_cms_ContentAttachments_FriendlyFileName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_cms_ContentAttachments] DROP CONSTRAINT [DF_spb_cms_ContentAttachments_FriendlyFileName]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_cms_ContentAttachments_MediaType]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_cms_ContentAttachments] DROP CONSTRAINT [DF_spb_cms_ContentAttachments_MediaType]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_cms_ContentAttachments_ContentType]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_cms_ContentAttachments] DROP CONSTRAINT [DF_spb_cms_ContentAttachments_ContentType]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_cms_ContentAttachments_FileLength]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_cms_ContentAttachments] DROP CONSTRAINT [DF_spb_cms_ContentAttachments_FileLength]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_cms_ContentAttachments_Height]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_cms_ContentAttachments] DROP CONSTRAINT [DF_spb_cms_ContentAttachments_Height]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_cms_ContentAttachments_Price]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_cms_ContentAttachments] DROP CONSTRAINT [DF_spb_cms_ContentAttachments_Price]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_cms_ContentAttachments_Password]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_cms_ContentAttachments] DROP CONSTRAINT [DF_spb_cms_ContentAttachments_Password]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_cms_ContentAttachments]') AND type in (N'U'))
DROP TABLE [dbo].[spb_cms_ContentAttachments]
GO
/****** Object:  Table [dbo].[spb_cms_ContentFolderModerators]    Script Date: 07/26/2013 15:08:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_cms_ContentFolderModerators]') AND type in (N'U'))
DROP TABLE [dbo].[spb_cms_ContentFolderModerators]
GO
/****** Object:  Table [dbo].[spb_cms_ContentFolders]    Script Date: 07/26/2013 15:08:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_cms_ContentFolders]') AND type in (N'U'))
DROP TABLE [dbo].[spb_cms_ContentFolders]
GO
/****** Object:  Table [dbo].[spb_cms_ContentItems]    Script Date: 07/26/2013 15:08:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_cms_ContentItems]') AND type in (N'U'))
DROP TABLE [dbo].[spb_cms_ContentItems]
GO
/****** Object:  Table [dbo].[spb_cms_ContentTypeColumnDefinitions]    Script Date: 07/26/2013 15:08:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_cms_ContentTypeColumnDefinitions]') AND type in (N'U'))
DROP TABLE [dbo].[spb_cms_ContentTypeColumnDefinitions]
GO
/****** Object:  Table [dbo].[spb_cms_ContentTypeDefinitions]    Script Date: 07/26/2013 15:08:52 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_cms_ContentTypeDefinitions_AllowContributeRoleNames]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_cms_ContentTypeDefinitions] DROP CONSTRAINT [DF_spb_cms_ContentTypeDefinitions_AllowContributeRoleNames]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_cms_ContentTypeDefinitions]') AND type in (N'U'))
DROP TABLE [dbo].[spb_cms_ContentTypeDefinitions]
GO
/****** Object:  Table [dbo].[spb_cms_FormControlDefinitions]    Script Date: 07/26/2013 15:08:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_cms_FormControlDefinitions]') AND type in (N'U'))
DROP TABLE [dbo].[spb_cms_FormControlDefinitions]
GO
/****** Object:  Table [dbo].[spb_cms_FormControlDefinitions]    Script Date: 07/26/2013 15:08:52 ******/
/****** Object:  Table [dbo].[spb_cms_FormControlDefinitions]    Script Date: 07/30/2013 15:50:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[spb_cms_FormControlDefinitions](
	[ControlCode] [varchar](64) NOT NULL,
	[ControlName] [nvarchar](64) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_spb_cms_FormControlDefinitions] PRIMARY KEY CLUSTERED 
(
	[ControlCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[spb_cms_ContentTypeDefinitions]    Script Date: 07/30/2013 15:50:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[spb_cms_ContentTypeDefinitions](
	[ContentTypeId] [int] IDENTITY(1,1) NOT NULL,
	[ContentTypeName] [nvarchar](64) NOT NULL,
	[ContentTypeKey] [varchar](64) NOT NULL,
	[IsBuiltIn] [tinyint] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[TableName] [varchar](64) NOT NULL,
	[ForeignKey] [varchar](64) NOT NULL,
	[Page_New] [varchar](128) NOT NULL,
	[Page_Edit] [varchar](128) NOT NULL,
	[Page_Manage] [varchar](128) NOT NULL,
	[Page_Default_List] [varchar](128) NOT NULL,
	[Page_Default_Detail] [varchar](128) NOT NULL,
	[IsEnabled] [tinyint] NOT NULL,
	[EnableContribute] [tinyint] NOT NULL,
	[EnableComment] [tinyint] NOT NULL,
	[EnableAttachment] [tinyint] NOT NULL,
	[AllowContributeRoleNames] [varchar](512) NOT NULL,
 CONSTRAINT [PK_spb_cms_ContentTypeDefinitions] PRIMARY KEY CLUSTERED 
(
	[ContentTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'允许投稿的角色名集合，多个角色名用英文逗号隔开' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_ContentTypeDefinitions', @level2type=N'COLUMN',@level2name=N'AllowContributeRoleNames'
GO
/****** Object:  Table [dbo].[spb_cms_ContentTypeColumnDefinitions]    Script Date: 07/30/2013 15:50:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[spb_cms_ContentTypeColumnDefinitions](
	[ColumnId] [int] IDENTITY(1,1) NOT NULL,
	[ContentTypeId] [int] NOT NULL,
	[ColumnName] [varchar](64) NOT NULL,
	[ColumnLabel] [nvarchar](128) NOT NULL,
	[IsBuiltIn] [tinyint] NOT NULL,
	[DataType] [varchar](64) NOT NULL,
	[Length] [int] NOT NULL,
	[Precision] [varchar](64) NOT NULL,
	[IsNotNull] [tinyint] NOT NULL,
	[DefaultValue] [nvarchar](64) NOT NULL,
	[IsIndex] [tinyint] NOT NULL,
	[IsUnique] [tinyint] NOT NULL,
	[KeyOrIndexName] [varchar](64) NOT NULL,
	[KeyOrIndexColumns] [varchar](255) NOT NULL,
	[ControlCode] [varchar](64) NOT NULL,
	[InitialValue] [varchar](20) NOT NULL,
	[EnableInput] [tinyint] NOT NULL,
	[EnableEdit] [tinyint] NOT NULL,
	[ValidateRole] [nvarchar](64) NULL,
 CONSTRAINT [PK_spb_cms_ContentTypeColumnDefinitions] PRIMARY KEY CLUSTERED 
(
	[ColumnId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[spb_cms_ContentItems]    Script Date: 07/30/2013 15:50:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[spb_cms_ContentItems](
	[ContentItemId] [bigint] IDENTITY(1,1) NOT NULL,
	[ContentFolderId] [int] NOT NULL,
	[ContentTypeId] [int] NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[FeaturedImageAttachmentId] [bigint] NULL,
	[FeaturedImage] [nvarchar](255) NULL,
	[UserId] [bigint] NOT NULL,
	[Author] [nvarchar](64) NOT NULL,
	[Summary] [nvarchar](512) NOT NULL,
	[IsContributed] [tinyint] NOT NULL,
	[IsEssential] [tinyint] NOT NULL,
	[IsGlobalSticky] [tinyint] NOT NULL,
	[GlobalStickyDate] [datetime] NOT NULL,
	[IsFolderSticky] [tinyint] NOT NULL,
	[FolderStickyDate] [datetime] NOT NULL,
	[IsLocked] [tinyint] NOT NULL,
	[AuditStatus] [smallint] NOT NULL,
	[IP] [nvarchar](64) NOT NULL,
	[ReleaseDate] [datetime] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[DisplayOrder] [bigint] NOT NULL,
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
 CONSTRAINT [PK_spb_cms_ContentItems] PRIMARY KEY CLUSTERED 
(
	[ContentItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'摘要' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_ContentItems', @level2type=N'COLUMN',@level2name=N'Summary'
GO
/****** Object:  Table [dbo].[spb_cms_ContentFolders]    Script Date: 07/30/2013 15:50:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[spb_cms_ContentFolders](
	[ContentFolderId] [int] IDENTITY(1,1) NOT NULL,
	[FolderName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](2000) NOT NULL,
	[ParentId] [int] NOT NULL,
	[ParentIdList] [nvarchar](255) NOT NULL,
	[ChildCount] [int] NOT NULL,
	[Depth] [int] NOT NULL,
	[IsEnabled] [tinyint] NOT NULL,
	[ContentItemCount] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[ContentTypeKeys] [nvarchar](255) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[EnableContribute] [tinyint] NOT NULL,
	[IsAsNavigation] [tinyint] NOT NULL,
	[NeedAuditing] [tinyint] NOT NULL,
	[IsLink] [tinyint] NOT NULL,
	[LinkUrl] [nvarchar](255) NOT NULL,
	[IsLinkToNewWindow] [tinyint] NOT NULL,
	[Page_List] [varchar](128) NOT NULL,
	[Page_Detail] [varchar](128) NOT NULL,
	[ExtensionField] [nvarchar](max) NULL,
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
 CONSTRAINT [PK_spb_cms_ContentFolders] PRIMARY KEY CLUSTERED 
(
	[ContentFolderId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容模型Key集合(多个用英文逗号隔开)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_ContentFolders', @level2type=N'COLUMN',@level2name=N'ContentTypeKeys'
GO
/****** Object:  Table [dbo].[spb_cms_ContentFolderModerators]    Script Date: 07/30/2013 15:50:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[spb_cms_ContentFolderModerators](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContentFolderId] [int] NOT NULL,
	[UserId] [bigint] NOT NULL,
 CONSTRAINT [PK_spb_cms_ContentFolderModerators] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[spb_cms_ContentAttachments]    Script Date: 07/30/2013 15:50:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[spb_cms_ContentAttachments](
	[AttachmentId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[UserDisplayName] [nvarchar](64) NOT NULL,
	[FileName] [nvarchar](255) NOT NULL,
	[FriendlyFileName] [nvarchar](255) NOT NULL,
	[MediaType] [int] NOT NULL,
	[ContentType] [nvarchar](128) NOT NULL,
	[FileLength] [bigint] NOT NULL,
	[Height] [int] NOT NULL,
	[Width] [int] NOT NULL,
	[Price] [int] NOT NULL,
	[Password] [nvarchar](32) NOT NULL,
	[IP] [nvarchar](64) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
 CONSTRAINT [PK_spb_cms_ContentAttachments] PRIMARY KEY CLUSTERED 
(
	[AttachmentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'附件上传人UserId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_ContentAttachments', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'附件上传人名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_ContentAttachments', @level2type=N'COLUMN',@level2name=N'UserDisplayName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'实际存储文件名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_ContentAttachments', @level2type=N'COLUMN',@level2name=N'FileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件显示名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_ContentAttachments', @level2type=N'COLUMN',@level2name=N'FriendlyFileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'媒体类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_ContentAttachments', @level2type=N'COLUMN',@level2name=N'MediaType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'附件MIME类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_ContentAttachments', @level2type=N'COLUMN',@level2name=N'ContentType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件大小' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_ContentAttachments', @level2type=N'COLUMN',@level2name=N'FileLength'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片类型附件的高度（单位:px）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_ContentAttachments', @level2type=N'COLUMN',@level2name=N'Height'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片类型附件的宽度（单位:px）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_ContentAttachments', @level2type=N'COLUMN',@level2name=N'Width'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'售价（积分）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_ContentAttachments', @level2type=N'COLUMN',@level2name=N'Price'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下载密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_ContentAttachments', @level2type=N'COLUMN',@level2name=N'Password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'附件上传人IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_ContentAttachments', @level2type=N'COLUMN',@level2name=N'IP'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_ContentAttachments', @level2type=N'COLUMN',@level2name=N'DateCreated'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可序列化属性名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_ContentAttachments', @level2type=N'COLUMN',@level2name=N'PropertyNames'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可序列化属性内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_ContentAttachments', @level2type=N'COLUMN',@level2name=N'PropertyValues'
GO
/****** Object:  Table [dbo].[spb_cms_Addon_News]    Script Date: 07/30/2013 15:50:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[spb_cms_Addon_News](
	[ContentItemId] [bigint] NOT NULL,
	[TrimBodyAsSummary] [tinyint] NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[CopyFrom] [nvarchar](255) NOT NULL,
	[CopyFromUrl] [nvarchar](512) NOT NULL,
	[EnableComment] [tinyint] NOT NULL,
	[OriginalAuthor] [nvarchar](64) NOT NULL,
	[Editor] [nvarchar](64) NOT NULL,
	[Color] [varchar](16) NOT NULL,
	[IsBold] [tinyint] NOT NULL,
	[FirstAsTitleImage] [tinyint] NOT NULL,
	[AutoPage] [tinyint] NOT NULL,
	[PageLength] [int] NOT NULL,
 CONSTRAINT [PK_spb_cms_Addon_News] PRIMARY KEY CLUSTERED 
(
	[ContentItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联主表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_Addon_News', @level2type=N'COLUMN',@level2name=N'ContentItemId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否截取内容前200字作为摘要' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_Addon_News', @level2type=N'COLUMN',@level2name=N'TrimBodyAsSummary'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_Addon_News', @level2type=N'COLUMN',@level2name=N'Body'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'来源名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_Addon_News', @level2type=N'COLUMN',@level2name=N'CopyFrom'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'来源地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_Addon_News', @level2type=N'COLUMN',@level2name=N'CopyFromUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否允许评论' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_Addon_News', @level2type=N'COLUMN',@level2name=N'EnableComment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原创作者用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_Addon_News', @level2type=N'COLUMN',@level2name=N'OriginalAuthor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'责任编辑用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_Addon_News', @level2type=N'COLUMN',@level2name=N'Editor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题颜色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_Addon_News', @level2type=N'COLUMN',@level2name=N'Color'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题是否加粗' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_Addon_News', @level2type=N'COLUMN',@level2name=N'IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'设置第一张图片为标题图' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_Addon_News', @level2type=N'COLUMN',@level2name=N'FirstAsTitleImage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分页类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_Addon_News', @level2type=N'COLUMN',@level2name=N'AutoPage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每页显示的字符' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_Addon_News', @level2type=N'COLUMN',@level2name=N'PageLength'
GO
/****** Object:  Table [dbo].[spb_cms_Addon_Links]    Script Date: 07/30/2013 15:50:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[spb_cms_Addon_Links](
	[ContentItemId] [int] NOT NULL,
	[Color] [varchar](16) NOT NULL,
	[IsBold] [tinyint] NOT NULL,
	[LinkUrl] [nvarchar](512) NOT NULL,
 CONSTRAINT [PK_spb_cms_Addon_Links] PRIMARY KEY CLUSTERED 
(
	[ContentItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联主表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_Addon_Links', @level2type=N'COLUMN',@level2name=N'ContentItemId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题颜色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_Addon_Links', @level2type=N'COLUMN',@level2name=N'Color'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否加粗' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_Addon_Links', @level2type=N'COLUMN',@level2name=N'IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_cms_Addon_Links', @level2type=N'COLUMN',@level2name=N'LinkUrl'
GO
/****** Object:  Default [DF_spb_cms_ContentAttachments_FileName]    Script Date: 07/30/2013 15:50:34 ******/
ALTER TABLE [dbo].[spb_cms_ContentAttachments] ADD  CONSTRAINT [DF_spb_cms_ContentAttachments_FileName]  DEFAULT ('') FOR [FileName]
GO
/****** Object:  Default [DF_spb_cms_ContentAttachments_FriendlyFileName]    Script Date: 07/30/2013 15:50:34 ******/
ALTER TABLE [dbo].[spb_cms_ContentAttachments] ADD  CONSTRAINT [DF_spb_cms_ContentAttachments_FriendlyFileName]  DEFAULT ('') FOR [FriendlyFileName]
GO
/****** Object:  Default [DF_spb_cms_ContentAttachments_MediaType]    Script Date: 07/30/2013 15:50:34 ******/
ALTER TABLE [dbo].[spb_cms_ContentAttachments] ADD  CONSTRAINT [DF_spb_cms_ContentAttachments_MediaType]  DEFAULT ((99)) FOR [MediaType]
GO
/****** Object:  Default [DF_spb_cms_ContentAttachments_ContentType]    Script Date: 07/30/2013 15:50:34 ******/
ALTER TABLE [dbo].[spb_cms_ContentAttachments] ADD  CONSTRAINT [DF_spb_cms_ContentAttachments_ContentType]  DEFAULT ('') FOR [ContentType]
GO
/****** Object:  Default [DF_spb_cms_ContentAttachments_FileLength]    Script Date: 07/30/2013 15:50:34 ******/
ALTER TABLE [dbo].[spb_cms_ContentAttachments] ADD  CONSTRAINT [DF_spb_cms_ContentAttachments_FileLength]  DEFAULT ((0)) FOR [FileLength]
GO
/****** Object:  Default [DF_spb_cms_ContentAttachments_Height]    Script Date: 07/30/2013 15:50:34 ******/
ALTER TABLE [dbo].[spb_cms_ContentAttachments] ADD  CONSTRAINT [DF_spb_cms_ContentAttachments_Height]  DEFAULT ((0)) FOR [Height]
GO
/****** Object:  Default [DF_spb_cms_ContentAttachments_Price]    Script Date: 07/30/2013 15:50:34 ******/
ALTER TABLE [dbo].[spb_cms_ContentAttachments] ADD  CONSTRAINT [DF_spb_cms_ContentAttachments_Price]  DEFAULT ((0)) FOR [Price]
GO
/****** Object:  Default [DF_spb_cms_ContentAttachments_Password]    Script Date: 07/30/2013 15:50:34 ******/
ALTER TABLE [dbo].[spb_cms_ContentAttachments] ADD  CONSTRAINT [DF_spb_cms_ContentAttachments_Password]  DEFAULT ('') FOR [Password]
GO
/****** Object:  Default [DF_spb_cms_ContentTypeDefinitions_AllowContributeRoleNames]    Script Date: 07/30/2013 15:50:34 ******/
ALTER TABLE [dbo].[spb_cms_ContentTypeDefinitions] ADD  CONSTRAINT [DF_spb_cms_ContentTypeDefinitions_AllowContributeRoleNames]  DEFAULT ('') FOR [AllowContributeRoleNames]
GO
