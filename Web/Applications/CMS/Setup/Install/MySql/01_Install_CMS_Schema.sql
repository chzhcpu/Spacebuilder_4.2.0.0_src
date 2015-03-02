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
