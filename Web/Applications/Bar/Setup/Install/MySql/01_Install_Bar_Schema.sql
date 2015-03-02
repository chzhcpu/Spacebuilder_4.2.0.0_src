SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `spb_BarPosts`;
CREATE TABLE `spb_BarPosts` (
PostId BigInt NOT NULL AUTO_INCREMENT,
SectionId BigInt NOT NULL DEFAULT 0,
TenantTypeId Char(6) NOT NULL,
OwnerId BigInt NOT NULL DEFAULT 0,
ThreadId BigInt NOT NULL,
ParentId BigInt NOT NULL DEFAULT 0,
UserId BigInt NOT NULL,
Author Varchar(64) NOT NULL,
`Subject` Varchar(128) NOT NULL,
Body mediumtext NOT NULL,
AuditStatus SmallInt NOT NULL DEFAULT 40,
IP Varchar(64) NOT NULL DEFAULT '',
ChildPostCount Int(11) NOT NULL DEFAULT 0,
DateCreated DateTime NOT NULL,
LastModified DateTime NOT NULL,
KEY `IX_AuditStatus` (`AuditStatus`),
KEY `IX_OwnerId` (`OwnerId`),
KEY `IX_ParentId` (`ParentId`),
KEY `IX_SectionId` (`SectionId`),
KEY `IX_TenantTypeId` (`TenantTypeId`),
KEY `IX_ThreadId` (`ThreadId`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`PostId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `spb_BarRatings`;
CREATE TABLE `spb_BarRatings` (
RatingId BigInt NOT NULL AUTO_INCREMENT,
ThreadId BigInt NOT NULL,
UserId BigInt NOT NULL,
UserDisplayName Varchar(64) NOT NULL,
TradePoints Int(11) NOT NULL DEFAULT 0,
ReputationPoints Int(11) NOT NULL DEFAULT 0,
Reason Varchar(255) NOT NULL,
IP Varchar(64) NOT NULL DEFAULT '',
DateCreated DateTime NOT NULL,
KEY `IX_ThreadId` (`ThreadId`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`RatingId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `spb_BarSectionManagers`;
CREATE TABLE `spb_BarSectionManagers` (
Id BigInt NOT NULL AUTO_INCREMENT,
SectionId BigInt NOT NULL DEFAULT 0,
UserId BigInt NOT NULL,
KEY `IX_SectionId` (`SectionId`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `spb_BarSections`;
CREATE TABLE `spb_BarSections` (
SectionId BigInt NOT NULL,
TenantTypeId Char(6) NOT NULL,
OwnerId BigInt NOT NULL DEFAULT 0,
UserId BigInt NOT NULL DEFAULT 0,
Name Varchar(64) NOT NULL,
`Description` mediumtext NOT NULL,
LogoImage Varchar(255) NOT NULL DEFAULT '',
IsEnabled TinyInt NOT NULL DEFAULT 1,
EnableRss TinyInt NOT NULL DEFAULT 1,
ThreadCategoryStatus SmallInt NOT NULL DEFAULT 1,
AuditStatus SmallInt NOT NULL DEFAULT 40,
DisplayOrder Int(11) NOT NULL,
DateCreated DateTime NOT NULL,
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
KEY `IX_AuditStatus` (`AuditStatus`),
KEY `IX_DisplayOrder` (`DisplayOrder`),
KEY `IX_OwnerId` (`OwnerId`),
KEY `IX_TenantTypeId` (`TenantTypeId`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`SectionId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `spb_BarThreads`;
CREATE TABLE `spb_BarThreads` (
ThreadId BigInt NOT NULL AUTO_INCREMENT,
SectionId BigInt NOT NULL DEFAULT 0,
TenantTypeId Char(6) NOT NULL,
OwnerId BigInt NOT NULL DEFAULT 0,
UserId BigInt NOT NULL,
Author Varchar(64) NOT NULL,
`Subject` Varchar(128) NOT NULL,
Body mediumtext NOT NULL,
IsLocked TinyInt NOT NULL DEFAULT 0,
IsEssential TinyInt NOT NULL DEFAULT 0,
IsSticky TinyInt NOT NULL DEFAULT 0,
StickyDate DateTime NOT NULL,
IsHidden TinyInt NOT NULL DEFAULT 0,
HighlightStyle Varchar(512) NOT NULL,
HighlightDate DateTime NOT NULL,
Price Int(11) NOT NULL,
AuditStatus SmallInt NOT NULL DEFAULT 40,
PostCount Int(11) NOT NULL DEFAULT 0,
IP Varchar(64) NOT NULL DEFAULT '',
DateCreated DateTime NOT NULL,
LastModified DateTime NOT NULL,
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
KEY `IX_AuditStatus` (`AuditStatus`),
KEY `IX_OwnerId` (`OwnerId`),
KEY `IX_SectionId` (`SectionId`),
KEY `IX_TenantTypeId` (`TenantTypeId`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`ThreadId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
