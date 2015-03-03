-- todo: 需要修改ApplicationId
/* 应用数据 */
DELETE FROM [dbo].[tn_Applications] WHERE [ApplicationId] = 9001

/* 应用在呈现区域的设置 */
DELETE FROM [dbo].[tn_ApplicationInPresentAreaSettings] WHERE [ApplicationId] = 9001

/* 应用在呈现区域的导航初始化数据 */
DELETE FROM [dbo].[tn_InitialNavigations] WHERE [ApplicationId] = 9001

/* 应用在呈现区域的管理操作 */
DELETE FROM [dbo].[tn_ApplicationManagementOperations] WHERE [ApplicationId] = 9001