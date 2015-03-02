-- 应用
DELETE FROM `tn_Applications` WHERE `ApplicationId` = 1002;
INSERT `tn_Applications` (`ApplicationId`, `ApplicationKey`, `Description`, `IsEnabled`, `IsLocked`, `DisplayOrder`) VALUES (1002, 'Blog', '日志应用', 1, 0, 1002);

-- 应用在呈现区域的设置
DELETE FROM `tn_ApplicationInPresentAreaSettings` WHERE `ApplicationId` = 1002;
INSERT `tn_ApplicationInPresentAreaSettings` (`ApplicationId`, `PresentAreaKey`, `IsBuiltIn`, `IsAutoInstall`, `IsGenerateData`) VALUES (1002, 'Channel', 0, 1, 0);
INSERT `tn_ApplicationInPresentAreaSettings` (`ApplicationId`, `PresentAreaKey`, `IsBuiltIn`, `IsAutoInstall`, `IsGenerateData`) VALUES (1002, 'UserSpace', 0, 1, 1);

-- 默认安装记录
DELETE FROM `tn_ApplicationInPresentAreaInstallations` WHERE `ApplicationId` = 1002 and OwnerId = 0;
INSERT `tn_ApplicationInPresentAreaInstallations` (`OwnerId`, `ApplicationId`, `PresentAreaKey`) VALUES (0, 1002, 'Channel');

-- 审核
DELETE FROM `tn_AuditItems` WHERE `ApplicationId` = 1002;
INSERT `tn_AuditItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `Description`) VALUES ('Blog_Thread', 1002, '撰写日志', 5, '');

-- 积分
DELETE FROM `tn_PointItems` WHERE `ApplicationId`=1002;
INSERT `tn_PointItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `ExperiencePoints`, `ReputationPoints`, `TradePoints`, `TradePoints2`, `TradePoints3`, `TradePoints4`, `Description`,`NeedPointMessage`) VALUES ('Blog_CreateThread', 1002, '创建日志', 110, 5, 1, 5, 0, 0, 0, '',1);
INSERT `tn_PointItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `ExperiencePoints`, `ReputationPoints`, `TradePoints`, `TradePoints2`, `TradePoints3`, `TradePoints4`, `Description`,`NeedPointMessage`) VALUES ('Blog_DeleteThread', 1002, '删除日志', 111, -5, -1, -5, 0, 0, 0, '',1);

-- 快捷操作
DELETE FROM `tn_ApplicationManagementOperations` WHERE `ApplicationId` = 1002;
INSERT `tn_ApplicationManagementOperations` (`OperationId`, `ApplicationId`, `AssociatedNavigationId`, `PresentAreaKey`, `OperationType`, `OperationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (10100201, 1002, 0, 'Channel', 1, '撰写日志', '', '', 'UserSpace_Blog_Create', 'spaceKey', 'Write', NULL, '_blank', 10100202, 1, 0, 1);
INSERT `tn_ApplicationManagementOperations` (`OperationId`, `ApplicationId`, `AssociatedNavigationId`, `PresentAreaKey`, `OperationType`, `OperationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (11100201, 1002, 0, 'UserSpace', 1, '撰写日志', ' ', ' ', 'UserSpace_Blog_Create', NULL, 'Write', '', '_self', 11100201, 1, 1, 1);

-- 导航
DELETE FROM `tn_InitialNavigations` WHERE `ApplicationId` = 1002;
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (10100201, 0, 0, 'Channel', 1002, 0, '日志', '', '', 'Channel_Blog_Home', NULL, 'Blog', NULL, '_self', 10100201, 0, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (10100202, 10100201, 1, 'Channel', 1002, 0, '日志首页', '', '', 'Channel_Blog_Home', NULL, NULL, NULL, '_self', 10100202, 0, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (10100203, 10100201, 1, 'Channel', 1002, 0, '我的日志', '', '', 'UserSpace_Blog_Blog', 'spaceKey', NULL, NULL, '_self', 10100203, 0, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (11100201, 0, 0, 'UserSpace', 1002, 0, '日志', ' ', ' ', 'UserSpace_Blog_Home', NULL, 'Blog', NULL, '_self', 11100201, 0, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (11100202, 11100201, 1, 'UserSpace', 1002, 0, '日志首页', ' ', ' ', 'UserSpace_Blog_Home', NULL, NULL, NULL, '_self', 11100202, 1, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (11100203, 11100201, 1, 'UserSpace', 1002, 0, '我的日志', ' ', ' ', 'UserSpace_Blog_Blog', NULL, NULL, NULL, '_self', 11100203, 1, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (11100204, 11100201, 1, 'UserSpace', 1002, 0, '我的关注', ' ', ' ', 'UserSpace_Blog_Subscribed', NULL, NULL, NULL, '_self', 11100204, 1, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (20100201, 20000011, 2, 'ControlPanel', 1002, 0, '日志', '', '', 'ControlPanel_Blog_Home', NULL, NULL, NULL, '_self', 20100201, 0, 0, 1);

-- 动态初始化数据
DELETE FROM  `tn_ActivityItems` WHERE `ApplicationId` = 1002;
INSERT `tn_ActivityItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `Description`, `IsOnlyOnce`, `IsUserReceived`, `IsSiteReceived`) VALUES ('CreateBlogComment', 1002, '日志评论', 2, '', 0, 1, 0);
INSERT `tn_ActivityItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `Description`, `IsOnlyOnce`, `IsUserReceived`, `IsSiteReceived`) VALUES ('CreateBlogThread', 1002, '发布日志', 1, '', 0, 1, 1);


-- 用户角色
DELETE FROM `tn_Roles` WHERE `ApplicationId` = 1002;
INSERT `tn_Roles` (`RoleName`, `FriendlyRoleName`, `IsBuiltIn`, `ConnectToUser`, `ApplicationId`, `IsPublic`, `Description`, `IsEnabled`, `RoleImage`) VALUES ('BlogAdministrator', '日志管理员', 1, 1, 1002, 1, '管理日志应用下的内容', 1, '');

-- 权限项
DELETE FROM `tn_PermissionItems` WHERE `ApplicationId` = 1002;
INSERT `tn_PermissionItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `EnableQuota`, `EnableScope`) VALUES ('Blog_Create', 1002, '发布日志', 2, 0, 0);
-- 角色针对权限的设置
DELETE FROM `tn_PermissionItemsInUserRoles` WHERE `ItemKey` = 'Blog_Create' and `RoleName` = 'RegisteredUsers';
INSERT `tn_PermissionItemsInUserRoles` (`RoleName`, `ItemKey`, `PermissionType`, `PermissionQuota`, `PermissionScope`, `IsLocked`) VALUES ( 'RegisteredUsers', 'Blog_Create', 1, 0, 0, 0);

