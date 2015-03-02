-- 应用
DELETE FROM `tn_Applications` WHERE `ApplicationId` = 1011;
INSERT `tn_Applications` (`ApplicationId`, `ApplicationKey`, `Description`, `IsEnabled`, `IsLocked`, `DisplayOrder`) VALUES (1011, 'Group', '群组应用', 1, 0, 1011);

-- 应用在呈现区域相关设置
DELETE FROM `tn_ApplicationInPresentAreaSettings` WHERE `ApplicationId` = 1011;
INSERT `tn_ApplicationInPresentAreaSettings` (`ApplicationId`, `PresentAreaKey`, `IsBuiltIn`, `IsAutoInstall`, `IsGenerateData`) VALUES (1011, 'Channel', 0, 1, 1);
INSERT `tn_ApplicationInPresentAreaSettings` (`ApplicationId`, `PresentAreaKey`, `IsBuiltIn`, `IsAutoInstall`, `IsGenerateData`) VALUES (1011, 'UserSpace', 0, 1, 0);

-- 快捷操作
DELETE FROM `tn_ApplicationManagementOperations` WHERE `ApplicationId` = 1011;
INSERT `tn_ApplicationManagementOperations` (`OperationId`, `ApplicationId`, `AssociatedNavigationId`, `PresentAreaKey`, `OperationType`, `OperationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (10101101, 1011, 0, 'Channel', 1, '创建群组', '', '', 'Channel_Group_Create', NULL, 'UserRelation', NULL, '_blank', 10101101, 0, 1, 1);
INSERT `tn_ApplicationManagementOperations` (`OperationId`, `ApplicationId`, `AssociatedNavigationId`, `PresentAreaKey`, `OperationType`, `OperationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (20101101, 1011, 0, 'ControlPanel', 1, '群组类别管理', '', '', 'ControlPanel_Content_ManageSiteCategories', 'tenantTypeId', NULL, NULL, '_blank', 20101101, 1, 0, 1);

-- 导航
DELETE FROM `tn_InitialNavigations` WHERE `ApplicationId` = 1011;
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (10101101, 0, 0, 'Channel', 1011, 0, '群组', '', '', N'Channel_Group_Home', NULL, 'Group', NULL, '_self', 10101101, 0, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (10101102, 10101101, 1, 'Channel', 1011, 0, '群组首页', ' ', ' ', 'Channel_Group_Home', NULL, NULL, NULL, '_self', 10101102, 0, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (10101103, 10101101, 1, 'Channel', 1011, 0, '我的群组', '', '', 'Channel_Group_UserGroups', 'spaceKey', NULL, NULL, '_self', 10101103, 0, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (10101104, 10101101, 1, 'Channel', 1011, 0, '发现群组', '', '', 'Channel_Group_FindGroup', NULL, NULL, NULL, '_self', 10101104, 0, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (11101101, 0, 0, 'UserSpace', 1011, 0, '群组', ' ', ' ', 'Channel_Group_UserGroups', 'spaceKey', 'Group', NULL, '_self', 11101101, 0, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (13900180, 0, 0, 'GroupSpace', 0, 1, '成员', ' ', ' ', 'GroupSpace_Member', NULL, NULL, NULL, '_self', 13101280, 0, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (13900190, 0, 0, 'GroupSpace', 0, 1, '管理', ' ', ' ', 'Group_Bar_ManageThreads', NULL, NULL, NULL, '_self', 13101190, 0, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (20101101, 20000011, 2, N'ControlPanel', 1011, 0, '群组', '', '', 'ControlPanel_Group_Home', NULL, NULL, NULL, '_self', 20101101, 0, 0, 1);

-- 默认安装记录
DELETE FROM `tn_ApplicationInPresentAreaInstallations` WHERE `ApplicationId` = 1011 and OwnerId = 0;
INSERT `tn_ApplicationInPresentAreaInstallations` (`OwnerId`, `ApplicationId`, `PresentAreaKey`) VALUES (0, 1011, 'Channel');

-- 动态
DELETE FROM  `tn_ActivityItems` WHERE `ApplicationId` = 1011;
INSERT `tn_ActivityItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `Description`, `IsOnlyOnce`, `IsUserReceived`, `IsSiteReceived`) VALUES ('CreateGroup', 1011, '创建群组', 0, N'', 0, 1, 1);
INSERT `tn_ActivityItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `Description`, `IsOnlyOnce`, `IsUserReceived`, `IsSiteReceived`) VALUES ('JoinGroup', 1011, '加入群组', 0, N'', 0, 1, 0);
INSERT `tn_ActivityItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `Description`, `IsOnlyOnce`, `IsUserReceived`, `IsSiteReceived`) VALUES ('CreateGroupMember', 1011, '新成员加入', 0, '', 1, 0, 0);

-- 用户角色
DELETE FROM `tn_Roles` WHERE `ApplicationId` = 1011;
INSERT `tn_Roles` (`RoleName`, `FriendlyRoleName`, `IsBuiltIn`, `ConnectToUser`, `ApplicationId`, `IsPublic`, `Description`, `IsEnabled`, `RoleImage`) VALUES ('GroupAdministrator', '群组管理员', 1, 1, 1011, 1, '管理群组应用下的内容', 1, '');

-- 审核
DELETE FROM `tn_AuditItems` WHERE `ApplicationId` = 1011;
INSERT `tn_AuditItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `Description`) VALUES ('Group', 1011, '创建群组', 11, '');

-- 审核规则
INSERT `tn_AuditItemsInUserRoles`(`RoleName`,`ItemKey` ,`StrictDegree`,`IsLocked`)VALUES('RegisteredUsers',N'Group',2 ,0);
INSERT `tn_AuditItemsInUserRoles`(`RoleName`,`ItemKey` ,`StrictDegree`,`IsLocked`)VALUES('ModeratedUser',N'Group',2 ,0);

-- 积分
DELETE FROM `tn_PointItems` WHERE `ApplicationId`=1011;
INSERT `tn_PointItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `ExperiencePoints`, `ReputationPoints`, `TradePoints`, `TradePoints2`, `TradePoints3`, `TradePoints4`, `Description`,`NeedPointMessage`) VALUES ('Group_CreateGroup', 1011, '创建群组', 101, 100, 50, 50, 0, 0, 0, '',1);
INSERT `tn_PointItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `ExperiencePoints`, `ReputationPoints`, `TradePoints`, `TradePoints2`, `TradePoints3`, `TradePoints4`, `Description`,`NeedPointMessage`) VALUES ('Group_DeleteGroup', 1011, '删除群组', 102, -100, -50, -50, 0, 0, 0, '',0);

-- 租户类型
DELETE FROM `tn_TenantTypes` WHERE `ApplicationId`=1011;
INSERT `tn_TenantTypes` (`TenantTypeId`, `ApplicationId`, `Name`, `ClassType`) VALUES ('101100', 1011, '群组', 'Spacebuilder.Group.GroupEntity,Spacebuilder.Group');

-- 租户使相关服务
DELETE FROM `tn_TenantTypesInServices` WHERE `TenantTypeId`='101100';
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('101100', 'Count');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('101100', 'SiteCategory');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('101100', 'Tag');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('101100', 'Recommend');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('101100', 'Visit');

-- 自运行任务
DELETE FROM `tn_TaskDetails` WHERE `ClassType`='Spacebuilder.Group.CalculateGrowthValuesTask,Spacebuilder.Group';
INSERT `tn_TaskDetails` (`Name`, `TaskRule`, `ClassType`, `Enabled`, `RunAtRestart`, `IsRunning`, `LastStart`, `LastEnd`, `LastIsSuccess`, `NextStart`, `StartDate`, `EndDate`, `RunAtServer`) VALUES ('更新群组的成长值', '0 0 0/12 * * ?', 'Spacebuilder.Group.CalculateGrowthValuesTask,Spacebuilder.Group', 1, 0, 0, null, null, 1, null, null, NULL, 0);

-- 推荐
DELETE FROM `tn_RecommendItemTypes` WHERE `TypeId` IN ('00001111','10110001');
INSERT `tn_RecommendItemTypes` (`TypeId`, `TenantTypeId`, `Name`, `Description`, `HasFeaturedImage`, `DateCreated`) VALUES ('00001111', '000011', '推荐群主', '推荐群主', 0, '2013/06/26');
INSERT `tn_RecommendItemTypes` (`TypeId`, `TenantTypeId`, `Name`, `Description`, `HasFeaturedImage`, `DateCreated`) VALUES ('10110001', '101100', '推荐群组', '推荐群组', 0, '2013/06/26');

-- 类别
DELETE FROM `tn_Categories` WHERE `TenantTypeId` = '101100';
INSERT `tn_Categories` (`CategoryId`, `ParentId`, `OwnerId`, `TenantTypeId`, `CategoryName`, `Description`, `DisplayOrder`, `Depth`, `ChildCount`, `ItemCount`, `PrivacyStatus`, `AuditStatus`, `FeaturedItemId`, `LastModified`, `DateCreated`, `PropertyNames`, `PropertyValues`) VALUES (28, 0, 0, '101100', '默认类别', '', 28, 0, 0, 0, 2, 40, 0, '2013/06/26', '2013/06/26', NULL, NULL);

-- 广告位
DELETE FROM `tn_AdvertisingPosition` WHERE `PositionId` like '101011%' or `PositionId` like '131011%';
INSERT `tn_AdvertisingPosition` (`PositionId`, `PresentAreaKey`, `Description`, `FeaturedImage`, `Width`, `Height`, `IsEnable`) VALUES ('10101100001', 'Channel', '群组频道首页中上部广告位(550x190)', 'AdvertisingPosition\\00001\\01011\\00001\\10101100001.jpg', 550, 190, 1);
INSERT `tn_AdvertisingPosition` (`PositionId`, `PresentAreaKey`, `Description`, `FeaturedImage`, `Width`, `Height`, `IsEnable`) VALUES ('10101100002', 'Channel', '群组频道首页左中部广告位(190x70)', 'AdvertisingPosition\\00001\\01011\\00002\\10101100002.jpg', 190, 70, 1);
INSERT `tn_AdvertisingPosition` (`PositionId`, `PresentAreaKey`, `Description`, `FeaturedImage`, `Width`, `Height`, `IsEnable`) VALUES ('13101100003', 'GroupSpace', '群组详细显示页中部广告位(710x100)', 'AdvertisingPosition\\00001\\31011\\00003\\13101100003.jpg', 710, 100, 1);
INSERT `tn_AdvertisingPosition` (`PositionId`, `PresentAreaKey`, `Description`, `FeaturedImage`, `Width`, `Height`, `IsEnable`) VALUES ('13101100004', 'GroupSpace', '群组讨论详细显示页中部广告位(710x70)', 'AdvertisingPosition\\00001\\31011\\00004\\13101100004.jpg', 710, 70, 1);
INSERT `tn_AdvertisingPosition` (`PositionId`, `PresentAreaKey`, `Description`, `FeaturedImage`, `Width`, `Height`, `IsEnable`) VALUES ('13101100005', 'GroupSpace', '群组讨论详细显示页右下部广告位(230x260)', 'AdvertisingPosition\\00001\\31011\\00005\\13101100005.jpg', 230, 260, 1);