SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `spb_Announcements`;
CREATE TABLE `spb_Announcements` (
Id BigInt NOT NULL AUTO_INCREMENT,
Subject Varchar(255) NOT NULL DEFAULT '',
SubjectStyle Varchar(512) NOT NULL,
Body mediumtext NOT NULL,
IsHyperLink TinyInt NOT NULL DEFAULT 0,
HyperLinkUrl Varchar(512) NOT NULL DEFAULT '',
EnabledDescription TinyInt NOT NULL DEFAULT 0,
ReleaseDate DateTime NOT NULL,
ExpiredDate DateTime NOT NULL,
LastModified DateTime NOT NULL,
CreatDate DateTime NOT NULL,
UserId BigInt NOT NULL DEFAULT 0,
DisplayOrder BigInt NOT NULL DEFAULT 100,
DisplayArea Varchar(64) NOT NULL DEFAULT '',
KEY `IX_DisplayArea` (`DisplayArea`),
KEY `IX_DisplayOrder` (`DisplayOrder`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `spb_CustomStyles`;
CREATE TABLE `spb_CustomStyles` (
Id BigInt NOT NULL AUTO_INCREMENT,
PresentAreaKey Varchar(32) NOT NULL,
OwnerId BigInt NOT NULL,
SerializedCustomStyle mediumtext NOT NULL,
BackgroundImage Varchar(128) NOT NULL,
LastModified DateTime NOT NULL,
KEY `IX_OwnerId` (`OwnerId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `spb_EducationExperiences`;
CREATE TABLE `spb_EducationExperiences` (
Id BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL,
Degree SmallInt NOT NULL,
School Varchar(128) NOT NULL,
StartYear Int(11) NOT NULL,
Department Varchar(128) NOT NULL,
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `spb_Identifications`;
CREATE TABLE `spb_Identifications` (
IdentificationId BigInt NOT NULL AUTO_INCREMENT,
IdentificationTypeId BigInt NOT NULL,
UserId BigInt NOT NULL,
TrueName Varchar(64) NOT NULL,
IdNumber Varchar(32) NOT NULL,
Status TinyInt NOT NULL,
Email Varchar(64) NOT NULL,
Mobile Varchar(64) NOT NULL,
Description Varchar(255) NOT NULL,
DateCreated DateTime NOT NULL,
DisposerId BigInt NOT NULL,
LastModified DateTime NOT NULL,
IdentificationLogo Varchar(255) NOT NULL,
KEY `IX_IdentificationId` (`IdentificationId`),
PRIMARY KEY (`IdentificationId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `spb_IdentificationTypes`;
CREATE TABLE `spb_IdentificationTypes` (
IdentificationTypeId BigInt NOT NULL AUTO_INCREMENT,
Name Varchar(64) NOT NULL,
Description Varchar(255) NOT NULL,
Enabled TinyInt NOT NULL,
CreaterId BigInt NOT NULL,
DateCreated DateTime NOT NULL,
IdentificationTypeLogo Varchar(255) NOT NULL,
PRIMARY KEY (`IdentificationTypeId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `spb_ImpeachReports`;
CREATE TABLE `spb_ImpeachReports` (
ReportId BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL,
Reporter Varchar(64) NOT NULL,
ReportedUserId BigInt NOT NULL,
Email Varchar(64) NOT NULL,
Title Varchar(255) NOT NULL,
Telephone Varchar(64) NOT NULL,
Reason SmallInt NOT NULL,
Description Varchar(255) NOT NULL,
URL Varchar(255) NOT NULL,
DateCreated DateTime NOT NULL,
LastModified DateTime NOT NULL,
Status TinyInt NOT NULL,
DisposerId BigInt NOT NULL,
KEY `IX_Reason` (`Reason`),
KEY `IX_Status` (`Status`),
PRIMARY KEY (`ReportId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `spb_Links`;
CREATE TABLE `spb_Links` (
LinkId BigInt NOT NULL AUTO_INCREMENT,
OwnerType SmallInt NOT NULL,
OwnerId BigInt NOT NULL,
LinkName Varchar(128) NOT NULL,
LinkType TinyInt NOT NULL,
LinkUrl Varchar(512) NOT NULL,
ImageUrl Varchar(512) NOT NULL,
Description Varchar(512) NOT NULL,
IsEnabled TinyInt NOT NULL,
DisplayOrder BigInt NOT NULL,
DateCreated DateTime NOT NULL,
LastModified DateTime NOT NULL,
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
PRIMARY KEY (`LinkId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `spb_Profiles`;
CREATE TABLE `spb_Profiles` (
UserId BigInt NOT NULL,
Gender SmallInt NOT NULL DEFAULT 0,
BirthdayType SmallInt NOT NULL DEFAULT 1,
Birthday DateTime NOT NULL,
LunarBirthday DateTime NOT NULL,
NowAreaCode Varchar(8) NOT NULL,
HomeAreaCode Varchar(8) NOT NULL,
Email Varchar(64) NOT NULL,
Mobile Varchar(64) NOT NULL,
QQ Varchar(64) NOT NULL,
Msn Varchar(64) NOT NULL,
Skype Varchar(64) NOT NULL,
Fetion Varchar(64) NOT NULL,
Aliwangwang Varchar(64) NOT NULL,
CardType SmallInt NOT NULL,
CardID Varchar(64) NOT NULL,
Introduction Varchar(255) NOT NULL,
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
Integrity Int(11) NOT NULL,
PRIMARY KEY (`UserId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `spb_WorkExperiences`;
CREATE TABLE `spb_WorkExperiences` (
Id BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL,
CompanyName Varchar(64) NOT NULL,
CompanyAreaCode Varchar(8) NOT NULL,
StartDate DateTime NOT NULL,
EndDate DateTime NOT NULL,
JobDescription Varchar(128) NOT NULL,
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_AccountBindings`;
CREATE TABLE `tn_AccountBindings` (
Id BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL,
AccountTypeKey Varchar(64) NOT NULL,
Identification Varchar(255) NOT NULL DEFAULT '',
AccessToken Varchar(255) NOT NULL DEFAULT '',
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
KEY `IX_AccountTypeKey` (`AccountTypeKey`),
KEY `IX_Identification` (`Identification`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_AccountTypes`;
CREATE TABLE `tn_AccountTypes` (
AccountTypeKey Varchar(64) NOT NULL,
ThirdAccountGetterClassType Varchar(255) NOT NULL DEFAULT '',
AppKey Varchar(255) NOT NULL DEFAULT '',
AppSecret Varchar(255) NOT NULL DEFAULT '',
IsSync TinyInt NOT NULL DEFAULT 0,
IsShareMicroBlog TinyInt NOT NULL DEFAULT 0,
IsFollowMicroBlog TinyInt NOT NULL DEFAULT 0,
OfficialMicroBlogAccount Varchar(255) NOT NULL DEFAULT '',
IsEnabled TinyInt NOT NULL DEFAULT 0,
PRIMARY KEY (`AccountTypeKey`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Activities`;
CREATE TABLE `tn_Activities` (
ActivityId BigInt NOT NULL AUTO_INCREMENT,
OwnerId BigInt NOT NULL,
OwnerType SmallInt NOT NULL,
OwnerName Varchar(64) NOT NULL,
ActivityItemKey Varchar(32) NOT NULL,
ApplicationId Int(11) NOT NULL,
TenantTypeId Char(6) NOT NULL,
UserId BigInt NOT NULL,
SourceId BigInt NOT NULL,
ReferenceId BigInt NOT NULL,
ReferenceTenantTypeId Char(6) NOT NULL,
IsPrivate TinyInt NOT NULL,
IsOriginalThread TinyInt NOT NULL,
HasVideo TinyInt NOT NULL,
HasMusic TinyInt NOT NULL,
HasImage TinyInt NOT NULL,
DateCreated DateTime NOT NULL,
LastModified DateTime NOT NULL,
KEY `IX_ActivityItemKey` (`ActivityItemKey`),
KEY `IX_ApplicationId` (`ApplicationId`),
KEY `IX_LastModified` (`LastModified`),
KEY `IX_OwnerId_OwnerType` (`OwnerId`,`OwnerType`),
KEY `IX_OwnerType` (`OwnerType`),
KEY `IX_ReferenceId` (`ReferenceId`),
KEY `IX_SourceId` (`SourceId`),
KEY `IX_TenantTypeId` (`TenantTypeId`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`ActivityId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_ActivityItems`;
CREATE TABLE `tn_ActivityItems` (
ItemKey Varchar(32) NOT NULL,
ApplicationId Int(11) NOT NULL,
ItemName Varchar(32) NOT NULL DEFAULT '',
DisplayOrder Int(11) NOT NULL DEFAULT 0,
Description Varchar(128) NOT NULL DEFAULT '',
IsOnlyOnce TinyInt NOT NULL,
IsUserReceived TinyInt NOT NULL DEFAULT 1,
IsSiteReceived TinyInt NOT NULL DEFAULT 1,
KEY `IX_ApplicationId` (`ApplicationId`),
KEY `IX_DisplayOrder` (`DisplayOrder`),
PRIMARY KEY (`ItemKey`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_ActivityItemUserSettings`;
CREATE TABLE `tn_ActivityItemUserSettings` (
Id BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL,
ItemKey Varchar(32) NOT NULL,
IsReceived TinyInt NOT NULL,
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_ActivitySiteInbox`;
CREATE TABLE `tn_ActivitySiteInbox` (
Id BigInt NOT NULL AUTO_INCREMENT,
ActivityId BigInt NOT NULL,
KEY `IX_ActivityId` (`ActivityId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_ActivityUserInbox`;
CREATE TABLE `tn_ActivityUserInbox` (
Id BigInt NOT NULL AUTO_INCREMENT,
ActivityId BigInt NOT NULL,
UserId BigInt NOT NULL,
KEY `IX_ActivityId` (`ActivityId`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_AdvertisingPosition`;
CREATE TABLE `tn_AdvertisingPosition` (
PositionId Varchar(25) NOT NULL,
PresentAreaKey Varchar(32) NOT NULL,
Description Varchar(255) NOT NULL DEFAULT '',
FeaturedImage Varchar(512) NOT NULL,
Width Int(11) NOT NULL,
Height Int(11) NOT NULL,
IsEnable TinyInt NOT NULL DEFAULT 1,
PRIMARY KEY (`PositionId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Advertisings`;
CREATE TABLE `tn_Advertisings` (
AdvertisingId BigInt NOT NULL AUTO_INCREMENT,
Name Varchar(512) NOT NULL,
AdvertisingType SmallInt NOT NULL,
Body mediumtext NOT NULL,
AttachmentUrl Varchar(512) NOT NULL,
Url Varchar(512) NOT NULL,
IsEnable TinyInt NOT NULL DEFAULT 1,
IsBlank TinyInt NOT NULL DEFAULT 1,
StartDate DateTime NOT NULL,
EndDate DateTime NOT NULL,
UseredPositionCount Int(11) NOT NULL DEFAULT 0,
DisplayOrder BigInt NOT NULL,
DateCreated DateTime NOT NULL,
LastModified DateTime NOT NULL,
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
TextStyle Varchar(512) NOT NULL,
PRIMARY KEY (`AdvertisingId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_AdvertisingsInPosition`;
CREATE TABLE `tn_AdvertisingsInPosition` (
Id BigInt NOT NULL AUTO_INCREMENT,
AdvertisingId BigInt NOT NULL,
PositionId Varchar(25) NOT NULL,
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_ApplicationData`;
CREATE TABLE `tn_ApplicationData` (
Id Int(11) NOT NULL AUTO_INCREMENT,
ApplicationId Int(11) NOT NULL,
TenantTypeId Char(6) NOT NULL,
Datakey Varchar(32) NOT NULL,
LongValue BigInt NOT NULL DEFAULT 0,
DecimalValue Decimal(10,2) NOT NULL DEFAULT 0,
StringValue Varchar(255) NOT NULL DEFAULT '',
KEY `IX_ApplicationId_TenantTypeId` (`ApplicationId`,`TenantTypeId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_ApplicationInPresentAreaInstallations`;
CREATE TABLE `tn_ApplicationInPresentAreaInstallations` (
Id Int(11) NOT NULL AUTO_INCREMENT,
OwnerId BigInt NOT NULL,
ApplicationId Int(11) NOT NULL,
PresentAreaKey Varchar(32) NOT NULL,
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_ApplicationInPresentAreaSettings`;
CREATE TABLE `tn_ApplicationInPresentAreaSettings` (
Id Int(11) NOT NULL AUTO_INCREMENT,
ApplicationId Int(11) NOT NULL,
PresentAreaKey Varchar(32) NOT NULL,
IsBuiltIn TinyInt NOT NULL DEFAULT 0,
IsAutoInstall TinyInt NOT NULL DEFAULT 0,
IsGenerateData TinyInt NOT NULL DEFAULT 1,
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_ApplicationManagementOperations`;
CREATE TABLE `tn_ApplicationManagementOperations` (
OperationId Int(11) NOT NULL,
ApplicationId Int(11) NOT NULL,
AssociatedNavigationId Int(11) NOT NULL DEFAULT 0,
PresentAreaKey Varchar(32) NOT NULL,
OperationType Int(11) NOT NULL,
OperationText Varchar(64) NOT NULL DEFAULT '',
ResourceName Varchar(64) NOT NULL DEFAULT '',
NavigationUrl Varchar(255) NOT NULL DEFAULT '',
UrlRouteName Varchar(64) NOT NULL,
RouteDataName Varchar(255) DEFAULT NULL,
IconName Varchar(32) DEFAULT NULL,
ImageUrl Varchar(255) DEFAULT NULL,
NavigationTarget Varchar(32) DEFAULT NULL,
DisplayOrder Int(11) NOT NULL DEFAULT 100,
OnlyOwnerVisible TinyInt NOT NULL DEFAULT 1,
IsLocked TinyInt NOT NULL DEFAULT 0,
IsEnabled TinyInt NOT NULL DEFAULT 1,
PRIMARY KEY (`OperationId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Applications`;
CREATE TABLE `tn_Applications` (
ApplicationId Int(11) NOT NULL,
ApplicationKey Varchar(64) NOT NULL,
Description Varchar(255) NOT NULL DEFAULT '',
IsEnabled TinyInt NOT NULL DEFAULT 1,
IsLocked TinyInt NOT NULL DEFAULT 0,
DisplayOrder Int(11) NOT NULL DEFAULT 1000,
PRIMARY KEY (`ApplicationId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Areas`;
CREATE TABLE `tn_Areas` (
AreaCode Varchar(8) NOT NULL,
ParentCode Varchar(8) NOT NULL DEFAULT '',
Name Varchar(64) NOT NULL DEFAULT '',
PostCode Varchar(8) NOT NULL DEFAULT '',
DisplayOrder Int(11) NOT NULL DEFAULT 0,
Depth Int(11) NOT NULL DEFAULT 0,
ChildCount Int(11) NOT NULL DEFAULT 0,
KEY `IX_DisplayOrder` (`DisplayOrder`),
PRIMARY KEY (`AreaCode`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_AttachmentDownloadRecords`;
CREATE TABLE `tn_AttachmentDownloadRecords` (
Id BigInt NOT NULL AUTO_INCREMENT,
AttachmentId BigInt NOT NULL,
AssociateId BigInt NOT NULL,
OwnerId BigInt NOT NULL,
TenantTypeId Char(6) NOT NULL,
UserId BigInt NOT NULL,
UserDisplayName Varchar(64) NOT NULL DEFAULT '',
Price Int(11) NOT NULL DEFAULT 0,
LastDownloadDate DateTime NOT NULL,
DownloadDate DateTime NOT NULL,
FromUrl Varchar(512) DEFAULT NULL,
IP Varchar(64) NOT NULL DEFAULT '',
KEY `IX_AssociateId` (`AssociateId`),
KEY `IX_AttachmentId` (`AttachmentId`),
KEY `IX_LastDownloadDate` (`LastDownloadDate`),
KEY `IX_OwnerId_TenantTypeId` (`OwnerId`,`TenantTypeId`),
KEY `IX_TenantTypeId` (`TenantTypeId`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Attachments`;
CREATE TABLE `tn_Attachments` (
AttachmentId BigInt NOT NULL AUTO_INCREMENT,
AssociateId BigInt NOT NULL,
OwnerId BigInt NOT NULL,
TenantTypeId Char(6) NOT NULL,
UserId BigInt NOT NULL,
UserDisplayName Varchar(64) NOT NULL,
FileName Varchar(255) NOT NULL DEFAULT '',
FriendlyFileName Varchar(255) NOT NULL DEFAULT '',
MediaType Int(11) NOT NULL DEFAULT 99,
ContentType Varchar(128) NOT NULL DEFAULT '',
FileLength BigInt NOT NULL DEFAULT 0,
Height Int(11) NOT NULL DEFAULT 0,
Width Int(11) NOT NULL DEFAULT 0,
Price Int(11) NOT NULL DEFAULT 0,
Password Varchar(32) NOT NULL DEFAULT '',
IP Varchar(64) NOT NULL DEFAULT '',
DateCreated DateTime NOT NULL,
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
KEY `IX_AssociateId` (`AssociateId`),
KEY `IX_OwnerId_TenantTypeId` (`OwnerId`,`TenantTypeId`),
KEY `IX_TenantTypeId` (`TenantTypeId`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`AttachmentId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_AttitudeRecords`;
CREATE TABLE `tn_AttitudeRecords` (
Id BigInt NOT NULL AUTO_INCREMENT,
ObjectId BigInt NOT NULL,
UserId BigInt NOT NULL,
TenantTypeId Char(6) NOT NULL,
IsSupport TinyInt NOT NULL,
KEY `IX_ObjectId` (`ObjectId`),
KEY `IX_TenantTypeId` (`TenantTypeId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Attitudes`;
CREATE TABLE `tn_Attitudes` (
Id BigInt NOT NULL AUTO_INCREMENT,
ObjectId BigInt NOT NULL DEFAULT 0,
SupportCount Int(11) NOT NULL,
OpposeCount Int(11) NOT NULL,
TenantTypeId Char(6) NOT NULL,
Comprehensive Float(7,2) NOT NULL,
KEY `IX_ObjectId` (`ObjectId`),
KEY `IX_TenantTypeId` (`TenantTypeId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_AtUsers`;
CREATE TABLE `tn_AtUsers` (
Id BigInt NOT NULL AUTO_INCREMENT,
TenantTypeId Char(6) NOT NULL,
AssociateId BigInt NOT NULL,
UserId BigInt NOT NULL DEFAULT 0,
KEY `IX_AssociateId` (`AssociateId`),
KEY `IX_TenantTypeId` (`TenantTypeId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_AuditItems`;
CREATE TABLE `tn_AuditItems` (
ItemKey Varchar(32) NOT NULL,
ApplicationId Int(11) NOT NULL,
ItemName Varchar(64) NOT NULL,
DisplayOrder Int(11) NOT NULL,
Description Varchar(128) NOT NULL,
KEY `IX_ApplicationId` (`ApplicationId`),
KEY `IX_DisplayOrder` (`DisplayOrder`),
PRIMARY KEY (`ItemKey`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_AuditItemsInUserRoles`;
CREATE TABLE `tn_AuditItemsInUserRoles` (
Id Int(11) NOT NULL AUTO_INCREMENT,
RoleName Varchar(32) NOT NULL,
ItemKey Varchar(32) NOT NULL,
StrictDegree SmallInt NOT NULL,
IsLocked TinyInt NOT NULL,
KEY `IX_RoleName` (`RoleName`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Categories`;
CREATE TABLE `tn_Categories` (
CategoryId BigInt NOT NULL AUTO_INCREMENT,
ParentId BigInt NOT NULL DEFAULT 0,
OwnerId BigInt NOT NULL,
TenantTypeId Char(6) NOT NULL,
CategoryName Varchar(128) NOT NULL,
Description Varchar(255) NOT NULL DEFAULT '',
DisplayOrder Int(11) NOT NULL DEFAULT 0,
Depth Int(11) NOT NULL DEFAULT 0,
ChildCount Int(11) NOT NULL DEFAULT 0,
ItemCount Int(11) NOT NULL DEFAULT 0,
PrivacyStatus TinyInt NOT NULL DEFAULT 30,
AuditStatus SmallInt NOT NULL DEFAULT 40,
FeaturedItemId BigInt NOT NULL DEFAULT 0,
LastModified DateTime NOT NULL,
DateCreated DateTime NOT NULL,
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
KEY `IX_AuditStatus` (`AuditStatus`),
KEY `IX_CategoryName` (`CategoryName`),
KEY `IX_DisplayOrder` (`DisplayOrder`),
KEY `IX_OwnerId_TenantTypeId` (`OwnerId`,`TenantTypeId`),
KEY `IX_ParentId` (`ParentId`),
PRIMARY KEY (`CategoryId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Comments`;
CREATE TABLE `tn_Comments` (
Id BigInt NOT NULL AUTO_INCREMENT,
ParentId BigInt NOT NULL,
CommentedObjectId BigInt NOT NULL,
TenantTypeId Char(6) NOT NULL,
OwnerId BigInt NOT NULL,
UserId BigInt NOT NULL,
Author Varchar(64) NOT NULL,
ToUserId BigInt NOT NULL,
ToUserDisplayName Varchar(64) NOT NULL,
Subject Varchar(255) NOT NULL,
Body mediumtext NOT NULL,
IsPrivate TinyInt NOT NULL,
AuditStatus SmallInt NOT NULL,
ChildCount Int(11) NOT NULL,
IsAnonymous TinyInt NOT NULL DEFAULT 0,
IP Varchar(64) NOT NULL,
DateCreated DateTime NOT NULL,
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
KEY `IX_AuditStatus` (`AuditStatus`),
KEY `IX_CommentedObjectId` (`CommentedObjectId`),
KEY `IX_OwnerId_TenantTypeId` (`OwnerId`,`TenantTypeId`),
KEY `IX_ParentId` (`ParentId`),
KEY `IX_TenantTypeId` (`TenantTypeId`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_CommonOperations`;
CREATE TABLE `tn_CommonOperations` (
Id BigInt NOT NULL AUTO_INCREMENT,
NavigationId Int(11) NOT NULL DEFAULT 0,
UserId BigInt NOT NULL DEFAULT 0,
KEY `IX_NavigationId` (`NavigationId`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_ContentPrivacySpecifyObjects`;
CREATE TABLE `tn_ContentPrivacySpecifyObjects` (
Id BigInt NOT NULL AUTO_INCREMENT,
TenantTypeId Char(6) NOT NULL,
ContentId BigInt NOT NULL,
SpecifyObjectTypeId Int(11) NOT NULL,
SpecifyObjectId BigInt NOT NULL,
SpecifyObjectName Varchar(64) NOT NULL,
DateCreated DateTime NOT NULL,
KEY `IX_ContentId` (`ContentId`),
KEY `IX_SpecifyObjectTypeId` (`SpecifyObjectTypeId`),
KEY `IX_TenantTypeId` (`TenantTypeId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_EmailQueue`;
CREATE TABLE `tn_EmailQueue` (
Id Int(11) NOT NULL AUTO_INCREMENT,
Priority Int(11) NOT NULL DEFAULT 0,
IsBodyHtml TinyInt NOT NULL DEFAULT 1,
MailTo mediumtext NOT NULL,
MailCc mediumtext DEFAULT NULL,
MailBcc mediumtext DEFAULT NULL,
MailFrom Varchar(512) NOT NULL,
Subject Varchar(512) NOT NULL DEFAULT '',
Body mediumtext NOT NULL,
NextTryTime DateTime NOT NULL,
NumberOfTries Int(11) NOT NULL DEFAULT 0,
IsFailed TinyInt NOT NULL DEFAULT 0,
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_EmotionCategories`;
CREATE TABLE `tn_EmotionCategories` (
DirectoryName Varchar(32) NOT NULL,
DisplayOrder Int(11) NOT NULL DEFAULT 100,
IsEnabled TinyInt NOT NULL DEFAULT 1,
PRIMARY KEY (`DirectoryName`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Favorites`;
CREATE TABLE `tn_Favorites` (
Id BigInt NOT NULL AUTO_INCREMENT,
TenantTypeId Char(6) NOT NULL DEFAULT '',
UserId BigInt NOT NULL DEFAULT 0,
ObjectId BigInt NOT NULL DEFAULT 0,
KEY `IX_TenantTypeId` (`TenantTypeId`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Follows`;
CREATE TABLE `tn_Follows` (
Id BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL DEFAULT 0,
FollowedUserId BigInt NOT NULL DEFAULT 0,
NoteName Varchar(64) NOT NULL DEFAULT '',
IsQuietly TinyInt NOT NULL DEFAULT 0,
IsNewFollower TinyInt NOT NULL DEFAULT 1,
DateCreated DateTime NOT NULL,
LastContactDate DateTime  NULL,
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
IsMutual TinyInt NOT NULL DEFAULT 0,
KEY `IX_UserId_FollowedUserId` (`UserId`,`FollowedUserId`),
KEY `IX_FollowedUserId` (`FollowedUserId`),
KEY `IX_LastContactDate` (`LastContactDate`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_InitialNavigations`;
CREATE TABLE `tn_InitialNavigations` (
NavigationId Int(11) NOT NULL,
ParentNavigationId Int(11) NOT NULL DEFAULT 0,
Depth Int(11) NOT NULL DEFAULT 0,
PresentAreaKey Varchar(32) NOT NULL,
ApplicationId Int(11) NOT NULL DEFAULT 0,
NavigationType Int(11) NOT NULL,
NavigationText Varchar(64) NOT NULL,
ResourceName Varchar(64) NOT NULL,
NavigationUrl Varchar(255) NOT NULL,
UrlRouteName Varchar(64) NOT NULL,
RouteDataName Varchar(255) DEFAULT NULL,
IconName Varchar(32) DEFAULT NULL,
ImageUrl Varchar(255) DEFAULT NULL,
NavigationTarget Varchar(32) DEFAULT NULL,
DisplayOrder Int(11) NOT NULL DEFAULT 100,
OnlyOwnerVisible TinyInt NOT NULL DEFAULT 0,
IsLocked TinyInt NOT NULL DEFAULT 0,
IsEnabled TinyInt NOT NULL DEFAULT 1,
PRIMARY KEY (`NavigationId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_InvitationCodes`;
CREATE TABLE `tn_InvitationCodes` (
Code Varchar(32) NOT NULL,
UserId BigInt NOT NULL,
IsMultiple TinyInt NOT NULL,
ExpiredDate DateTime NOT NULL,
DateCreated DateTime NOT NULL,
PRIMARY KEY (`Code`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_InvitationCodeStatistics`;
CREATE TABLE `tn_InvitationCodeStatistics` (
UserId BigInt NOT NULL,
CodeUnUsedCount Int(11) NOT NULL,
CodeUsedCount Int(11) NOT NULL,
CodeBuyedCount Int(11) NOT NULL,
PRIMARY KEY (`UserId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Invitations`;
CREATE TABLE `tn_Invitations` (
Id BigInt NOT NULL AUTO_INCREMENT,
ApplicationId Int(11) NOT NULL DEFAULT 0,
InvitationTypeKey Varchar(64) NOT NULL,
UserId BigInt NOT NULL,
SenderUserId BigInt NOT NULL,
Sender Varchar(64) NOT NULL,
RelativeObjectName Varchar(128) NOT NULL DEFAULT '',
RelativeObjectId BigInt NOT NULL,
RelativeObjectUrl Varchar(255) NOT NULL,
Status TinyInt NOT NULL DEFAULT 0,
DateCreated DateTime NOT NULL,
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
KEY `IX_ApplicationId` (`ApplicationId`),
KEY `IX_InvitationTypeKey` (`InvitationTypeKey`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_InviteFriendRecords`;
CREATE TABLE `tn_InviteFriendRecords` (
Id BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL DEFAULT 0,
InvitedUserId BigInt NOT NULL DEFAULT 0,
Code Varchar(512) NOT NULL,
DateCreated DateTime NOT NULL,
InvitingUserHasBeingRewarded TinyInt NOT NULL,
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_ItemsInCategories`;
CREATE TABLE `tn_ItemsInCategories` (
Id BigInt NOT NULL AUTO_INCREMENT,
CategoryId BigInt NOT NULL,
ItemId BigInt NOT NULL,
KEY `IX_CategoryId_ItemId` (`CategoryId`,`ItemId`),
KEY `IX_ItemId` (`ItemId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_ItemsInTags`;
CREATE TABLE `tn_ItemsInTags` (
Id BigInt NOT NULL AUTO_INCREMENT,
TagName Varchar(128) DEFAULT NULL,
TagInOwnerId BigInt NOT NULL,
ItemId BigInt NOT NULL,
TenantTypeId Char(6) NOT NULL,
KEY `IX_ItemId` (`ItemId`),
KEY `IX_TagName` (`TagName`),
KEY `IX_TagInOwnerId` (`TagInOwnerId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Messages`;
CREATE TABLE `tn_Messages` (
MessageId BigInt NOT NULL AUTO_INCREMENT,
SenderUserId BigInt NOT NULL DEFAULT 0,
Sender Varchar(64) NOT NULL DEFAULT '',
ReceiverUserId BigInt NOT NULL DEFAULT 0,
Receiver Varchar(64) NOT NULL DEFAULT '',
Subject Varchar(255) DEFAULT NULL,
Body Varchar(4000) NOT NULL DEFAULT '',
IsRead TinyInt NOT NULL DEFAULT 0,
IP Varchar(64) NOT NULL DEFAULT N'000.000.000.000',
DateCreated DateTime NOT NULL,
KEY `IX_ReceiverUserId` (`ReceiverUserId`),
PRIMARY KEY (`MessageId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_MessageSessions`;
CREATE TABLE `tn_MessageSessions` (
SessionId BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL DEFAULT 0,
OtherUserId BigInt NOT NULL DEFAULT 0,
LastMessageId BigInt NOT NULL DEFAULT 0,
MessageCount Int(11) NOT NULL DEFAULT 0,
UnreadMessageCount Int(11) NOT NULL DEFAULT 0,
MessageType Int(11) NOT NULL DEFAULT 0,
LastModified DateTime NOT NULL,
PRIMARY KEY (`SessionId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_MessagesInSessions`;
CREATE TABLE `tn_MessagesInSessions` (
Id BigInt NOT NULL AUTO_INCREMENT,
SessionId BigInt NOT NULL,
MessageId BigInt NOT NULL DEFAULT 0,
KEY `IX_SessionId` (`SessionId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Notices`;
CREATE TABLE `tn_Notices` (
Id BigInt NOT NULL AUTO_INCREMENT,
ApplicationId Int(11) NOT NULL DEFAULT 0,
TypeId Int(11) NOT NULL,
UserId BigInt NOT NULL,
TemplateName Varchar(64) NOT NULL,
LeadingActorUserId BigInt NOT NULL,
LeadingActor Varchar(64) NOT NULL,
RelativeObjectUrl Varchar(255) NOT NULL DEFAULT '',
RelativeObjectName Varchar(128) NOT NULL DEFAULT '',
RelativeObjectId BigInt NOT NULL,
Body Varchar(2000) NOT NULL DEFAULT '',
Status TinyInt NOT NULL DEFAULT 0,
DateCreated DateTime NOT NULL,
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
KEY `IX_ApplicationId` (`ApplicationId`),
KEY `IX_TypeId` (`TypeId`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_OnlineUsers`;
CREATE TABLE `tn_OnlineUsers` (
Id BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL,
UserName Varchar(64) NOT NULL,
DisplayName Varchar(64) NOT NULL,
LastActivityTime DateTime NOT NULL,
LastAction Varchar(512) NOT NULL DEFAULT '',
Ip Varchar(64) NOT NULL DEFAULT '',
DateCreated DateTime NOT NULL,
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_OnlineUserStatistics`;
CREATE TABLE `tn_OnlineUserStatistics` (
Id Int(11) NOT NULL AUTO_INCREMENT,
LoggedUserCount Int(11) NOT NULL DEFAULT 0,
AnonymousCount Int(11) NOT NULL DEFAULT 0,
UserCount Int(11) NOT NULL DEFAULT 0,
DateCreated DateTime NOT NULL,
KEY `IX_UserCount` (`UserCount`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_OperationLogs`;
CREATE TABLE `tn_OperationLogs` (
Id BigInt NOT NULL AUTO_INCREMENT,
ApplicationId Int(11) NOT NULL,
Source Varchar(64) NOT NULL,
OperationType Varchar(64) NOT NULL,
OperationObjectName Varchar(128) NOT NULL DEFAULT '',
OperationObjectId BigInt NOT NULL,
Description Varchar(2000) NOT NULL,
OperatorUserId BigInt NOT NULL,
Operator Varchar(64) NOT NULL,
OperatorIP Varchar(64) NOT NULL,
AccessUrl Varchar(255) NOT NULL,
DateCreated DateTime NOT NULL,
KEY `IX_ApplicationId` (`ApplicationId`),
KEY `IX_OperationType` (`OperationType`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_OwnerData`;
CREATE TABLE `tn_OwnerData` (
Id BigInt NOT NULL AUTO_INCREMENT,
OwnerId BigInt NOT NULL,
TenantTypeId Char(6) NOT NULL DEFAULT '',
DataKey Varchar(32) NOT NULL,
LongValue BigInt NOT NULL DEFAULT 0,
DecimalValue BigInt NOT NULL,
StringValue Varchar(255) NOT NULL DEFAULT '',
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_ParsedMedias`;
CREATE TABLE `tn_ParsedMedias` (
Alias Varchar(16) NOT NULL,
Url Varchar(255) NOT NULL,
MediaType SmallInt NOT NULL,
Name Varchar(50) NOT NULL DEFAULT '',
Description Varchar(512) NOT NULL DEFAULT '',
ThumbnailUrl Varchar(255) NOT NULL DEFAULT '',
PlayerUrl Varchar(255) NOT NULL DEFAULT '',
SourceFileUrl Varchar(255) NOT NULL DEFAULT '',
DateCreated DateTime NOT NULL,
PRIMARY KEY (`Alias`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_PermissionItems`;
CREATE TABLE `tn_PermissionItems` (
ItemKey Varchar(32) NOT NULL,
ApplicationId Int(11) NOT NULL,
ItemName Varchar(64) NOT NULL DEFAULT '',
DisplayOrder Int(11) NOT NULL DEFAULT 0,
EnableQuota TinyInt NOT NULL DEFAULT 0,
EnableScope TinyInt NOT NULL DEFAULT 0,
PRIMARY KEY (`ItemKey`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_PermissionItemsInUserRoles`;
CREATE TABLE `tn_PermissionItemsInUserRoles` (
Id Int(11) NOT NULL AUTO_INCREMENT,
RoleName Varchar(32) NOT NULL,
ItemKey Varchar(32) NOT NULL,
PermissionType Int(11) NOT NULL DEFAULT 1,
PermissionQuota Float(7,2) NOT NULL DEFAULT 0,
PermissionScope Int(11) NOT NULL DEFAULT 4,
IsLocked TinyInt NOT NULL DEFAULT 0,
KEY `IX_RoleName` (`RoleName`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_PointCategories`;
CREATE TABLE `tn_PointCategories` (
CategoryKey Varchar(32) NOT NULL,
CategoryName Varchar(64) NOT NULL,
Unit Varchar(8) NOT NULL,
QuotaPerDay Int(11) NOT NULL DEFAULT 0,
Description Varchar(128) NOT NULL DEFAULT '',
DisplayOrder Int(11) NOT NULL DEFAULT 0,
PRIMARY KEY (`CategoryKey`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_PointItems`;
CREATE TABLE `tn_PointItems` (
ItemKey Varchar(32) NOT NULL,
ApplicationId Int(11) NOT NULL,
ItemName Varchar(64) NOT NULL DEFAULT '',
DisplayOrder Int(11) NOT NULL DEFAULT 0,
ExperiencePoints Int(11) NOT NULL DEFAULT 0,
ReputationPoints Int(11) NOT NULL DEFAULT 0,
TradePoints Int(11) NOT NULL DEFAULT 0,
TradePoints2 Int(11) NOT NULL DEFAULT 0,
TradePoints3 Int(11) NOT NULL DEFAULT 0,
TradePoints4 Int(11) NOT NULL DEFAULT 0,
Description Varchar(128) NOT NULL DEFAULT '',
NeedPointMessage  Smallint NOT NULL DEFAULT 0,
PRIMARY KEY (`ItemKey`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_PointRecords`;
CREATE TABLE `tn_PointRecords` (
RecordId BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL,
PointItemName Varchar(64) NOT NULL,
Description Varchar(512) NOT NULL DEFAULT '',
ExperiencePoints Int(11) NOT NULL DEFAULT 0,
ReputationPoints Int(11) NOT NULL DEFAULT 0,
TradePoints Int(11) NOT NULL DEFAULT 0,
TradePoints2 Int(11) NOT NULL DEFAULT 0,
TradePoints3 Int(11) NOT NULL DEFAULT 0,
TradePoints4 Int(11) NOT NULL DEFAULT 0,
IsIncome TinyInt NOT NULL,
DateCreated DateTime NOT NULL,
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`RecordId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_PointStatistics`;
CREATE TABLE `tn_PointStatistics` (
Id BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL,
PointCategoryKey Varchar(32) NOT NULL,
Points Int(11) NOT NULL DEFAULT 0,
StatisticalYear SmallInt NOT NULL,
StatisticalMonth SmallInt NOT NULL,
StatisticalDay SmallInt NOT NULL,
KEY `IX_PointCategoryKey` (`PointCategoryKey`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_PresentAreaNavigations`;
CREATE TABLE `tn_PresentAreaNavigations` (
Id BigInt NOT NULL AUTO_INCREMENT,
NavigationId Int(11) NOT NULL,
ParentNavigationId Int(11) NOT NULL DEFAULT 0,
Depth Int(11) NOT NULL DEFAULT 0,
PresentAreaKey Varchar(32) NOT NULL,
ApplicationId Int(11) NOT NULL,
OwnerId BigInt NOT NULL,
NavigationType Int(11) NOT NULL,
NavigationText Varchar(64) NOT NULL,
ResourceName Varchar(64) NOT NULL,
NavigationUrl Varchar(255) NOT NULL,
UrlRouteName Varchar(64) NOT NULL,
RouteDataName Varchar(255) DEFAULT NULL,
IconName Varchar(32) DEFAULT NULL,
ImageUrl Varchar(255) DEFAULT NULL,
NavigationTarget Varchar(32) DEFAULT NULL,
DisplayOrder Int(11) NOT NULL DEFAULT 100,
OnlyOwnerVisible TinyInt NOT NULL DEFAULT 0,
IsLocked TinyInt NOT NULL DEFAULT 0,
IsEnabled TinyInt NOT NULL DEFAULT 1,
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_PresentAreas`;
CREATE TABLE `tn_PresentAreas` (
PresentAreaKey Varchar(32) NOT NULL,
AllowMultipleInstances TinyInt NOT NULL DEFAULT 1,
EnableThemes TinyInt NOT NULL DEFAULT 1,
DefaultAppearanceId Varchar(128) NOT NULL,
ThemeLocation Varchar(255) NOT NULL,
PRIMARY KEY (`PresentAreaKey`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_PrivacyItems`;
CREATE TABLE `tn_PrivacyItems` (
ItemKey Varchar(32) NOT NULL,
ItemGroupId Int(11) NOT NULL,
ApplicationId Int(11) NOT NULL,
ItemName Varchar(64) NOT NULL DEFAULT '',
Description Varchar(128) NOT NULL DEFAULT '',
DisplayOrder Int(11) NOT NULL DEFAULT 0,
PrivacyStatus SmallInt NOT NULL DEFAULT 0,
KEY `IX_ItemGroupId` (`ItemGroupId`),
PRIMARY KEY (`ItemKey`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_RatingGrades`;
CREATE TABLE `tn_RatingGrades` (
Id BigInt NOT NULL AUTO_INCREMENT,
ObjectId BigInt NOT NULL DEFAULT 0,
TenantTypeId Char(6) NOT NULL,
RateNumber TinyInt NOT NULL DEFAULT 1,
RateCount Int(11) NOT NULL,
KEY `IX_ObjectId_TenantTypeId` (`ObjectId`,`TenantTypeId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_RatingRecords`;
CREATE TABLE `tn_RatingRecords` (
Id BigInt NOT NULL AUTO_INCREMENT,
ObjectId BigInt NOT NULL DEFAULT 0,
TenantTypeId Char(6) NOT NULL,
RateNumber TinyInt NOT NULL DEFAULT 1,
UserId BigInt NOT NULL DEFAULT 0,
DateCreated DateTime NOT NULL,
KEY `IX_ObjectId_TenantTypeId` (`ObjectId`,`TenantTypeId`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Ratings`;
CREATE TABLE `tn_Ratings` (
Id BigInt NOT NULL AUTO_INCREMENT,
ObjectId BigInt NOT NULL DEFAULT 0,
TenantTypeId Char(6) NOT NULL,
OwnerId BigInt NOT NULL DEFAULT 0,
RateCount Int(11) NOT NULL DEFAULT 0,
Comprehensive Float(7,2) NOT NULL,
RateSum Int(11) NOT NULL DEFAULT 0,
KEY `IX_ObjectId_TenantTypeId` (`ObjectId`,`TenantTypeId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_RecommendItems`;
CREATE TABLE `tn_RecommendItems` (
Id BigInt NOT NULL AUTO_INCREMENT,
TenantTypeId Char(6) NOT NULL,
TypeId Varchar(8) NOT NULL,
ItemId BigInt NOT NULL,
ItemName Varchar(255) NOT NULL,
FeaturedImage Varchar(512) NOT NULL DEFAULT '',
ReferrerName Varchar(64) NOT NULL,
ReferrerId BigInt NOT NULL,
DateCreated DateTime NOT NULL,
ExpiredDate DateTime NOT NULL,
DisplayOrder BigInt NOT NULL,
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
IsLink TinyInt NOT NULL DEFAULT 0,
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_RecommendItemTypes`;
CREATE TABLE `tn_RecommendItemTypes` (
TypeId Varchar(8) NOT NULL,
TenantTypeId Char(6) NOT NULL,
Name Varchar(64) NOT NULL,
Description Varchar(512) NOT NULL,
HasFeaturedImage TinyInt NOT NULL DEFAULT 0,
DateCreated DateTime NOT NULL,
KEY `IX_TenantTypeId` (`TenantTypeId`),
PRIMARY KEY (`TypeId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_RelatedTags`;
CREATE TABLE `tn_RelatedTags` (
Id BigInt NOT NULL AUTO_INCREMENT,
TagId BigInt NOT NULL,
RelatedTagId BigInt NOT NULL,
KEY `IX_RelatedTagId_TagId` (`RelatedTagId`,`TagId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_ReminderRecords`;
CREATE TABLE `tn_ReminderRecords` (
Id BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL,
ReminderModeId Int(11) NOT NULL,
ReminderInfoTypeId Int(11) NOT NULL,
ObjectId BigInt NOT NULL,
DateCreated DateTime NOT NULL,
LastReminderTime DateTime NOT NULL,
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Roles`;
CREATE TABLE `tn_Roles` (
RoleName Varchar(32) NOT NULL,
FriendlyRoleName Varchar(64) NOT NULL DEFAULT '',
IsBuiltIn TinyInt NOT NULL DEFAULT 0,
ConnectToUser TinyInt NOT NULL DEFAULT 0,
ApplicationId Int(11) NOT NULL DEFAULT 0,
IsPublic TinyInt NOT NULL DEFAULT 0,
Description Varchar(255) NOT NULL DEFAULT '',
IsEnabled TinyInt NOT NULL DEFAULT 1,
RoleImage Varchar(255) NOT NULL DEFAULT '',
PRIMARY KEY (`RoleName`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Schools`;
CREATE TABLE `tn_Schools` (
Id BigInt NOT NULL AUTO_INCREMENT,
Name Varchar(128) NOT NULL DEFAULT '',
PinyinName Varchar(512) NOT NULL DEFAULT '',
ShortPinyinName Varchar(64) NOT NULL DEFAULT '',
SchoolType SmallInt NOT NULL DEFAULT 0,
AreaCode Varchar(8) NOT NULL DEFAULT '',
DisplayOrder BigInt NOT NULL DEFAULT 0,
KEY `IX_AreaCode` (`AreaCode`),
KEY `IX_DisplayOrder` (`DisplayOrder`),
KEY `IX_SchoolType` (`SchoolType`),
KEY `IX_ShortPinyinName` (`ShortPinyinName`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_SearchedTerms`;
CREATE TABLE `tn_SearchedTerms` (
Id BigInt NOT NULL,
Term Varchar(64) NOT NULL,
SearchTypeCode Varchar(32) NOT NULL,
IsAddedByAdministrator TinyInt NOT NULL,
DisplayOrder BigInt NOT NULL DEFAULT 0,
DateCreated DateTime NOT NULL,
LastModified DateTime NOT NULL,
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_SensitiveWords`;
CREATE TABLE `tn_SensitiveWords` (
Id Int(11) NOT NULL AUTO_INCREMENT,
Word Varchar(255) NOT NULL DEFAULT '',
Replacement Varchar(255) NOT NULL DEFAULT '',
TypeId Int(11) NOT NULL DEFAULT 0,
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_SensitiveWordTypes`;
CREATE TABLE `tn_SensitiveWordTypes` (
TypeId Int(11) NOT NULL AUTO_INCREMENT,
Name Varchar(64) NOT NULL,
PRIMARY KEY (`TypeId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Settings`;
CREATE TABLE `tn_Settings` (
ClassType Varchar(128) NOT NULL,
Settings mediumtext NOT NULL,
PRIMARY KEY (`ClassType`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_ShortUrls`;
CREATE TABLE `tn_ShortUrls` (
Alias Varchar(6) NOT NULL,
Url Varchar(255) NOT NULL DEFAULT '',
OtherShortUrl Varchar(32) NOT NULL DEFAULT '',
DateCreated DateTime NOT NULL,
PRIMARY KEY (`Alias`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;
DROP TABLE IF EXISTS `tn_smtpsettings`;CREATE TABLE `tn_smtpsettings` (  `Id` bigint(20) NOT NULL AUTO_INCREMENT,  `Host` varchar(50) NOT NULL,  `Port` int(11) NOT NULL,  `EnableSsl` tinyint(4) NOT NULL,  `RequireCredentials` tinyint(4) NOT NULL,  `UserName` varchar(50) NOT NULL,  `UserEmailAddress` varchar(100) NOT NULL,  `Password` varchar(50) NOT NULL,  `ForceSmtpUserAsFromAddress` tinyint(4) NOT NULL,  `DailyLimit` int(11) NOT NULL,  PRIMARY KEY (`Id`)) ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;

DROP TABLE IF EXISTS `tn_StopedUsers`;
CREATE TABLE `tn_StopedUsers` (
Id BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL,
ToUserId BigInt NOT NULL,
ToUserDisplayName Varchar(64) NOT NULL,
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_SystemData`;
CREATE TABLE `tn_SystemData` (
Datakey Varchar(32) NOT NULL,
LongValue BigInt NOT NULL,
DecimalValue Decimal(10,2) NOT NULL,
PRIMARY KEY (`Datakey`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_TagGroups`;
CREATE TABLE `tn_TagGroups` (
GroupId BigInt NOT NULL AUTO_INCREMENT,
TenantTypeId Char(6) NOT NULL,
GroupName Varchar(64) NOT NULL,
KEY `IX_GroupName` (`GroupName`),
KEY `IX_TenantTypeId` (`TenantTypeId`),
PRIMARY KEY (`GroupId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Tags`;
CREATE TABLE `tn_Tags` (
TagId BigInt NOT NULL AUTO_INCREMENT,
TenantTypeId Char(6) NOT NULL,
TagName Varchar(64) NOT NULL,
DisplayName Varchar(64) DEFAULT NULL,
Description Varchar(512) NOT NULL DEFAULT '',
FeaturedImage Varchar(255) NOT NULL DEFAULT '',
IsFeatured TinyInt NOT NULL DEFAULT 0,
ItemCount Int(11) NOT NULL DEFAULT 0,
OwnerCount Int(11) NOT NULL DEFAULT 0,
AuditStatus SmallInt NOT NULL DEFAULT 40,
DateCreated DateTime NOT NULL,
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
KEY `IX_AuditStatus` (`AuditStatus`),
KEY `IX_ItemCount` (`ItemCount`),
KEY `IX_OwnerCount` (`OwnerCount`),
KEY `IX_TagName` (`TagName`),
KEY `IX_TenantTypeId` (`TenantTypeId`),
PRIMARY KEY (`TagId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_TagsInGroups`;
CREATE TABLE `tn_TagsInGroups` (
Id BigInt NOT NULL AUTO_INCREMENT,
GroupId BigInt NOT NULL,
TagName Varchar(64) NOT NULL,
TenantTypeId Char(6) NOT NULL DEFAULT '',
KEY `IX_GroupId_TenantTypeId` (`GroupId`,`TenantTypeId`),
KEY `IX_TagName_TenantTypeId` (`TagName`,`TenantTypeId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_TagsInOwners`;
CREATE TABLE `tn_TagsInOwners` (
Id BigInt NOT NULL AUTO_INCREMENT,
TenantTypeId Char(6) NOT NULL,
TagName Varchar(128) NOT NULL,
OwnerId BigInt NOT NULL,
ItemCount Int(11) NOT NULL DEFAULT 0,
KEY `IX_OwnerId` (`OwnerId`),
KEY `IX_TagName` (`TagName`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_TaskDetails`;
CREATE TABLE `tn_TaskDetails` (
Id Int(11) NOT NULL AUTO_INCREMENT,
Name Varchar(64) NOT NULL DEFAULT '',
TaskRule Varchar(64) NOT NULL DEFAULT '',
ClassType Varchar(255) NOT NULL,
Enabled TinyInt NOT NULL DEFAULT 1,
RunAtRestart TinyInt NOT NULL DEFAULT 1,
IsRunning TinyInt NOT NULL DEFAULT 0,
LastStart DateTime DEFAULT NULL,
LastEnd DateTime DEFAULT NULL,
LastIsSuccess TinyInt DEFAULT NULL,
NextStart DateTime DEFAULT NULL,
StartDate DateTime DEFAULT NULL,
EndDate DateTime DEFAULT NULL,
RunAtServer TinyInt DEFAULT NULL,
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_tenanttypes`;CREATE TABLE `tn_tenanttypes` (  `TenantTypeId` char(6) NOT NULL,  `ApplicationId` int(11) NOT NULL,  `Name` varchar(32) NOT NULL,  `ClassType` varchar(255) NOT NULL DEFAULT '',  PRIMARY KEY (`TenantTypeId`)) ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_tenanttypesinservices`;CREATE TABLE `tn_tenanttypesinservices` (  `Id` int(11) NOT NULL AUTO_INCREMENT,  `TenantTypeId` char(6) NOT NULL,  `ServiceKey` varchar(32) DEFAULT NULL,  PRIMARY KEY (`Id`)) ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_ThemeAppearances`;
CREATE TABLE `tn_ThemeAppearances` (
Id Varchar(128) NOT NULL,
PresentAreaKey Varchar(32) NOT NULL,
ThemeKey Varchar(32) NOT NULL,
AppearanceKey Varchar(32) NOT NULL,
Name Varchar(64) NOT NULL,
PreviewImage Varchar(255) NOT NULL,
PreviewLargeImage Varchar(255) NOT NULL DEFAULT '',
LogoFileName Varchar(64) NOT NULL DEFAULT '',
Description Varchar(1024) NOT NULL DEFAULT '',
Tags Varchar(255) NOT NULL DEFAULT '',
Author Varchar(128) NOT NULL DEFAULT '',
Copyright Varchar(512) NOT NULL DEFAULT '',
LastModified DateTime NOT NULL,
Version Varchar(10) NOT NULL DEFAULT '',
ForProductVersion Varchar(10) NOT NULL DEFAULT '',
DateCreated DateTime NOT NULL,
IsEnabled TinyInt NOT NULL DEFAULT 1,
DisplayOrder Int(11) NOT NULL DEFAULT 0,
UserCount Int(11) NOT NULL DEFAULT 0,
Roles Varchar(255) NOT NULL DEFAULT '',
RequiredRank Int(11) NOT NULL DEFAULT 0,
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Themes`;
CREATE TABLE `tn_Themes` (
Id Varchar(128) NOT NULL,
PresentAreaKey Varchar(32) NOT NULL,
ThemeKey Varchar(32) NOT NULL,
Parent Varchar(32) NOT NULL DEFAULT '',
Version Varchar(10) NOT NULL,
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_UserBlockedObjects`;
CREATE TABLE `tn_UserBlockedObjects` (
Id BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL,
ObjectType SmallInt NOT NULL,
ObjectId BigInt NOT NULL,
ObjectName Varchar(64) NOT NULL,
DateCreated DateTime NOT NULL,
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_UserInvitationSettings`;
CREATE TABLE `tn_UserInvitationSettings` (
Id BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL,
InvitationTypeKey Varchar(64) NOT NULL,
IsAllowable TinyInt NOT NULL,
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_UserNoticeSettings`;
CREATE TABLE `tn_UserNoticeSettings` (
Id BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL,
TypeId Int(11) NOT NULL,
IsAllowable TinyInt NOT NULL,
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_UserPrivacySettings`;
CREATE TABLE `tn_UserPrivacySettings` (
Id BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL,
ItemKey Varchar(32) NOT NULL,
PrivacyStatus SmallInt NOT NULL DEFAULT 0,
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_UserPrivacySpecifyObjects`;
CREATE TABLE `tn_UserPrivacySpecifyObjects` (
Id BigInt NOT NULL AUTO_INCREMENT,
UserPrivacySettingId BigInt NOT NULL,
SpecifyObjectTypeId Int(11) NOT NULL,
SpecifyObjectId BigInt NOT NULL,
SpecifyObjectName Varchar(64) NOT NULL,
DateCreated DateTime NOT NULL,
KEY `IX_SpecifyObjectTypeId` (`SpecifyObjectTypeId`),
KEY `IX_Id` (`Id`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_UserRanks`;
CREATE TABLE `tn_UserRanks` (
Rank Int(11) NOT NULL,
PointLower Int(11) NOT NULL,
RankName Varchar(64) NOT NULL,
PRIMARY KEY (`Rank`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_UserReminderSettings`;
CREATE TABLE `tn_UserReminderSettings` (
Id BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL,
ReminderModeId Int(11) NOT NULL DEFAULT 1,
ReminderInfoTypeId Int(11) NOT NULL DEFAULT 1,
ReminderThreshold Int(11) NOT NULL,
IsEnabled TinyInt NOT NULL,
IsRepeated TinyInt NOT NULL,
RepeatInterval Int(11) NOT NULL,
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Users`;
CREATE TABLE `tn_Users` (
UserId BigInt NOT NULL,
UserName Varchar(64) NOT NULL,
Password Varchar(128) NOT NULL,
PasswordFormat Int(11) NOT NULL DEFAULT 1,
PasswordQuestion Varchar(64) NOT NULL DEFAULT '',
PasswordAnswer Varchar(64) NOT NULL DEFAULT '',
AccountEmail Varchar(64) NOT NULL DEFAULT '',
IsEmailVerified TinyInt NOT NULL DEFAULT 0,
AccountMobile Varchar(64) NOT NULL DEFAULT '',
IsMobileVerified TinyInt NOT NULL DEFAULT 0,
TrueName Varchar(64) NOT NULL DEFAULT '',
NickName Varchar(64) NOT NULL DEFAULT '',
ForceLogin TinyInt NOT NULL DEFAULT 0,
IsActivated TinyInt NOT NULL DEFAULT 1,
DateCreated DateTime NOT NULL,
IpCreated Varchar(64) NOT NULL DEFAULT '',
UserType TinyInt NOT NULL DEFAULT 1,
LastActivityTime DateTime NOT NULL,
LastAction Varchar(512) NOT NULL DEFAULT '',
IpLastActivity Varchar(64) NOT NULL DEFAULT '',
IsBanned TinyInt NOT NULL DEFAULT 0,
BanReason Varchar(64) NOT NULL,
BanDeadline DateTime NOT NULL,
IsModerated TinyInt NOT NULL DEFAULT 0,
IsForceModerated TinyInt NOT NULL DEFAULT 0,
DatabaseQuota Int(11) NOT NULL DEFAULT 0,
DatabaseQuotaUsed Int(11) NOT NULL DEFAULT 0,
ThemeAppearance Varchar(128) NOT NULL,
IsUseCustomStyle TinyInt NOT NULL DEFAULT 0,
Avatar Varchar(128) NOT NULL DEFAULT '',
FollowedCount Int(11) NOT NULL DEFAULT 0,
FollowerCount Int(11) NOT NULL DEFAULT 0,
ExperiencePoints Int(11) NOT NULL DEFAULT 0,
ReputationPoints Int(11) NOT NULL DEFAULT 0,
TradePoints Int(11) NOT NULL DEFAULT 0,
TradePoints2 Int(11) NOT NULL DEFAULT 0,
TradePoints3 Int(11) NOT NULL DEFAULT 0,
TradePoints4 Int(11) NOT NULL DEFAULT 0,
FrozenTradePoints Int(11) NOT NULL DEFAULT 0,
Rank Int(11) NOT NULL DEFAULT 1,
KEY `IX_AccountEmail` (`AccountEmail`),
KEY `IX_AccountMobile` (`AccountMobile`),
KEY `IX_FollowedCount` (`FollowedCount`),
KEY `IX_FollowerCount` (`FollowerCount`),
KEY `IX_Rank` (`Rank`),
KEY `IX_UserName` (`UserName`),
PRIMARY KEY (`UserId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_UsersInRoles`;
CREATE TABLE `tn_UsersInRoles` (
Id BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL,
RoleName Varchar(32) NOT NULL,
KEY `IX_RoleName` (`RoleName`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS = 0;


DROP TABLE IF EXISTS `tn_Visit`;
CREATE TABLE `tn_Visit` (
Id BigInt NOT NULL DEFAULT 0,
TenantTypeId Char(6) NOT NULL DEFAULT '',
VisitorId BigInt NOT NULL DEFAULT 0,
Visitor Varchar(64) NOT NULL DEFAULT '',
ToObjectId BigInt NOT NULL DEFAULT 0,
ToObjectName Varchar(64) NOT NULL DEFAULT '',
LastVisitTime DateTime NOT NULL,
KEY `IX_TenantTypeId` (`TenantTypeId`),
KEY `IX_ToObjectId` (`ToObjectId`),
KEY `IX_VisitorId` (`VisitorId`),
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;

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

DROP TABLE IF EXISTS `spb_cms_Addon_Links`;
CREATE TABLE `spb_cms_Addon_Links` (
ContentItemId Int(11) NOT NULL,
Color Varchar(16) NOT NULL,
IsBold TinyInt NOT NULL,
LinkUrl Varchar(512) NOT NULL,
PRIMARY KEY (`ContentItemId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
DROP TABLE IF EXISTS `spb_cms_Addon_News`;
CREATE TABLE `spb_cms_Addon_News` (
ContentItemId BigInt NOT NULL,
TrimBodyAsSummary TinyInt NOT NULL,
Body mediumtext NOT NULL,
CopyFrom Varchar(255) NOT NULL,
CopyFromUrl Varchar(512) NOT NULL,
EnableComment TinyInt NOT NULL,
OriginalAuthor Varchar(64) NOT NULL,
Editor Varchar(64) NOT NULL,
Color Varchar(16) NOT NULL,
IsBold TinyInt NOT NULL,
FirstAsTitleImage TinyInt NOT NULL,
AutoPage TinyInt NOT NULL,
PageLength Int(11) NOT NULL,
PRIMARY KEY (`ContentItemId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
DROP TABLE IF EXISTS `spb_cms_ContentAttachments`;
CREATE TABLE `spb_cms_ContentAttachments` (
AttachmentId BigInt NOT NULL AUTO_INCREMENT,
UserId BigInt NOT NULL,
UserDisplayName Varchar(64) NOT NULL,
`FileName` Varchar(255) NOT NULL DEFAULT '',
FriendlyFileName Varchar(255) NOT NULL DEFAULT '',
MediaType Int(11) NOT NULL DEFAULT 99,
ContentType Varchar(128) NOT NULL DEFAULT '',
FileLength BigInt NOT NULL DEFAULT 0,
Height Int(11) NOT NULL DEFAULT 0,
Width Int(11) NOT NULL,
Price Int(11) NOT NULL DEFAULT 0,
`Password` Varchar(32) NOT NULL DEFAULT '',
IP Varchar(64) NOT NULL,
DateCreated DateTime NOT NULL,
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
KEY `IX_MediaType` (`MediaType`),
KEY `IX_UserId` (`UserId`),
PRIMARY KEY (`AttachmentId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
DROP TABLE IF EXISTS `spb_cms_ContentFolderModerators`;
CREATE TABLE `spb_cms_ContentFolderModerators` (
Id Int(11) NOT NULL AUTO_INCREMENT,
ContentFolderId Int(11) NOT NULL,
UserId BigInt NOT NULL,
PRIMARY KEY (`Id`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
DROP TABLE IF EXISTS `spb_cms_ContentFolders`;
CREATE TABLE `spb_cms_ContentFolders` (
ContentFolderId Int(11) NOT NULL AUTO_INCREMENT,
FolderName Varchar(255) NOT NULL,
`Description` Varchar(2000) NOT NULL,
ParentId Int(11) NOT NULL,
ParentIdList Varchar(255) NOT NULL,
ChildCount Int(11) NOT NULL,
Depth Int(11) NOT NULL,
IsEnabled TinyInt NOT NULL,
ContentItemCount Int(11) NOT NULL,
DateCreated DateTime NOT NULL,
ContentTypeKeys Varchar(255) NOT NULL,
DisplayOrder Int(11) NOT NULL,
EnableContribute TinyInt NOT NULL,
IsAsNavigation TinyInt NOT NULL,
NeedAuditing TinyInt NOT NULL,
IsLink TinyInt NOT NULL,
LinkUrl Varchar(255) NOT NULL,
IsLinkToNewWindow TinyInt NOT NULL,
Page_List Varchar(128) NOT NULL,
Page_Detail Varchar(128) NOT NULL,
ExtensionField mediumtext DEFAULT NULL,
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
PRIMARY KEY (`ContentFolderId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
DROP TABLE IF EXISTS `spb_cms_ContentItems`;
CREATE TABLE `spb_cms_ContentItems` (
ContentItemId BigInt NOT NULL AUTO_INCREMENT,
ContentFolderId Int(11) NOT NULL,
ContentTypeId Int(11) NOT NULL,
Title Varchar(255) NOT NULL,
FeaturedImageAttachmentId BigInt DEFAULT NULL,
FeaturedImage Varchar(255) DEFAULT NULL,
UserId BigInt NOT NULL,
Author Varchar(64) NOT NULL,
Summary Varchar(512) NOT NULL,
IsContributed TinyInt NOT NULL,
IsEssential TinyInt NOT NULL,
IsGlobalSticky TinyInt NOT NULL,
GlobalStickyDate DateTime NOT NULL,
IsFolderSticky TinyInt NOT NULL,
FolderStickyDate DateTime NOT NULL,
IsLocked TinyInt NOT NULL,
AuditStatus SmallInt NOT NULL,
IP Varchar(64) NOT NULL,
ReleaseDate DateTime NOT NULL,
DateCreated DateTime NOT NULL,
LastModified DateTime NOT NULL,
DisplayOrder BigInt NOT NULL,
PropertyNames mediumtext DEFAULT NULL,
PropertyValues mediumtext DEFAULT NULL,
PRIMARY KEY (`ContentItemId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
DROP TABLE IF EXISTS `spb_cms_ContentTypeColumnDefinitions`;
CREATE TABLE `spb_cms_ContentTypeColumnDefinitions` (
ColumnId Int(11) NOT NULL AUTO_INCREMENT,
ContentTypeId Int(11) NOT NULL,
ColumnName Varchar(64) NOT NULL,
ColumnLabel Varchar(128) NOT NULL,
IsBuiltIn TinyInt NOT NULL,
DataType Varchar(64) NOT NULL,
`Length` Int(11) NOT NULL,
`Precision` Varchar(64) NOT NULL,
IsNotNull TinyInt NOT NULL,
DefaultValue Varchar(64) NOT NULL,
IsIndex TinyInt NOT NULL,
IsUnique TinyInt NOT NULL,
KeyOrIndexName Varchar(64) NOT NULL,
KeyOrIndexColumns Varchar(255) NOT NULL,
ControlCode Varchar(64) NOT NULL,
InitialValue Varchar(20) NOT NULL,
EnableInput TinyInt NOT NULL,
EnableEdit TinyInt NOT NULL,
ValidateRole Varchar(64) DEFAULT NULL,
PRIMARY KEY (`ColumnId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
DROP TABLE IF EXISTS `spb_cms_ContentTypeDefinitions`;
CREATE TABLE `spb_cms_ContentTypeDefinitions` (
ContentTypeId Int(11) NOT NULL AUTO_INCREMENT,
ContentTypeName Varchar(64) NOT NULL,
ContentTypeKey Varchar(64) NOT NULL,
IsBuiltIn TinyInt NOT NULL,
DisplayOrder Int(11) NOT NULL,
TableName Varchar(64) NOT NULL,
ForeignKey Varchar(64) NOT NULL,
Page_New Varchar(128) NOT NULL,
Page_Edit Varchar(128) NOT NULL,
Page_Manage Varchar(128) NOT NULL,
Page_Default_List Varchar(128) NOT NULL,
Page_Default_Detail Varchar(128) NOT NULL,
IsEnabled TinyInt NOT NULL,
EnableContribute TinyInt NOT NULL,
EnableComment TinyInt NOT NULL,
EnableAttachment TinyInt NOT NULL,
AllowContributeRoleNames Varchar(512) NOT NULL DEFAULT '',
PRIMARY KEY (`ContentTypeId`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
DROP TABLE IF EXISTS `spb_cms_FormControlDefinitions`;
CREATE TABLE `spb_cms_FormControlDefinitions` (
ControlCode Varchar(64) NOT NULL,
ControlName Varchar(64) NOT NULL,
`Description` Varchar(255) DEFAULT NULL,
PRIMARY KEY (`ControlCode`)
)ENGINE=innodb DEFAULT CHARSET=utf8;
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