-- 审核初始化数据
DELETE FROM `tn_AuditItems` WHERE `ApplicationId` = 1002;
INSERT INTO `tn_AuditItems` (`ItemKey`,`ApplicationId`,`ItemName`,`DisplayOrder`,`Description`) VALUES ('Blog_Thread',1002,'日志',1,'');

-- 租户类型
DELETE FROM `tn_TenantTypes` WHERE `ApplicationId`=1002;
INSERT `tn_TenantTypes` (`TenantTypeId`, `ApplicationId`, `Name`, `ClassType`) VALUES ('100200', 1002, '日志应用', '');
INSERT `tn_TenantTypes` (`TenantTypeId`, `ApplicationId`, `Name`, `ClassType`) VALUES ('100201', 1002, '日志', 'Spacebuilder.Blog.BlogThread,Spacebuilder.Blog');

-- 租户使用到的服务
DELETE FROM `tn_TenantTypesInServices` WHERE `TenantTypeId`='100201';
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('100201', 'Attachment');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('100201', 'Attitude');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('100201', 'AtUser');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('100201', 'Comment');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('100201', 'OwnerCategory');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('100201', 'SiteCategory');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('100201', 'UserCategory');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('100201', 'Subscribe');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('100201', 'Recommend');
INSERT `tn_TenantTypesInServices` (`TenantTypeId`, `ServiceKey`) VALUES ('100201', 'Tag');

-- 推荐类别
DELETE FROM `tn_RecommendItemTypes` WHERE `TypeId` in ('10020101','10020102','00001102');
INSERT `tn_RecommendItemTypes` (`TypeId`, `TenantTypeId`, `Name`, `Description`, `HasFeaturedImage`, `DateCreated`) VALUES ('10020101', '100201', '推荐日志', '标题列表', 0, '2013/06/26');
INSERT `tn_RecommendItemTypes` (`TypeId`, `TenantTypeId`, `Name`, `Description`, `HasFeaturedImage`, `DateCreated`) VALUES ('10020102', '100201', '推荐日志幻灯片', '频道日志首页幻灯片日志', 1, '2013/06/26');
INSERT `tn_RecommendItemTypes` (`TypeId`, `TenantTypeId`, `Name`, `Description`, `HasFeaturedImage`, `DateCreated`) VALUES ('00001102', '000011', '推荐博主', '推荐博主', 0, '2013/06/26');

-- 类别
DELETE FROM `tn_Categories` WHERE `TenantTypeId` = '100201';
INSERT `tn_Categories` (`CategoryId`, `ParentId`, `OwnerId`, `TenantTypeId`, `CategoryName`, `Description`, `DisplayOrder`, `Depth`, `ChildCount`, `ItemCount`, `PrivacyStatus`, `AuditStatus`, `FeaturedItemId`, `LastModified`, `DateCreated`, `PropertyNames`, `PropertyValues`) VALUES (40, 0, 0, '100201', '默认类别', '', 40, 0, 0, 0, 2, 40, 0, '2013/06/26', '2013/06/26', NULL, NULL);

-- 广告位
DELETE FROM `tn_AdvertisingPosition` WHERE `PositionId` like '101002%';
INSERT `tn_AdvertisingPosition` (`PositionId`, `PresentAreaKey`, `Description`, `FeaturedImage`, `Width`, `Height`, `IsEnable`) VALUES ('10100200001', 'Channel', '日志频道首页头部广告位(950x100)', 'AdvertisingPosition\\00001\\01002\\00001\\10100200001.jpg', 950, 100, 1);
INSERT `tn_AdvertisingPosition` (`PositionId`, `PresentAreaKey`, `Description`, `FeaturedImage`, `Width`, `Height`, `IsEnable`) VALUES ('10100200002', 'Channel', '日志频道首页右中部广告位(230x260)', 'AdvertisingPosition\\00001\\01002\\00002\\10100200002.jpg', 230, 260, 1);
INSERT `tn_AdvertisingPosition` (`PositionId`, `PresentAreaKey`, `Description`, `FeaturedImage`, `Width`, `Height`, `IsEnable`) VALUES ('10100200003', 'Channel', '日志详细显示中部广告位(740x50)', 'AdvertisingPosition\\00001\\01002\\00003\\10100200003.jpg', 740, 50, 1);