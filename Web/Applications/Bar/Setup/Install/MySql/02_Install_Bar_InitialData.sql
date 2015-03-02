-- 应用
DELETE FROM `tn_Applications` WHERE `ApplicationId` = 1012;
INSERT `tn_Applications` (`ApplicationId`, `ApplicationKey`, `Description`, `IsEnabled`, `IsLocked`, `DisplayOrder`) VALUES (1012, 'Bar', '帖吧应用', 1, 0, 1012);

-- 快捷操作
DELETE FROM `tn_ApplicationManagementOperations`  WHERE `ApplicationId` = 1012;
DELETE FROM `tn_ApplicationManagementOperations`  WHERE `ApplicationId` = 1011;
INSERT `tn_ApplicationManagementOperations` (`OperationId`, `ApplicationId`, `AssociatedNavigationId`, `PresentAreaKey`, `OperationType`, `OperationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (13101201, 1012, 13101201, 'GroupSpace', 1, '发帖', '', '', 'Group_Bar_Edit', NULL, NULL, NULL, '_self', 13101201, 0, 1, 1);
INSERT `tn_ApplicationManagementOperations` (`OperationId`, `ApplicationId`, `AssociatedNavigationId`, `PresentAreaKey`, `OperationType`, `OperationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (20101102, 1011, 0, 'ControlPanel', 1, '帖子管理', '', '', 'ControlPanel_GroupBar_ManageThreads', NULL, NULL, NULL, '_self', 20101102, 1, 0, 1);
INSERT `tn_ApplicationManagementOperations` (`OperationId`, `ApplicationId`, `AssociatedNavigationId`, `PresentAreaKey`, `OperationType`, `OperationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (20101103, 1011, 0, 'ControlPanel', 1, '回帖管理', '', '', 'ControlPanel_GroupBar_ManagePosts', NULL, NULL, NULL, '_self', 20101104, 1, 0, 1);
INSERT `tn_ApplicationManagementOperations` (`OperationId`, `ApplicationId`, `AssociatedNavigationId`, `PresentAreaKey`, `OperationType`, `OperationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (20101104, 1011, 0, 'ControlPanel', 1, '发言管理', '', '', 'ControlPanel_GroupMicroblog_Common', NULL, NULL, NULL, '_self', 20101103, 1, 0, 1);

-- 应用在呈现区域的设置
DELETE FROM `tn_ApplicationInPresentAreaSettings` WHERE `ApplicationId` = 1012;
INSERT `tn_ApplicationInPresentAreaSettings` (`ApplicationId`, `PresentAreaKey`, `IsBuiltIn`, `IsAutoInstall`, `IsGenerateData`) VALUES (1012, 'Channel', 0, 1, 1);
INSERT `tn_ApplicationInPresentAreaSettings` (`ApplicationId`, `PresentAreaKey`, `IsBuiltIn`, `IsAutoInstall`, `IsGenerateData`) VALUES (1012, 'UserSpace', 0, 1, 0);
INSERT `tn_ApplicationInPresentAreaSettings` (`ApplicationId`, `PresentAreaKey`, `IsBuiltIn`, `IsAutoInstall`, `IsGenerateData`) VALUES (1012, 'GroupSpace', 0, 1, 1);

-- 默认安装记录
DELETE FROM `tn_ApplicationInPresentAreaInstallations` WHERE `ApplicationId` = 1012  and OwnerId = 0;
INSERT `tn_ApplicationInPresentAreaInstallations` (`OwnerId`, `ApplicationId`, `PresentAreaKey`) VALUES (0, 1012, 'Channel');

-- 导航
DELETE FROM `tn_InitialNavigations` WHERE `ApplicationId` = 1012;
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (10101201, 0, 0, 'Channel', 1012, 0, '帖吧', '', '', 'Channel_Bar_Home', NULL, 'Bar', NULL, '_self', 10101201, 0, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (11101201, 0, 0, 'UserSpace', 1012, 0, '帖吧', ' ', ' ', 'Channel_Bar_UserBar', '', 'Bar', NULL, '_blank', 11101201, 0, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (20101201, 20000011, 2, 'ControlPanel', 1012, 0, '帖吧', ' ', ' ', 'ControlPanel_Bar_Home', NULL, NULL, NULL, '_self', 20101201, 0, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (13101201, 0, 0, 'GroupSpace', 1012, 0, '讨论', '', '', 'Group_Bar_SectionDetail', NULL, NULL, NULL, '_self', 13101201, 0, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (13101205, 13900190, 1, 'GroupSpace', 1012, 0, '讨论管理', ' ', ' ', 'Group_Bar_ManageThreads', NULL, NULL, NULL, '_self', 13101205, 0, 0, 1);

-- 用户角色
DELETE FROM `tn_Roles` WHERE `ApplicationId` = 1012;
INSERT `tn_Roles` (`RoleName`, `FriendlyRoleName`, `IsBuiltIn`, `ConnectToUser`, `ApplicationId`, `IsPublic`, `Description`, `IsEnabled`, `RoleImage`) VALUES ('BarAdministrator', '帖吧管理员', 1, 1, 1012, 1, '管理帖吧应用下的内容', 1, '');

-- 权限项
DELETE FROM `tn_PermissionItems` WHERE `ApplicationId` = 1012;
INSERT `tn_PermissionItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `EnableQuota`, `EnableScope`) VALUES ('Bar_CreateThread', 1012, '创建帖子', 11, 0, 0);
INSERT `tn_PermissionItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `EnableQuota`, `EnableScope`) VALUES ('Bar_CreatePost', 1012, '创建回帖', 12, 0, 0);
-- 角色针对权限的设置
DELETE FROM `tn_PermissionItemsInUserRoles` WHERE `ItemKey` = 'Bar_CreateThread' and `RoleName` = 'RegisteredUsers';
DELETE FROM `tn_PermissionItemsInUserRoles` WHERE `ItemKey` = 'Bar_CreatePost' and `RoleName` = 'RegisteredUsers';
INSERT `tn_PermissionItemsInUserRoles` (`RoleName`, `ItemKey`, `PermissionType`, `PermissionQuota`, `PermissionScope`, `IsLocked`) VALUES ( 'RegisteredUsers', 'Bar_CreateThread', 1, 0, 0, 0);
INSERT `tn_PermissionItemsInUserRoles` (`RoleName`, `ItemKey`, `PermissionType`, `PermissionQuota`, `PermissionScope`, `IsLocked`) VALUES ( 'RegisteredUsers', 'Bar_CreatePost', 1, 0, 0, 0);


-- 动态初始化数据
DELETE FROM  `tn_ActivityItems` WHERE `ApplicationId` = 1012;
INSERT `tn_ActivityItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `Description`, `IsOnlyOnce`, `IsUserReceived`, `IsSiteReceived`) VALUES ('CreateBarPost', 1012, '发布回帖', 2, '', 0, 1, 0);
INSERT `tn_ActivityItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `Description`, `IsOnlyOnce`, `IsUserReceived`, `IsSiteReceived`) VALUES ('CreateBarRating', 1012, '帖子评分', 3, '有人对帖子进行评分时，会产生此动态', 0, 1, 0);
INSERT `tn_ActivityItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `Description`, `IsOnlyOnce`, `IsUserReceived`, `IsSiteReceived`) VALUES ('CreateBarThread', 1012, '发布帖子', 1, '当被关注的帖吧或用户有新帖子发布时，会收到此动态', 0, 1, 1);

-- 审核
DELETE FROM `tn_AuditItems` WHERE `ApplicationId` = 1012;
INSERT `tn_AuditItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `Description`) VALUES ('Bar_Post', 1012, '回帖', 8, '');
INSERT `tn_AuditItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `Description`) VALUES ('Bar_Section', 1012, '创建帖吧', 6, '');
INSERT `tn_AuditItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `Description`) VALUES ('Bar_Thread', 1012, '发帖', 7, '');

-- 审核规则
INSERT `tn_AuditItemsInUserRoles`(`RoleName`,`ItemKey` ,`StrictDegree`,`IsLocked`)VALUES('RegisteredUsers','Bar_Section',2 ,0);
INSERT `tn_AuditItemsInUserRoles`(`RoleName`,`ItemKey` ,`StrictDegree`,`IsLocked`)VALUES('ModeratedUser','Bar_Section',2 ,0);

-- 积分
DELETE FROM `tn_PointItems` WHERE `ApplicationId`=1012;
INSERT `tn_PointItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `ExperiencePoints`, `ReputationPoints`, `TradePoints`, `TradePoints2`, `TradePoints3`, `TradePoints4`, `Description`,`NeedPointMessage`) VALUES ('Bar_CreateThread', 1012, '创建帖子', 120, 5, 1, 5, 0, 0, 0, '',1);
INSERT `tn_PointItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `ExperiencePoints`, `ReputationPoints`, `TradePoints`, `TradePoints2`, `TradePoints3`, `TradePoints4`, `Description`,`NeedPointMessage`) VALUES ('Bar_DeleteThread', 1012, '删除帖子', 121, -5, -1, -5, 0, 0, 0, '',1);

-- 租户类型
DELETE FROM `tn_TenantTypes` WHERE `ApplicationId` = 1012;
INSERT `tn_TenantTypes` (`TenantTypeId`, `ApplicationId`, `Name`, `ClassType`) VALUES ('101200', 1012, '帖吧', '');
INSERT `tn_TenantTypes` (`TenantTypeId`, `ApplicationId`, `Name`, `ClassType`) VALUES ('101201', 1012, '帖吧', 'Spacebuilder.Bar.BarSection,Spacebuilder.Bar');
INSERT `tn_TenantTypes` (`TenantTypeId`, `ApplicationId`, `Name`, `ClassType`) VALUES ('101202', 1012, '帖子', 'Spacebuilder.Bar.BarThread,Spacebuilder.Bar');

-- 租户使用到的服务
DELETE FROM `tn_TenantTypesInServices` WHERE `TenantTypeId`in ('101201','101202','101203');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('101201', 'SiteCategory');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('101201', 'Subscribe');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('101201', 'Recommend');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('101201', 'Count');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('101202', 'Attachment');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('101202', 'AtUser');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('101202', 'OwnerCategory');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('101202', 'Tag');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('101202', 'Comment');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('101202', 'Count');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('101202', 'Recommend');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('101203', 'Attachment');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('101203', 'AtUser');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ( '101202', 'UserCategory');

-- 自运行任务
DELETE FROM `tn_TaskDetails` WHERE `ClassType` = 'Spacebuilder.Bar.ExpireStickyThreadsTask,Spacebuilder.Bar';
INSERT `tn_TaskDetails` (`Name`, `TaskRule`, `ClassType`, `Enabled`, `RunAtRestart`, `IsRunning`, `LastStart`, `LastEnd`, `LastIsSuccess`, `NextStart`, `StartDate`, `EndDate`, `RunAtServer`) VALUES ('更新置顶时间到期的帖子', '0 0 0/12 * * ?', 'Spacebuilder.Bar.ExpireStickyThreadsTask,Spacebuilder.Bar', 1, 0, 0, null, null, 1, null, null, NULL, 0);

-- 推荐类别
DELETE FROM `tn_RecommendItemTypes` WHERE `TenantTypeId` in ('101201','101202');
INSERT `tn_RecommendItemTypes` (`TypeId`, `TenantTypeId`, `Name`, `Description`, `HasFeaturedImage`, `DateCreated`) VALUES ('10120101', '101201', '推荐帖吧', '', 0, '2013/06/26');
INSERT `tn_RecommendItemTypes` (`TypeId`, `TenantTypeId`, `Name`, `Description`, `HasFeaturedImage`, `DateCreated`) VALUES ('10120201', '101202', '推荐帖子幻灯片', '', 1, '2013/06/26');

-- 类别
DELETE FROM `tn_Categories` WHERE `TenantTypeId` = '101201';
INSERT `tn_Categories` (`CategoryId`, `ParentId`, `OwnerId`, `TenantTypeId`, `CategoryName`, `Description`, `DisplayOrder`, `Depth`, `ChildCount`, `ItemCount`, `PrivacyStatus`, `AuditStatus`, `FeaturedItemId`, `LastModified`, `DateCreated`, `PropertyNames`, `PropertyValues`) VALUES (1, 0, 0, '101201', '默认类别', '', 1, 0, 0, 0, 2, 40, 0, '2013/06/26', '2013/06/26', NULL, NULL);

-- 广告位
DELETE FROM `tn_AdvertisingPosition` WHERE `PositionId` like '101012%';
INSERT `tn_AdvertisingPosition` (`PositionId`, `PresentAreaKey`, `Description`, `FeaturedImage`, `Width`, `Height`, `IsEnable`) VALUES ('10101200001', 'Channel', '贴吧频道中部广告位(710x100)', 'AdvertisingPosition\\00001\\01012\\00001\\10101200001.jpg', 710, 100, 1);
INSERT `tn_AdvertisingPosition` (`PositionId`, `PresentAreaKey`, `Description`, `FeaturedImage`, `Width`, `Height`, `IsEnable`) VALUES ('10101200002', 'Channel', '贴吧详细显示页中部广告位(710x70)', 'AdvertisingPosition\\00001\\01012\\00002\\10101200002.jpg', 710, 70, 1);
INSERT `tn_AdvertisingPosition` (`PositionId`, `PresentAreaKey`, `Description`, `FeaturedImage`, `Width`, `Height`, `IsEnable`) VALUES ('10101200003', 'Channel', '贴吧详细显示页中部广告位(230x260)', 'AdvertisingPosition\\00001\\01012\\00003\\10101200003.jpg', 230, 260, 1);
INSERT `tn_AdvertisingPosition` (`PositionId`, `PresentAreaKey`, `Description`, `FeaturedImage`, `Width`, `Height`, `IsEnable`) VALUES ('10101200004', 'Channel', '帖子详细显示页左中部广告位(230x60)', 'AdvertisingPosition\\00001\\01012\\00004\\10101200004.jpg', 230, 60, 1);
INSERT `tn_AdvertisingPosition` (`PositionId`, `PresentAreaKey`, `Description`, `FeaturedImage`, `Width`, `Height`, `IsEnable`) VALUES ('10101200005', 'Channel', '帖子详细显示页左下部广告位(230x260)', 'AdvertisingPosition\\00001\\01012\\00005\\10101200005.jpg', 230, 260, 1);
INSERT `tn_AdvertisingPosition` (`PositionId`, `PresentAreaKey`, `Description`, `FeaturedImage`, `Width`, `Height`, `IsEnable`) VALUES ('10101200006', 'Channel', '帖子详细显示页中部广告位(710x70)', 'AdvertisingPosition\\00001\\01012\\00006\\10101200006.jpg', 710, 70, 1);