SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `spb_GroupMemberApplies`;
CREATE TABLE `spb_GroupMemberApplies` (
Id BigInt NOT NULL AUTO_INCREMENT,
GroupId BigInt NOT NULL,
UserId BigInt NOT NULL,
ApplyReason Varchar(255) NOT NULL DEFAULT '',
ApplyStatus SmallInt NOT NULL,
ApplyDate DateTime NOT NULL,
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `spb_GroupMembers`;
CREATE TABLE `spb_GroupMembers` (
Id BigInt NOT NULL AUTO_INCREMENT,
GroupId BigInt NOT NULL,
UserId BigInt NOT NULL,
IsManager TinyInt NOT NULL DEFAULT 0,
JoinDate DateTime NOT NULL,
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `spb_Groups`;
CREATE TABLE `spb_Groups` (
GroupId BigInt NOT NULL,
GroupName Varchar(255) NOT NULL DEFAULT '',
GroupKey Varchar(16) NOT NULL DEFAULT '',
`Description` Varchar(512) NOT NULL DEFAULT '',
AreaCode Varchar(8) NOT NULL,
UserId BigInt NOT NULL DEFAULT 0,
Logo Varchar(128) NOT NULL DEFAULT '',
IsPublic TinyInt NOT NULL DEFAULT 1,
JoinWay SmallInt NOT NULL,
EnableMemberInvite TinyInt NOT NULL DEFAULT 1,
AuditStatus SmallInt NOT NULL DEFAULT 40,
MemberCount Int(11) NOT NULL DEFAULT 0,
GrowthValue Int(11) NOT NULL DEFAULT 0,
ThemeAppearance Varchar(128) NOT NULL DEFAULT '',
DateCreated DateTime NOT NULL,
IP Varchar(64) NOT NULL DEFAULT '',
Announcement Varchar(512) NOT NULL DEFAULT '',
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
IsUseCustomStyle TinyInt NOT NULL DEFAULT 0,
KEY `IX_AuditStatus` (`AuditStatus`),
KEY `IX_GrowthValue` (`GrowthValue`),
KEY `IX_MemberCount` (`MemberCount`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`GroupId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
