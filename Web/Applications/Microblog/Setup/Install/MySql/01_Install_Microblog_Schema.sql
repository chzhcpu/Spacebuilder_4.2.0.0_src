SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `spb_Microblogs`;
CREATE TABLE `spb_Microblogs` (
MicroblogId BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL DEFAULT 0,
Author Varchar(64) NOT NULL DEFAULT '',
TenantTypeId Char(6) NOT NULL DEFAULT 101,
OwnerId BigInt NOT NULL DEFAULT 0,
OriginalMicroblogId BigInt NOT NULL DEFAULT 0,
ForwardedMicroblogId BigInt NOT NULL DEFAULT 0,
Body Varchar(1500) NOT NULL,
ReplyCount Int(11) NOT NULL DEFAULT 0,
ForwardedCount Int(11) NOT NULL DEFAULT 0,
HasPhoto TinyInt NOT NULL DEFAULT 0,
HasVideo TinyInt NOT NULL DEFAULT 0,
HasMusic TinyInt NOT NULL DEFAULT 0,
PostWay SmallInt NOT NULL DEFAULT 0,
Source Varchar(64) DEFAULT NULL,
SourceUrl Varchar(128) DEFAULT NULL,
IP Varchar(64) NOT NULL DEFAULT '',
AuditStatus SmallInt NOT NULL DEFAULT 40,
DateCreated DateTime NOT NULL,
KEY `IX_AuditStatus` (`AuditStatus`),
KEY `IX_ForwardedCount` (`ForwardedCount`),
KEY `IX_OriginalMicroblogId` (`OriginalMicroblogId`),
KEY `IX_OwnerId` (`OwnerId`),
KEY `IX_ReplyCount` (`ReplyCount`),
KEY `IX_TenantTypeId` (`TenantTypeId`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`MicroblogId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
