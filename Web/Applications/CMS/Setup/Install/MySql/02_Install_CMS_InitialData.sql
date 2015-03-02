-- 添加应用数据
DELETE FROM `tn_Applications` WHERE `ApplicationId` = 1015;
INSERT `tn_Applications` (`ApplicationId`, `ApplicationKey`, `Description`, `IsEnabled`, `IsLocked`, `DisplayOrder`) VALUES (1015, 'CMS', '资讯应用', 1, 0, 1015);

-- 应用在呈现区域的设置
DELETE FROM `tn_ApplicationInPresentAreaSettings` WHERE `ApplicationId` = 1015;
INSERT `tn_ApplicationInPresentAreaSettings` (`ApplicationId`, `PresentAreaKey`, `IsBuiltIn`, `IsAutoInstall`, `IsGenerateData`) VALUES (1015, 'Channel', 0, 1, 1);
INSERT `tn_ApplicationInPresentAreaSettings` (`ApplicationId`, `PresentAreaKey`, `IsBuiltIn`, `IsAutoInstall`, `IsGenerateData`) VALUES (1015, 'UserSpace', 0, 1, 0);

-- 默认安装记录
DELETE FROM `tn_ApplicationInPresentAreaInstallations` WHERE `ApplicationId` = 1015 and OwnerId = 0;
INSERT `tn_ApplicationInPresentAreaInstallations` (`OwnerId`, `ApplicationId`, `PresentAreaKey`) VALUES (0, 1015, 'Channel');

-- 快捷操作
DELETE FROM `tn_ApplicationManagementOperations` WHERE `ApplicationId` = 1015;
INSERT `tn_ApplicationManagementOperations` (`OperationId`, `ApplicationId`, `AssociatedNavigationId`, `PresentAreaKey`, `OperationType`, `OperationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (10101501, 1015, 0, 'Channel', 1, '投稿', '', '', 'Channel_CMS_Contribute', NULL, '', NULL, '_self', 10101501, 0, 1, 1);

-- 动态
DELETE FROM  `tn_ActivityItems` WHERE `ApplicationId` = 1015;
INSERT `tn_ActivityItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `Description`, `IsOnlyOnce`, `IsUserReceived`, `IsSiteReceived`) VALUES ('CreateCmsComment', 1015, '评论资讯', 1, '', 0, 1, 0);

-- 用户角色
DELETE FROM `tn_Roles` WHERE `ApplicationId` = 1015;
INSERT `tn_Roles` (`RoleName`, `FriendlyRoleName`, `IsBuiltIn`, `ConnectToUser`, `ApplicationId`, `IsPublic`, `Description`, `IsEnabled`, `RoleImage`) VALUES ('CMSAdministrator', '资讯管理员', 1, 1, 1015, 1, '管理资讯应用下的内容', 1, '');

-- 自运行任务
DELETE FROM `tn_TaskDetails` WHERE `ClassType` = 'Spacebuilder.CMS.ExpireStickyContentItemTask,Spacebuilder.CMS';
INSERT `tn_TaskDetails` (`Name`, `TaskRule`, `ClassType`, `Enabled`, `RunAtRestart`, `IsRunning`, `LastStart`, `LastEnd`, `LastIsSuccess`, `NextStart`, `StartDate`, `EndDate`, `RunAtServer`) VALUES ('更新置顶时间到期的资讯', '0 0 0/12 * * ?', 'Spacebuilder.CMS.ExpireStickyContentItemTask,Spacebuilder.CMS', 1, 0, 0, null, null, 1, null, null, NULL, 0);

-- 权限项
DELETE FROM `tn_PermissionItems` WHERE `ApplicationId` = 1015;
INSERT `tn_PermissionItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `EnableQuota`, `EnableScope`) VALUES ('CMS_ContentItem', 1015, '资讯投稿', 16, 0, 0);

-- 角色针对权限的设置
DELETE FROM `tn_PermissionItemsInUserRoles` WHERE `ItemKey` = 'CMS_ContentItem' and `RoleName` = 'RegisteredUsers';
INSERT `tn_PermissionItemsInUserRoles` (`RoleName`, `ItemKey`, `PermissionType`, `PermissionQuota`, `PermissionScope`, `IsLocked`) VALUES ( 'RegisteredUsers', 'CMS_ContentItem', 1, 0, 0, 0);

-- 审核项
DELETE FROM `tn_AuditItems` WHERE `ApplicationId` = 1015;
INSERT `tn_AuditItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `Description`) VALUES ('CMS_ContentItem', 1015, '资讯', 1, '');

-- 审核规则
DELETE FROM `tn_AuditItemsInUserRoles` WHERE `ItemKey` = 'CMS_ContentItem' and `RoleName` = 'RegisteredUsers';
INSERT `tn_AuditItemsInUserRoles`(`RoleName`,`ItemKey` ,`StrictDegree`,`IsLocked`)VALUES('RegisteredUsers','CMS_ContentItem',2 ,0);

