-----添加应用数据
DELETE FROM [dbo].[tn_Applications] WHERE [ApplicationId] = 1015
INSERT [dbo].[tn_Applications] ([ApplicationId], [ApplicationKey], [Description], [IsEnabled], [IsLocked], [DisplayOrder]) VALUES (1015, N'CMS', N'资讯应用', 1, 0, 1015)

-----应用在呈现区域的设置
DELETE FROM [dbo].[tn_ApplicationInPresentAreaSettings] WHERE [ApplicationId] = 1015
INSERT [dbo].[tn_ApplicationInPresentAreaSettings] ([ApplicationId], [PresentAreaKey], [IsBuiltIn], [IsAutoInstall], [IsGenerateData]) VALUES (1015, N'Channel', 0, 1, 1)
INSERT [dbo].[tn_ApplicationInPresentAreaSettings] ([ApplicationId], [PresentAreaKey], [IsBuiltIn], [IsAutoInstall], [IsGenerateData]) VALUES (1015, N'UserSpace', 0, 1, 0)

-----默认安装记录
DELETE FROM [dbo].[tn_ApplicationInPresentAreaInstallations] WHERE [ApplicationId] = 1015 and OwnerId = 0
INSERT [dbo].[tn_ApplicationInPresentAreaInstallations] ([OwnerId], [ApplicationId], [PresentAreaKey]) VALUES (0, 1015, 'Channel')

