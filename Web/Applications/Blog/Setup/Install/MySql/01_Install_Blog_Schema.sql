SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `spb_BlogThreads`;
CREATE TABLE `spb_BlogThreads` (
ThreadId BigInt NOT NULL AUTO_INCREMENT,
TenantTypeId Char(6) NOT NULL,
OwnerId BigInt NOT NULL DEFAULT 0,
UserId BigInt NOT NULL,
Author Varchar(64) NOT NULL,
`Subject` Varchar(128) NOT NULL,
Body mediumtext NOT NULL,
Summary Varchar(255) NOT NULL DEFAULT '',
IsDraft TinyInt NOT NULL DEFAULT 0,
IsLocked TinyInt NOT NULL DEFAULT 0,
IsEssential TinyInt NOT NULL DEFAULT 0,
IsSticky TinyInt NOT NULL DEFAULT 0,
AuditStatus SmallInt NOT NULL DEFAULT 40,
PrivacyStatus SmallInt NOT NULL,
IsReproduced TinyInt NOT NULL DEFAULT 0,
OriginalAuthorId BigInt NOT NULL,
IP Varchar(64) NOT NULL DEFAULT '',
DateCreated DateTime NOT NULL,
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
Keywords Varchar(128) NOT NULL DEFAULT '',
FeaturedImageAttachmentId BigInt NOT NULL DEFAULT 0,
FeaturedImage Varchar(255) NOT NULL DEFAULT '',
LastModified DateTime NOT NULL,
KEY `IX_AuditStatus` (`AuditStatus`),
KEY `IX_IsEssential` (`IsEssential`),
KEY `IX_OwnerId_TenantTypeId` (`OwnerId`,`TenantTypeId`),
KEY `IX_PrivacyStatus` (`PrivacyStatus`),
KEY `IX_TenantTypeId` (`TenantTypeId`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`ThreadId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