-- 应用的审核规则
delete from `tn_ApplicationData` where `ApplicationId`=1015;
insert `tn_ApplicationData`(`ApplicationId` ,`TenantTypeId`,`Datakey`,`LongValue`,`DecimalValue` ,`StringValue`) values(1015,'','PubliclyAuditStatus',40,0.0000,'');

-- 积分项
DELETE FROM `tn_PointItems` WHERE `ApplicationId` = 1015;
INSERT `tn_PointItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `ExperiencePoints`, `ReputationPoints`, `TradePoints`, `TradePoints2`, `TradePoints3`, `TradePoints4`, `Description`,`NeedPointMessage`) VALUES ('CMS_ContributeAccepted', 1015, '投稿被采纳', 151, 20, 0, 20, 0, 0, 0, '',0);
INSERT `tn_PointItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `ExperiencePoints`, `ReputationPoints`, `TradePoints`, `TradePoints2`, `TradePoints3`, `TradePoints4`, `Description`,`NeedPointMessage`) VALUES ('CMS_ContributeDeleted', 1015, '投稿被删除', 151, -20, 0, -20, 0, 0, 0, '',0);
INSERT `tn_PointItems` (`ItemKey`, `ApplicationId`, `ItemName`, `DisplayOrder`, `ExperiencePoints`, `ReputationPoints`, `TradePoints`, `TradePoints2`, `TradePoints3`, `TradePoints4`, `Description`,`NeedPointMessage`) VALUES ('CMS_StickyNews', 1015, '投稿被置顶', 151, 100, 0, 100, 0, 0, 0, '',0);

-- 推荐类别
DELETE FROM `tn_RecommendItemTypes` WHERE `TypeId` in ('10150101','10150102');
INSERT `tn_RecommendItemTypes` (`TypeId`, `TenantTypeId`, `Name`, `Description`, `HasFeaturedImage`, `DateCreated`) VALUES ('10150101', '101501', '推荐资讯幻灯片', '', 1, now());
INSERT `tn_RecommendItemTypes` (`TypeId`, `TenantTypeId`, `Name`, `Description`, `HasFeaturedImage`, `DateCreated`) VALUES ('10150102', '000031', '精彩点评', '', 0, now());

-- 租户类型
DELETE FROM `tn_TenantTypes` WHERE TenantTypeId in ('101500','101501','101502' );
INSERT `tn_TenantTypes` (`TenantTypeId`, `ApplicationId`, `Name`, `ClassType`) VALUES ('101500', 1015, '资讯应用', '');
INSERT `tn_TenantTypes` (`TenantTypeId`, `ApplicationId`, `Name`, `ClassType`) VALUES ('101501', 1015, '资讯', 'Spacebuilder.CMS.ContentItem,Spacebuilder.CMS.Service');
INSERT `tn_TenantTypes` (`TenantTypeId`, `ApplicationId`, `Name`, `ClassType`) VALUES ('101502', 1015, '资讯附件','Spacebuilder.CMS.ContentAttachment,Spacebuilder.CMS.Service');

-- 租户使用到的服务
DELETE FROM `tn_TenantTypesInServices` WHERE `TenantTypeId` = '101501';
INSERT INTO `tn_TenantTypesInServices`(`TenantTypeId`,`ServiceKey`) VALUES('101501','Attachment');
INSERT INTO `tn_TenantTypesInServices`(`TenantTypeId`,`ServiceKey`) VALUES('101501','Tag');
INSERT INTO `tn_TenantTypesInServices`(`TenantTypeId`,`ServiceKey`) VALUES('101501','Notice');
INSERT INTO `tn_TenantTypesInServices`(`TenantTypeId`,`ServiceKey`) VALUES ('101501','Recommend');
INSERT INTO `tn_TenantTypesInServices`(`TenantTypeId`,`ServiceKey`) VALUES ('101501','Comment');
INSERT INTO `tn_TenantTypesInServices`(`TenantTypeId`,`ServiceKey`) VALUES ('101501','Attitude');
INSERT INTO `tn_TenantTypesInServices`(`TenantTypeId`,`ServiceKey`) VALUES ('101501','Visit');
INSERT INTO `tn_TenantTypesInServices`(`TenantTypeId`,`ServiceKey`) VALUES ('101501','Count');
INSERT INTO `tn_TenantTypesInServices`(`TenantTypeId`,`ServiceKey`) VALUES ('101502','Count');

