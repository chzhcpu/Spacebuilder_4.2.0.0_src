-----应用
DELETE FROM [dbo].[tn_Applications] WHERE [ApplicationId] = 9002
INSERT [dbo].[tn_Applications] ([ApplicationId], [ApplicationKey], [Description], [IsEnabled], [IsLocked], [DisplayOrder]) VALUES (9002, N'Topic', N'专题应用', 1, 0, 9002)

-----应用在呈现区域相关设置
DELETE FROM [dbo].[tn_ApplicationInPresentAreaSettings] WHERE [ApplicationId] = 9002
INSERT [dbo].[tn_ApplicationInPresentAreaSettings] ([ApplicationId], [PresentAreaKey], [IsBuiltIn], [IsAutoInstall], [IsGenerateData]) VALUES (9002, N'Channel', 0, 1, 1)
INSERT [dbo].[tn_ApplicationInPresentAreaSettings] ([ApplicationId], [PresentAreaKey], [IsBuiltIn], [IsAutoInstall], [IsGenerateData]) VALUES (9002, N'UserSpace', 0, 1, 0)

----快捷操作
DELETE FROM [dbo].[tn_ApplicationManagementOperations] WHERE [ApplicationId] = 9002
INSERT [dbo].[tn_ApplicationManagementOperations] ([OperationId], [ApplicationId], [AssociatedNavigationId], [PresentAreaKey], [OperationType], [OperationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (10900201, 9002, 0, N'Channel', 1, N'创建专题', N'', N'', N'Channel_Topic_Create', NULL, N'UserRelation', NULL, N'_blank', 10900201, 0, 1, 1)
INSERT [dbo].[tn_ApplicationManagementOperations] ([OperationId], [ApplicationId], [AssociatedNavigationId], [PresentAreaKey], [OperationType], [OperationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (20900201, 9002, 0, N'ControlPanel', 1, N'专题类别管理', N'', N'', N'ControlPanel_Content_ManageSiteCategories', N'tenantTypeId', NULL, NULL, N'_blank', 20900201, 1, 0, 1)

-----导航
DELETE FROM [dbo].[tn_InitialNavigations] WHERE [ApplicationId] = 9002
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (10900201, 0, 0, N'Channel', 9002, 0, N'专题', N'', N'', N'Channel_Topic_Home', NULL, N'Topic', NULL, N'_self', 10900201, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (10900202, 10900201, 1, N'Channel', 9002, 0, N'专题首页', N' ', N' ', N'Channel_Topic_Home', NULL, NULL, NULL, N'_self', 10900202, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (10900203, 10900201, 1, N'Channel', 9002, 0, N'我的专题', N'', N'', N'Channel_Topic_UserTopics', N'spaceKey', NULL, NULL, N'_self', 10900203, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (10900204, 10900201, 1, N'Channel', 9002, 0, N'发现专题', N'', N'', N'Channel_Topic_FindTopic', NULL, NULL, NULL, N'_self', 10900204, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (11900201, 0, 0, N'UserSpace', 9002, 0, N'专题', N' ', N' ', N'Channel_Topic_UserTopics', N'spaceKey', N'Topic', NULL, N'_self', 11900201, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (13900280, 0, 0, N'TopicSpace', 0, 1, N'成员', N' ', N' ', N'TopicSpace_Member', NULL, NULL, NULL, N'_self', 13900280, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (13900290, 0, 0, N'TopicSpace', 0, 1, N'管理', N' ', N' ', N'Topic_Bar_ManageThreads', NULL, NULL, NULL, N'_self', 13900290, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (20900201, 20000011, 2, N'ControlPanel', 9002, 0, N'专题', N'', N'', N'ControlPanel_Topic_Home', NULL, NULL, NULL, N'_self', 20900201, 0, 0, 1)

-----默认安装记录
DELETE FROM [dbo].[tn_ApplicationInPresentAreaInstallations] WHERE [ApplicationId] = 9002 and OwnerId = 0
INSERT [dbo].[tn_ApplicationInPresentAreaInstallations] ([OwnerId], [ApplicationId], [PresentAreaKey]) VALUES (0, 9002, 'Channel')

-----动态
DELETE FROM  [dbo].[tn_ActivityItems] WHERE [ApplicationId] = 9002
INSERT [dbo].[tn_ActivityItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [Description], [IsOnlyOnce], [IsUserReceived], [IsSiteReceived]) VALUES (N'CreateTopic', 9002, N'创建专题', 0, N'', 0, 1, 1)
INSERT [dbo].[tn_ActivityItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [Description], [IsOnlyOnce], [IsUserReceived], [IsSiteReceived]) VALUES (N'JoinTopic', 9002, N'加入专题', 0, N'', 0, 1, 0)
INSERT [dbo].[tn_ActivityItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [Description], [IsOnlyOnce], [IsUserReceived], [IsSiteReceived]) VALUES (N'CreateTopicMember', 9002, N'新成员加入', 0, N'', 1, 0, 0)

-----用户角色
DELETE FROM [dbo].[tn_Roles] WHERE [ApplicationId] = 9002
INSERT [dbo].[tn_Roles] ([RoleName], [FriendlyRoleName], [IsBuiltIn], [ConnectToUser], [ApplicationId], [IsPublic], [Description], [IsEnabled], [RoleImage]) VALUES (N'TopicAdministrator', N'专题管理员', 1, 1, 9002, 1, N'管理专题应用下的内容', 1, N'')

-----审核
DELETE FROM [dbo].[tn_AuditItems] WHERE [ApplicationId] = 9002
INSERT [dbo].[tn_AuditItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [Description]) VALUES (N'Topic', 9002, N'创建专题', 11, N'')

-----审核规则
INSERT [dbo].[tn_AuditItemsInUserRoles]([RoleName],[ItemKey] ,[StrictDegree],[IsLocked])VALUES(N'RegisteredUsers',N'Topic',2 ,0)
INSERT [dbo].[tn_AuditItemsInUserRoles]([RoleName],[ItemKey] ,[StrictDegree],[IsLocked])VALUES(N'ModeratedUser',N'Topic',2 ,0)

-----积分
DELETE FROM [dbo].[tn_PointItems] WHERE [ApplicationId]=9002
INSERT [dbo].[tn_PointItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [ExperiencePoints], [ReputationPoints], [TradePoints], [TradePoints2], [TradePoints3], [TradePoints4], [Description],[NeedPointMessage]) VALUES (N'Topic_CreateTopic', 9002, N'创建专题', 101, 100, 50, 50, 0, 0, 0, N'',1)
INSERT [dbo].[tn_PointItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [ExperiencePoints], [ReputationPoints], [TradePoints], [TradePoints2], [TradePoints3], [TradePoints4], [Description],[NeedPointMessage]) VALUES (N'Topic_DeleteTopic', 9002, N'删除专题', 102, -100, -50, -50, 0, 0, 0, N'',0)

-----租户类型
DELETE FROM [dbo].[tn_TenantTypes] WHERE [ApplicationId]=9002
INSERT [dbo].[tn_TenantTypes] ([TenantTypeId], [ApplicationId], [Name], [ClassType]) VALUES (N'900200', 9002, N'专题', N'SpecialTopic.Topic.TopicEntity,SpecialTopic.Topic')

-----租户使相关服务
DELETE FROM [dbo].[tn_TenantTypesInServices] WHERE [TenantTypeId]='900200'
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'900200', N'Count')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'900200', N'SiteCategory')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'900200', N'Tag')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'900200', N'Recommend')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'900200', N'Visit')

-----自运行任务
DELETE FROM [dbo].[tn_TaskDetails] WHERE [ClassType]=N'SpecialTopic.Topic.CalculateGrowthValuesTask,SpecialTopic.Topic'
INSERT [dbo].[tn_TaskDetails] ([Name], [TaskRule], [ClassType], [Enabled], [RunAtRestart], [IsRunning], [LastStart], [LastEnd], [LastIsSuccess], [NextStart], [StartDate], [EndDate], [RunAtServer]) VALUES (N'更新专题的成长值', N'0 0 0/12 * * ?', N'SpecialTopic.Topic.CalculateGrowthValuesTask,SpecialTopic.Topic', 1, 0, 0, N'', N'', 1, N'', N'', NULL, 0)

-----推荐
DELETE FROM [dbo].[tn_RecommendItemTypes] WHERE [TypeId] IN ('00001111','90020001')
INSERT [dbo].[tn_RecommendItemTypes] ([TypeId], [TenantTypeId], [Name], [Description], [HasFeaturedImage], [DateCreated]) VALUES (N'00001111', N'000011', N'推荐群主', N'推荐群主', 0, N'')
INSERT [dbo].[tn_RecommendItemTypes] ([TypeId], [TenantTypeId], [Name], [Description], [HasFeaturedImage], [DateCreated]) VALUES (N'90020001', N'900200', N'推荐专题', N'推荐专题', 0, N'')

-----类别
DELETE FROM [dbo].[tn_Categories] WHERE [TenantTypeId] = '900200'
SET IDENTITY_INSERT [tn_Categories] ON
INSERT [tn_Categories] ([ParentId], [OwnerId], [TenantTypeId], [CategoryName], [Description], [DisplayOrder], [Depth], [ChildCount], [ItemCount], [PrivacyStatus], [AuditStatus], [FeaturedItemId], [LastModified], [DateCreated], [PropertyNames], [PropertyValues]) VALUES (0, 0, N'900200', N'默认类别', N'', 28, 0, 0, 0, 2, 40, 0, CAST(0x0000A187003CE170 AS DateTime), CAST(0x0000A187003376B7 AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [tn_Categories] OFF

-----广告位
DELETE FROM [dbo].[tn_AdvertisingPosition] WHERE [PositionId] like '109002%' or [PositionId] like '139002%'
INSERT [dbo].[tn_AdvertisingPosition] ([PositionId], [PresentAreaKey], [Description], [FeaturedImage], [Width], [Height], [IsEnable]) VALUES (N'10900200001', N'Channel', N'专题频道首页中上部广告位(550x190)', N'AdvertisingPosition\00001\09002\00001\10900200001.jpg', 550, 190, 1)
INSERT [dbo].[tn_AdvertisingPosition] ([PositionId], [PresentAreaKey], [Description], [FeaturedImage], [Width], [Height], [IsEnable]) VALUES (N'10900200002', N'Channel', N'专题频道首页左中部广告位(190x70)', N'AdvertisingPosition\00001\09002\00002\10900200002.jpg', 190, 70, 1)
INSERT [dbo].[tn_AdvertisingPosition] ([PositionId], [PresentAreaKey], [Description], [FeaturedImage], [Width], [Height], [IsEnable]) VALUES (N'13900200003', N'TopicSpace', N'专题详细显示页中部广告位(710x100)', N'AdvertisingPosition\00001\39002\00003\13900200003.jpg', 710, 100, 1)
INSERT [dbo].[tn_AdvertisingPosition] ([PositionId], [PresentAreaKey], [Description], [FeaturedImage], [Width], [Height], [IsEnable]) VALUES (N'13900200004', N'TopicSpace', N'专题讨论详细显示页中部广告位(710x70)', N'AdvertisingPosition\00001\39002\00004\13900200004.jpg', 710, 70, 1)
INSERT [dbo].[tn_AdvertisingPosition] ([PositionId], [PresentAreaKey], [Description], [FeaturedImage], [Width], [Height], [IsEnable]) VALUES (N'13900200005', N'TopicSpace', N'专题讨论详细显示页右下部广告位(230x260)', N'AdvertisingPosition\00001\39002\00005\13900200005.jpg', 230, 260, 1)


--添加皮肤设置

insert tn_PresentAreas(PresentAreaKey, AllowMultipleInstances, EnableThemes, DefaultAppearanceId, ThemeLocation) values('TopicSpace',1,	1,	'TopicSpace,Default,Default','~/Themes/TopicSpace')

/*insert [tn_ThemeAppearances] ([Id],[PresentAreaKey] ,[ThemeKey],[AppearanceKey],[Name] ,[PreviewImage],[PreviewLargeImage],[LogoFileName],[Description],[Tags],[Author] ,[Copyright],[LastModified],[Version] ,[ForProductVersion],[DateCreated],[IsEnabled],[DisplayOrder],[UserCount],[Roles],[RequiredRank]) 
values()
TopicSpace,Deep_sea,Default	TopicSpace	Deep_sea	Default	Deep_sea	Preview.png	Preview.png	 	 	 	admin	chz	1900-01-01 00:00:00.000	1.0	4.0	1900-01-01 00:00:00.000	1	4	0	 	1
TopicSpace,Default,Default	TopicSpace	Default	Default	Default	Preview.png	Preview.png				admin	chz	2015-02-08 00:00:00.000	1.0	4.0	2015-02-08 00:00:00.000	1	1	1		1
TopicSpace,Portal,Default	TopicSpace	Portal	Default	Portal	Preview.png	Preview.png	 	 	 	admin	chz	1900-01-01 00:00:00.000	1.0	4.0	1900-01-01 00:00:00.000	1	5	0	 	1
*/

/*
insert tn_themes([Id],[PresentAreaKey],[ThemeKey],[Parent],[Version])
values()
TopicSpace,Deep_sea	TopicSpace	Deep_sea	Default	1.0
TopicSpace,Default	TopicSpace	Default	Default	1.0
TopicSpace,Portal	TopicSpace	Portal	Default	1.0
*/