-----快捷操作
DELETE FROM [dbo].[tn_ApplicationManagementOperations] WHERE [ApplicationId] = 1015
INSERT [dbo].[tn_ApplicationManagementOperations] ([OperationId], [ApplicationId], [AssociatedNavigationId], [PresentAreaKey], [OperationType], [OperationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (10101501, 1015, 0, N'Channel', 1, N'投稿', N'', N'', N'Channel_CMS_Contribute', NULL, N'', NULL, N'_self', 10101501, 0, 1, 1)

-------动态
DELETE FROM  [dbo].[tn_ActivityItems] WHERE [ApplicationId] = 1015
INSERT [dbo].[tn_ActivityItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [Description], [IsOnlyOnce], [IsUserReceived], [IsSiteReceived]) VALUES (N'CreateCmsComment', 1015, N'评论资讯', 1, N'', 0, 1, 0)

-----用户角色
DELETE FROM [dbo].[tn_Roles] WHERE [ApplicationId] = 1015
INSERT [dbo].[tn_Roles] ([RoleName], [FriendlyRoleName], [IsBuiltIn], [ConnectToUser], [ApplicationId], [IsPublic], [Description], [IsEnabled], [RoleImage]) VALUES (N'CMSAdministrator', N'资讯管理员', 1, 1, 1015, 1, N'管理资讯应用下的内容', 1, N'')

-- 应用的审核规则
delete from tn_ApplicationData where ApplicationId=1015
insert tn_ApplicationData([ApplicationId] ,[TenantTypeId],[Datakey],[LongValue],[DecimalValue] ,[StringValue]) values(1015,'','PubliclyAuditStatus',40,0.0000,'')

-----自运行任务
DELETE FROM [dbo].[tn_TaskDetails] WHERE [ClassType] = N'Spacebuilder.CMS.ExpireStickyContentItemTask,Spacebuilder.CMS'
INSERT [dbo].[tn_TaskDetails] ([Name], [TaskRule], [ClassType], [Enabled], [RunAtRestart], [IsRunning], [LastStart], [LastEnd], [LastIsSuccess], [NextStart], [StartDate], [EndDate], [RunAtServer]) VALUES (N'更新置顶时间到期的资讯', N'0 0 0/12 * * ?', N'Spacebuilder.CMS.ExpireStickyContentItemTask,Spacebuilder.CMS', 1, 0, 0, N'', N'', 1, N'', N'', NULL, 0)

-----权限项
DELETE FROM [dbo].[tn_PermissionItems] WHERE [ApplicationId] = 1015
INSERT [dbo].[tn_PermissionItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [EnableQuota], [EnableScope]) VALUES (N'CMS_ContentItem', 1015, N'资讯投稿', 16, 0, 0)

-----角色针对权限的设置
DELETE FROM [dbo].[tn_PermissionItemsInUserRoles] WHERE [ItemKey] = N'CMS_ContentItem' and [RoleName] = N'RegisteredUsers'
INSERT [dbo].[tn_PermissionItemsInUserRoles] ([RoleName], [ItemKey], [PermissionType], [PermissionQuota], [PermissionScope], [IsLocked]) VALUES ( N'RegisteredUsers', N'CMS_ContentItem', 1, 0, 0, 0)

-----审核项
DELETE FROM [dbo].[tn_AuditItems] WHERE [ApplicationId] = 1015
INSERT [dbo].[tn_AuditItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [Description]) VALUES (N'CMS_ContentItem', 1015, N'资讯', 1, N'')

--审核规则
DELETE FROM [dbo].[tn_AuditItemsInUserRoles] WHERE [ItemKey] = N'CMS_ContentItem' and [RoleName] = N'RegisteredUsers'
INSERT [dbo].[tn_AuditItemsInUserRoles]([RoleName],[ItemKey] ,[StrictDegree],[IsLocked])VALUES(N'RegisteredUsers',N'CMS_ContentItem',2 ,0)

-----积分项
DELETE FROM [dbo].[tn_PointItems] WHERE [ApplicationId] = 1015
INSERT [dbo].[tn_PointItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [ExperiencePoints], [ReputationPoints], [TradePoints], [TradePoints2], [TradePoints3], [TradePoints4], [Description],[NeedPointMessage]) VALUES (N'CMS_ContributeAccepted', 1015, N'投稿被采纳', 151, 20, 0, 20, 0, 0, 0, N'',0)
INSERT [dbo].[tn_PointItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [ExperiencePoints], [ReputationPoints], [TradePoints], [TradePoints2], [TradePoints3], [TradePoints4], [Description],[NeedPointMessage]) VALUES (N'CMS_ContributeDeleted', 1015, N'投稿被删除', 151, -20, 0, -20, 0, 0, 0, N'',0)
INSERT [dbo].[tn_PointItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [ExperiencePoints], [ReputationPoints], [TradePoints], [TradePoints2], [TradePoints3], [TradePoints4], [Description],[NeedPointMessage]) VALUES (N'CMS_StickyNews', 1015, N'投稿被置顶', 151, 100, 0, 100, 0, 0, 0, N'',0)

-----推荐类别
DELETE FROM [dbo].[tn_RecommendItemTypes] WHERE [TypeId] in (N'10150101',N'10150102')
INSERT [dbo].[tn_RecommendItemTypes] ([TypeId], [TenantTypeId], [Name], [Description], [HasFeaturedImage], [DateCreated]) VALUES (N'10150101', N'101501', N'推荐资讯幻灯片', N'', 1, getdate())
INSERT [dbo].[tn_RecommendItemTypes] ([TypeId], [TenantTypeId], [Name], [Description], [HasFeaturedImage], [DateCreated]) VALUES (N'10150102', N'000031', N'精彩点评', N'', 0, getdate())

-----租户类型
DELETE FROM [dbo].[tn_TenantTypes] WHERE TenantTypeId in ('101500','101501','101502')
INSERT [dbo].[tn_TenantTypes] ([TenantTypeId], [ApplicationId], [Name], [ClassType]) VALUES (N'101500', 1015, N'资讯应用', N'')
INSERT [dbo].[tn_TenantTypes] ([TenantTypeId], [ApplicationId], [Name], [ClassType]) VALUES (N'101501', 1015, N'资讯', N'Spacebuilder.CMS.ContentItem,Spacebuilder.CMS.Service')
INSERT [dbo].[tn_TenantTypes] ([TenantTypeId], [ApplicationId], [Name], [ClassType]) VALUES (N'101502', 1015, N'资讯附件', N'Spacebuilder.CMS.ContentAttachment,Spacebuilder.CMS.Service')

-----租户使用到的服务
DELETE FROM [dbo].[tn_TenantTypesInServices] WHERE [TenantTypeId] in ('101501','101502')
INSERT INTO [dbo].[tn_TenantTypesInServices]([TenantTypeId],[ServiceKey]) VALUES('101501','Attachment')
INSERT INTO [dbo].[tn_TenantTypesInServices]([TenantTypeId],[ServiceKey]) VALUES('101501','Tag')
INSERT INTO [dbo].[tn_TenantTypesInServices]([TenantTypeId],[ServiceKey]) VALUES('101501','Notice')
INSERT INTO [dbo].[tn_TenantTypesInServices]([TenantTypeId],[ServiceKey]) VALUES ('101501','Recommend')
INSERT INTO [dbo].[tn_TenantTypesInServices]([TenantTypeId],[ServiceKey]) VALUES ('101501','Comment')
INSERT INTO [dbo].[tn_TenantTypesInServices]([TenantTypeId],[ServiceKey]) VALUES ('101501','Attitude')
INSERT INTO [dbo].[tn_TenantTypesInServices]([TenantTypeId],[ServiceKey]) VALUES ('101501','Visit')
INSERT INTO [dbo].[tn_TenantTypesInServices]([TenantTypeId],[ServiceKey]) VALUES ('101501','Count')
INSERT INTO [dbo].[tn_TenantTypesInServices]([TenantTypeId],[ServiceKey]) VALUES ('101502','Count')

-----初始化导航
DELETE FROM [dbo].[tn_InitialNavigations] WHERE [ApplicationId] = 1015
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (10101501, 0, 0, N'Channel', 1015, 0, N'资讯', N'', N'', N'Channel_CMS_Home', NULL, N'Cms', NULL, N'_self', 10101501, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (10101502, 10101501, 1, N'Channel', 1015, 0, N'资讯首页', N' ', N' ', N'Channel_CMS_Home', NULL, NULL, NULL, N'_self', 10101502, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (10101506, 10101501, 1, N'Channel', 1015, 0, N'我的资讯', N' ', N' ', N'Channel_CMS_My', NULL, NULL, NULL, N'_self', 10101506, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (11101501, 0, 0, N'UserSpace', 1015, 0, N'资讯', N' ', N' ', N'Channel_CMS_User', N'', N'Cms', NULL, N'_blank', 11101501, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (20101501, 20000011, 2, N'ControlPanel', 1015, 0, N'资讯', N'', N'', N'ControlPanel_CMS_Home', NULL, NULL, NULL, N'_self', 20101501, 0, 0, 1)

----数据模型
DELETE FROM [spb_cms_ContentTypeDefinitions]
INSERT [spb_cms_ContentTypeDefinitions] ([ContentTypeName], [ContentTypeKey], [IsBuiltIn], [DisplayOrder], [TableName], [ForeignKey], [Page_New], [Page_Edit], [Page_Manage], [Page_Default_List], [Page_Default_Detail], [IsEnabled], [EnableContribute], [EnableComment], [EnableAttachment]) VALUES (N'文章', N'News', 1, 1, N'spb_cms_Addon_News', N'ContentItemId', N'EditNews', N'EditNews', N'ManageNews', N'ListNews', N'ShowNews', 1, 0, 0, 1)
INSERT [spb_cms_ContentTypeDefinitions] ([ContentTypeName], [ContentTypeKey], [IsBuiltIn], [DisplayOrder], [TableName], [ForeignKey], [Page_New], [Page_Edit], [Page_Manage], [Page_Default_List], [Page_Default_Detail], [IsEnabled], [EnableContribute], [EnableComment], [EnableAttachment]) VALUES (N'链接', N'NewsLink', 1, 2, N'spb_cms_Addon_Links', N'ContentItemId', N'EditLink', N'EditLink', N'ManageLinks', N'ListLinks', N'''''', 1, 0, 0, 0)

----副表元信息
DELETE FROM spb_cms_ContentTypeColumnDefinitions
INSERT [spb_cms_ContentTypeColumnDefinitions] 
([ContentTypeId], [ColumnName], [ColumnLabel], [IsBuiltIn], [DataType], [Length], [Precision], [IsNotNull], [DefaultValue], 
[IsIndex], [IsUnique], [KeyOrIndexName], [KeyOrIndexColumns], [ControlCode], [InitialValue], [EnableInput], [EnableEdit], [ValidateRole])
SELECT 
[ContentTypeId]= (select top 1 ContentTypeId from spb_cms_ContentTypeDefinitions where TableName= O.name),
[ColumnName]=C.name,
[ColumnLabel]=cast(ISNULL(PFD.[value],N'') as nvarchar(128)),
[IsBuiltIn]=1,
[DataType]=case when T.name like '%char' then N'string' 
				when T.name = 'tinyint' then N'bool' 
				when T.name = 'smallint' or T.name = 'int' then N'int'
				when T.name = 'bigint' then N'long' 
				else T.name end,
[Length]=C.max_length,
[Precision]=cast(C.precision as varchar(64)),
[IsNotNull]=C.is_nullable,
[DefaultValue]=N'',
[IsIndex]=ISNULL(IDX.IsIndex,0),
[IsUnique]=ISNULL(IDX.IsUnique,0),
[KeyOrIndexName]=ISNULL(IDX.IndexName,N''),
[KeyOrIndexColumns]=case when IDX.IsIndex=1  then C.name else N'' end,
[ControlCode]=N'',
[InitialValue]=N'',
[EnableInput]=1,
[EnableEdit]=case when IDX.IndexName like 'PK%' then 0 else 1 end,
[ValidateRole]=N''
FROM sys.columns C
INNER JOIN sys.objects O
ON C.[object_id]=O.[object_id]
AND O.type='U'
AND O.is_ms_shipped=0
INNER JOIN sys.types T
ON C.user_type_id=T.user_type_id
LEFT JOIN sys.default_constraints D
ON C.[object_id]=D.parent_object_id
AND C.column_id=D.parent_column_id
AND C.default_object_id=D.[object_id]
LEFT JOIN sys.extended_properties PFD
ON PFD.class=1 
AND C.[object_id]=PFD.major_id 
AND C.column_id=PFD.minor_id
LEFT JOIN sys.extended_properties PTB
ON PTB.class=1 
AND PTB.minor_id=0 
AND C.[object_id]=PTB.major_id
LEFT JOIN -- 索引及主键信息
(
SELECT 
IDX.object_id,
IDXC.column_id,
IsIndex=1,
IsUnique=IDX.is_unique,
PrimaryKey=IDX.is_primary_key,
IndexName=IDX.Name
FROM sys.indexes IDX
INNER JOIN sys.index_columns IDXC
ON IDX.[object_id]=IDXC.[object_id]
AND IDX.index_id=IDXC.index_id
LEFT JOIN sys.key_constraints KC
ON IDX.[object_id]=KC.[parent_object_id]
AND IDX.index_id=KC.unique_index_id
INNER JOIN -- 对于一个列包含多个索引的情况,只显示第1个索引信息
(
SELECT [object_id], Column_id, index_id=MIN(index_id)
FROM sys.index_columns
GROUP BY [object_id], Column_id
) IDXCUQ
ON IDXC.[object_id]=IDXCUQ.[object_id]
AND IDXC.Column_id=IDXCUQ.Column_id
AND IDXC.index_id=IDXCUQ.index_id
--WHERE idx.object_id=object_id(N'spb_CMSPages') -- 如果只查询指定表,加上此条件
) IDX
ON C.[object_id]=IDX.[object_id]
AND C.column_id=IDX.column_id

WHERE O.name=N'spb_cms_Addon_News' or O.name=N'spb_cms_Addon_Links' -- 如果只查询指定表,加上此条件
ORDER BY O.name,C.column_id
