--2013-12-03 zhengw 清除tn_ItemsInCategories表中的重复数据
DELETE FROM [dbo].[tn_ItemsInCategories]
WHERE ID NOT IN
(
SELECT MAX(ID)
FROM [dbo].[tn_ItemsInCategories]
GROUP BY CategoryId, ItemId)
delete from tn_AccountTypes where AccountTypeKey='QQ'
INSERT [dbo].[tn_AccountTypes] 
	([AccountTypeKey], [ThirdAccountGetterClassType], [AppKey], [AppSecret], [IsSync], [IsShareMicroBlog], [IsFollowMicroBlog], [OfficialMicroBlogAccount], [IsEnabled]) 
VALUES 
	(N'QQ', N'Spacebuilder.Common.QQAccountGetter,Spacebuilder.Common', N'', N'', 1, 1, 0, N'', 0)
--2014-08-21 zhengw 为tn_Messages表的ReceiverUserId字段增加索引
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Messages]') AND name = N'IX_tn_Messages_ReceiverUserId')
CREATE NONCLUSTERED INDEX [IX_tn_Messages_ReceiverUserId] ON [dbo].[tn_Messages]
(
	[ReceiverUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--2014-10-14 xuely 为tn_Follows表增加LastContactDate字段，以实现最近联系人功能
IF NOT EXISTS (SELECT * FROM syscolumns WHERE id=object_id('[dbo].[tn_Follows]') and name='LastContactDate')
ALTER TABLE tn_Follows ADD LastContactDate datetime 
IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID('[dbo].[tn_Follows]') AND name = 'IX_tn_Follows_LastContactDate')
CREATE NONCLUSTERED INDEX IX_tn_Follows_LastContactDate ON [dbo].[tn_Follows] 
(
	[LastContactDate] DESC
) ON [PRIMARY]
GO

--2014-08-21 zhengw 修改数据库版本号
update tn_SystemData set DecimalValue = 4.2 where Datakey = 'SPBVersion'