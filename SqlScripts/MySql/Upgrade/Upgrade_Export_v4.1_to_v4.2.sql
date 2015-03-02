-- 2013-12-03 zhengw 清除tn_ItemsInCategories表中的重复数据
DELETE FROM `tn_itemsincategories`
WHERE `ID` NOT IN
(SELECT * FROM (SELECT MAX(`ID`) FROM `tn_itemsincategories` GROUP BY `CategoryId`, `ItemId`) t);

-- 2013-12-03 支持QQ登录
DELETE FROM `tn_AccountTypes` WHERE `AccountTypeKey`='QQ';
INSERT `tn_AccountTypes` 
	(`AccountTypeKey`, `ThirdAccountGetterClassType`, `AppKey`, `AppSecret`, `IsSync`, `IsShareMicroBlog`, `IsFollowMicroBlog`, `OfficialMicroBlogAccount`, `IsEnabled`) 
VALUES 
	('QQ', 'Spacebuilder.Common.QQAccountGetter,Spacebuilder.Common', '', '', 1, 1, 0, '', 0);

-- 2014-08-21 zhengw 为tn_Messages表的ReceiverUserId字段增加索引
DROP PROCEDURE IF EXISTS delete_index;  
create procedure delete_index(IN p_tablename varchar(200), IN p_idxname VARCHAR(200))
begin  
DECLARE str VARCHAR(250);  
  set @str=concat(' drop index ',p_idxname,' on ',p_tablename);   
    
  select count(*) into @cnt from information_schema.statistics where table_name=p_tablename and index_name=p_idxname ;  
  if @cnt >0 then   
    PREPARE stmt FROM @str;  
    EXECUTE stmt ;  
  end if;    
end ;  
call delete_index('tn_Messages','IX_ReceiverUserId');  
ALTER TABLE `tn_Messages` ADD INDEX IX_ReceiverUserId(`ReceiverUserId`);
DROP PROCEDURE IF EXISTS delete_index;  
-- 2014-10-14 xuely 为tn_Follows表增加LastContactDate字段，以实现最近联系人功能
drop procedure if exists addColumn_LastContactDate;  
delimiter ';;';  
create procedure addColumn_LastContactDate() begin  
if exists (select * from information_schema.columns where table_name = 'tn_Follows' and column_name = 'LastContactDate') then  
    DROP INDEX IX_LastContactDate on tn_Follows;
		ALTER TABLE tn_Follows DROP COLUMN LastContactDate; 
end if;  
ALTER TABLE tn_Follows ADD COLUMN LastContactDate DATETIME NULL;
ALTER TABLE tn_Follows ADD INDEX IX_LastContactDate(`LastContactDate`);
end;;  
delimiter ';';  
call addColumn_LastContactDate();  
drop procedure if exists addColumn_LastContactDate;  

-- 2014-08-21 zhengw 修改数据库版本号
update tn_SystemData set DecimalValue = 4.2 where Datakey = 'SPBVersion';