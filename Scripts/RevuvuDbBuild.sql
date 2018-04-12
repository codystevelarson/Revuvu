USE Revuvu
GO

IF EXISTS (SELECT * FROM SYS.TABLES WHERE NAME ='ReviewTags')
	DROP TABLE ReviewTags
GO

IF EXISTS (SELECT * FROM SYS.TABLES WHERE NAME ='Tags')
	DROP TABLE Tags
GO

IF EXISTS (SELECT * FROM SYS.TABLES WHERE NAME ='Comments')
	DROP TABLE Comments
GO

IF EXISTS (SELECT * FROM SYS.TABLES WHERE NAME ='Reviews')
	DROP TABLE Reviews
GO

IF EXISTS (SELECT * FROM SYS.TABLES WHERE NAME ='Categories')
	DROP TABLE Categories
GO

IF EXISTS (SELECT * FROM SYS.TABLES WHERE NAME ='Layouts')
	DROP TABLE Layouts
GO

IF EXISTS (SELECT * FROM SYS.TABLES WHERE NAME ='Pages')
	DROP TABLE Pages
GO

CREATE TABLE Pages(
	PageId int identity(1,1) primary key,
	PageTitle nvarchar(100) not null,
	PageBody nvarchar(max) not null
)

CREATE TABLE Tags(
	TagId int identity(1,1) primary key,
	TagName nvarchar(30) not null
)

CREATE TABLE Categories(
	CategoryId int identity (1,1) primary key,
	CategoryName nvarchar (100) not null,
)

CREATE TABLE Reviews(
	ReviewId int identity(1,1) primary key,
	CategoryId int foreign key references Categories(CategoryId) NULL,
	ReviewTitle nvarchar(150) not null,
	ReviewBody nvarchar(max) not null,
	Rating decimal(2,1) not null,
	DateCreated datetime2 not null default(getdate()),
	DatePublished datetime2 null,
	UpVotes int null,
	DownVotes int null,
	IsApproved bit not null,
	UserId NVARCHAR(128) not null FOREIGN KEY REFERENCES AspNetUsers(Id)
)

CREATE TABLE ReviewTags(
	ReviewId int not null,
	TagId int not null

	CONSTRAINT PK_ReviewTags PRIMARY KEY (ReviewId, TagId),
	CONSTRAINT FK_Reviews_ReviewTags FOREIGN KEY (ReviewId) REFERENCES Reviews(ReviewId),
	CONSTRAINT FK_Tags_ReviewTags FOREIGN KEY (TagId) REFERENCES Tags(TagId)
)

CREATE TABLE Comments(
	CommentId int identity(1,1) primary key,
	ReviewId int FOREIGN KEY REFERENCES Reviews(ReviewId) not null,
	CommentBody nvarchar(180) not null,
	IsDisplayed bit not null
)

CREATE TABLE Layouts(
	LayoutId int identity(1,1) not null,
	LayoutName nvarchar(50) not null,
	LogoImageFile nvarchar(300) not null,
	ColorMain nvarchar(50) not null,
	ColorSecondary nvarchar(50) not null,
	HeaderTitle nvarchar(50) not null,
	BannerText nvarchar(1000) not null,
	IsActive bit not null
)