-- 初始化导航
DELETE FROM `tn_InitialNavigations` WHERE `ApplicationId` = 1015;
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (10101501, 0, 0, 'Channel', 1015, 0, '资讯', '', '', 'Channel_CMS_Home', NULL, 'Cms', NULL, '_self', 10101501, 0, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (10101502, 10101501, 1, 'Channel', 1015, 0, '资讯首页', ' ', ' ', 'Channel_CMS_Home', NULL, NULL, NULL, '_self', 10101502, 0, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (10101506, 10101501, 1, 'Channel', 1015, 0, '我的资讯', ' ', ' ', 'Channel_CMS_My', NULL, NULL, NULL, '_self', 10101506, 0, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (11101501, 0, 0, 'UserSpace', 1015, 0, '资讯', ' ', ' ', 'Channel_CMS_User', '', 'Cms', NULL, '_blank', 11101501, 0, 0, 1);
INSERT `tn_InitialNavigations` (`NavigationId`, `ParentNavigationId`, `Depth`, `PresentAreaKey`, `ApplicationId`, `NavigationType`, `NavigationText`, `ResourceName`, `NavigationUrl`, `UrlRouteName`, `RouteDataName`, `IconName`, `ImageUrl`, `NavigationTarget`, `DisplayOrder`, `OnlyOwnerVisible`, `IsLocked`, `IsEnabled`) VALUES (20101501, 20000011, 2, 'ControlPanel', 1015, 0, '资讯', '', '', 'ControlPanel_CMS_Home', NULL, NULL, NULL, '_self', 20101501, 0, 0, 1);

-- 数据模型
DELETE FROM `spb_cms_contenttypedefinitions`;
INSERT `spb_cms_ContentTypeDefinitions` (`ContentTypeId`,`ContentTypeName`, `ContentTypeKey`, `IsBuiltIn`, `DisplayOrder`, `TableName`, `ForeignKey`, `Page_New`, `Page_Edit`, `Page_Manage`, `Page_Default_List`, `Page_Default_Detail`, `IsEnabled`, `EnableContribute`, `EnableComment`, `EnableAttachment`) VALUES 
(1,'文章', 'News', 1, 1, 'spb_cms_Addon_News', 'ContentItemId', 'EditNews', 'EditNews', 'ManageNews', 'ListNews', 'ShowNews', 1, 0, 0, 1),
(2,'链接', 'NewsLink', 1, 2, 'spb_cms_Addon_Links', 'ContentItemId', 'EditLink', 'EditLink', 'ManageLinks', 'ListLinks', '''''', 1, 0, 0, 0);

DELETE FROM `spb_cms_contenttypecolumndefinitions`;
INSERT INTO `spb_cms_contenttypecolumndefinitions`(`ColumnId`,`ContentTypeId`,`ColumnName`,`ColumnLabel`,`IsBuiltIn`,`DataType`,`Length`,`Precision`,`IsNotNull`,`DefaultValue`,`IsIndex`,`IsUnique`,`KeyOrIndexName`,`KeyOrIndexColumns`,`ControlCode`,`InitialValue`,`EnableInput`,`EnableEdit`,`ValidateRole`) 
VALUES(1,1, 'ContentItemId', '关联主表', 1, 'long', 8, '19', 0, '', 1, 1, 'PK_spb_cms_Addon_News', 'ContentItemId', '', '', 1, 0, ''),
(2,1, 'TrimBodyAsSummary', '是否截取内容前200字作为摘要', 1, 'bool', 1, '3', 0, '', 0, 0, '', '', '', '', 1, 1, ''),
(3,1, 'Body', '内容', 1, 'string', -1, '0', 0, '', 0, 0, '', '', '', '', 1, 1, ''),
(4,1, 'CopyFrom', '来源名称', 1, 'string', 510, '0', 0, '', 0, 0, '', '', '', '', 1, 1, ''),
(5,1, 'CopyFromUrl', '来源地址', 1, 'string', 1024, '0', 0, '', 0, 0, '', '', '', '', 1, 1, ''),
(6,1, 'EnableComment', '是否允许评论', 1, 'bool', 1, '3', 0, '', 0, 0, '', '', '', '', 1, 1, ''),
(7,1, 'OriginalAuthor', '原创作者用户Id', 1, 'string', 128, '0', 0, '', 0, 0, '', '', '', '', 1, 1, ''),
(8,1, 'Editor', '责任编辑用户Id', 1, 'string', 128, '0', 0, '', 0, 0, '', '', '', '', 1, 1, ''),
(9,1, 'Color', '标题颜色', 1, 'string', 16, '0', 0, '', 0, 0, '', '', '', '', 1, 1, ''),
(10,1, 'IsBold', '标题是否加粗', 1, 'bool', 1, '3', 0, '', 0, 0, '', '', '', '', 1, 1, ''),
(11,1, 'FirstAsTitleImage', '设置第一张图片为标题图', 1, 'bool', 1, '3', 0, '', 0, 0, '', '', '', '', 1, 1, ''),
(12,1, 'AutoPage', '分页类型', 1, 'bool', 1, '3', 0, '', 0, 0, '', '', '', '', 1, 1, ''),
(13,1, 'PageLength', '每页显示的字符', 1, 'int', 4, '10', 0, '', 0, 0, '', '', '', '', 1, 1, ''),
(14,2, 'ContentItemId', '关联主表', 1, 'int', 4, '10', 0, '', 1, 1, 'PK_spb_cms_Addon_Links', 'ContentItemId', '', '', 1, 0, ''),
(15,2, 'Color', '标题颜色', 1, 'string', 16, '0', 0, '', 0, 0, '', '', '', '', 1, 1, ''),
(16,2, 'IsBold', '是否加粗', 1, 'bool', 1, '3', 0, '', 0, 0, '', '', '', '', 1, 1, ''),
(17,2, 'LinkUrl', '链接地址', 1, 'string', 1024, '0', 0, '', 0, 0, '', '', '', '', 1, 1, '');