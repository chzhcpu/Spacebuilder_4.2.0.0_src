-- 应用数据
DELETE FROM `tn_Applications` WHERE `ApplicationId` = 1001;
INSERT `tn_Applications` (`ApplicationId`, `ApplicationKey`, `Description`, `IsEnabled`, `IsLocked`, `DisplayOrder`) VALUES (1001, 'Microblog', '微博应用', 1, 1, 1001);

-- 应用在呈现区域的设置
DELETE FROM `tn_ApplicationInPresentAreaSettings` WHERE `ApplicationId` = 1001;
INSERT `tn_ApplicationInPresentAreaSettings` (`ApplicationId`, `PresentAreaKey`, `IsBuiltIn`, `IsAutoInstall`, `IsGenerateData`) VALUES (1001, 'Channel', 1, 1, 0);
INSERT `tn_ApplicationInPresentAreaSettings` (`ApplicationId`, `PresentAreaKey`, `IsBuiltIn`, `IsAutoInstall`, `IsGenerateData`) VALUES (1001, 'UserSpace', 1, 1, 1);
INSERT `tn_ApplicationInPresentAreaSettings` (`ApplicationId`, `PresentAreaKey`, `IsBuiltIn`, `IsAutoInstall`, `IsGenerateData`) VALUES (1001, 'GroupSpace', 1, 1, 1);

-- 默认安装记录
DELETE FROM `tn_ApplicationInPresentAreaInstallations` WHERE `ApplicationId` = 1001 and OwnerId = 0;
INSERT `tn_ApplicationInPresentAreaInstallations` (`OwnerId`, `ApplicationId`, `PresentAreaKey`) VALUES (0, 1001, 'Channel');

-- 租户类型与服务的关系
DELETE FROM `tn_TenantTypesInServices` WHERE `TenantTypeId` = '100101';
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('100101', 'Comment');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('100101', 'Recommend');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('100101', 'Tag');

-- 导航
DELETE FROM `tn_InitialNavigations` WHERE `ApplicationId` = 1001;
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (10100101, 0, 0, 'Channel', 1001, 0, '微博', '', '', 'Channel_Microblog', NULL, 'Microblog', NULL, '_self', 10100101, 0, 1, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (11100101, 0, 0, 'UserSpace', 1001, 0, '微博', '', '', 'UserSpace_Microblog_Home', NULL, 'Microblog', NULL, '_self', 11100101, 0, 1, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (11100102, 11100101, 1, 'UserSpace', 1001, 0, '我的微博', '', '', 'UserSpace_Microblog_Home', NULL, NULL, NULL, '_self', 11100102, 0, 1, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (11100103, 11100101, 1, 'UserSpace', 1001, 0, '提到我的', ' ', ' ', 'UserSpace_Microblog_AtMe', NULL, NULL, NULL, '_self', 11100103, 0, 1, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (11100104, 11100101, 1, 'UserSpace', 1001, 0, '我的收藏', '', '', 'UserSpace_Microblog_Favorites', NULL, NULL, NULL, '_self', 11100104, 0, 1, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (20100101, 20000011, 2, 'ControlPanel', 1001, 0, '微博', ' ', ' ', 'ControlPanel_Microblog_Home', NULL, NULL, NULL, '_self', 20100101, 0, 1, 1);

-- 用户角色
DELETE FROM `tn_Roles` WHERE `ApplicationId` = 1001;
INSERT `tn_Roles` (`RoleName`, `FriendlyRoleName`, `IsBuiltIn`, `ConnectToUser`, `ApplicationId`, `IsPublic`, `Description`, `IsEnabled`, `RoleImage`) VALUES (N'MicroblogAdministrator', '微博管理员', 1, 1, 1001, 1, '管理微博应用下的内容', 1, '');

-- 权限项
DELETE FROM `tn_PermissionItems` WHERE `ApplicationId` = 1001;
INSERT `tn_PermissionItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `EnableQuota`, `EnableScope`) VALUES ('Microblog_Create', 1001, '发布微博', 1, 0, 0);
-- 角色针对权限的设置
DELETE FROM `tn_PermissionItemsInUserRoles` WHERE `ItemKey` = 'Microblog_Create' and `RoleName` = 'RegisteredUsers';
INSERT `tn_PermissionItemsInUserRoles` (`RoleName`, `ItemKey`, `PermissionType`, `PermissionQuota`, `PermissionScope`, `IsLocked`) VALUES ( 'RegisteredUsers', 'Microblog_Create', 1, 0, 0, 0);

-- 积分
DELETE FROM `tn_PointItems` WHERE `ApplicationId`=1001;
INSERT `tn_PointItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `ExperiencePoints`, `ReputationPoints`, `TradePoints`, `TradePoints2`, `TradePoints3`, `TradePoints4`, `Description`,`NeedPointMessage`) VALUES ('Microblog_CreateMicroblog', 1001, '创建微博', 150, 2, 0, 2, 0, 0, 0, '',1);
INSERT `tn_PointItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `ExperiencePoints`, `ReputationPoints`, `TradePoints`, `TradePoints2`, `TradePoints3`, `TradePoints4`, `Description`,`NeedPointMessage`) VALUES ('Microblog_DeleteMicroblog', 1001, '删除微博', 151, -2, 0, -2, 0, 0, 0, '',1);

-- 动态
DELETE FROM  `tn_ActivityItems` WHERE `ApplicationId` = 1001;
INSERT `tn_ActivityItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `Description`, `IsOnlyOnce`, `IsUserReceived`, `IsSiteReceived`) VALUES ('CreateMicroblog', 1001, '发布微博', 1, '', 0, 1, 1);

-- 审核
DELETE FROM `tn_AuditItems` WHERE `ApplicationId` = 1001;
INSERT `tn_AuditItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `Description`) VALUES ('Microblog_Comment', 1001, '评论微博', 3, ' ');
INSERT `tn_AuditItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `Description`) VALUES ('Microblog_Create', 1001, '创建微博', 4, ' ');

-- 推荐类别
DELETE FROM `tn_RecommendItemTypes` WHERE `TenantTypeId` = '100101';
INSERT INTO `tn_RecommendItemTypes`(`TypeId`,`TenantTypeId`,`Name`,`Description`,`HasFeaturedImage`,`DateCreated`)VALUES('10010101','100101','推荐话题','推荐话题',0,'2013/06/26');

-- 租户类型
DELETE FROM `tn_TenantTypes` WHERE `ApplicationId` = 1001;
INSERT `tn_TenantTypes` (`TenantTypeId`, `ApplicationId`, `Name`, `ClassType`) VALUES ('100101', 1001, '微博', 'Spacebuilder.Microblog.MicroblogEntity,Spacebuilder.Microblog');
