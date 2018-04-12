USE Revuvu
GO

DROP PROCEDURE IF EXISTS dbo.GetAllReviews
DROP PROCEDURE IF EXISTS dbo.GetReviewsByCategoryName
DROP PROCEDURE IF EXISTS dbo.AddReview
DROP PROCEDURE IF EXISTS dbo.EditReview
DROP PROCEDURE IF EXISTS dbo.DeleteReview
DROP PROCEDURE IF EXISTS dbo.GetReviewById
DROP PROCEDURE IF EXISTS dbo.GetCommentsByReviewId
DROP PROCEDURE IF EXISTS dbo.GetAllTags
DROP PROCEDURE IF EXISTS dbo.GetAllCategories
DROP PROCEDURE IF EXISTS dbo.GetCategoryByReviewId
DROP PROCEDURE IF EXISTS dbo.AddCategory
DROP PROCEDURE IF EXISTS dbo.DeleteCategory
DROP PROCEDURE IF EXISTS dbo.EditCategory
DROP PROCEDURE IF EXISTS dbo.GetCategoryById
DROP PROCEDURE IF EXISTS dbo.AddTag
DROP PROCEDURE IF EXISTS dbo.EditTag
DROP PROCEDURE IF EXISTS dbo.GetTagsByReviewId
DROP PROCEDURE IF EXISTS dbo.AddComment
DROP PROCEDURE IF EXISTS dbo.DeleteComment
DROP PROCEDURE IF EXISTS dbo.GetCommentsByReviewId
DROP PROCEDURE IF EXISTS dbo.GetTop5ReviewsByDate
DROP PROCEDURE IF EXISTS dbo.GetActiveLayout
DROP PROCEDURE IF EXISTS dbo.EditLayout
DROP PROCEDURE IF EXISTS dbo.DeleteLayout
DROP PROCEDURE IF EXISTS dbo.AddLayout
DROP PROCEDURE IF EXISTS dbo.GetLayoutById
DROP PROCEDURE IF EXISTS dbo.UpdateActiveLayout
DROP PROCEDURE IF EXISTS dbo.AddTagToReview
DROP PROCEDURE IF EXISTS dbo.GetReviewsByTagId
DROP PROCEDURE IF EXISTS dbo.GetTagByTagId
DROP PROCEDURE IF EXISTS dbo.GetReviewsByTagName
DROP PROCEDURE IF EXISTS dbo.AddPage
DROP PROCEDURE IF EXISTS dbo.EditPage
DROP PROCEDURE IF EXISTS dbo.DeletePage
DROP PROCEDURE IF EXISTS dbo.GetPageById
DROP PROCEDURE IF EXISTS dbo.GetAllPages
DROP PROCEDURE IF EXISTS dbo.DeleteTag
DROP PROCEDURE IF EXISTS dbo.EditTagsForReview
DROP PROCEDURE IF EXISTS dbo.DeleteTagsAssociatedWithReview

GO



CREATE PROCEDURE DeleteTag (
	@TagId int
) AS
BEGIN
	BEGIN TRANSACTION
	DELETE FROM ReviewTags WHERE TagId = @TagId
	DELETE FROM Tags WHERE TagId = @TagId;
	

	COMMIT TRANSACTION
END
GO

CREATE PROCEDURE GetAllPages AS
BEGIN
	SELECT PageId, PageTitle, PageBody
	FROM Pages
END
GO

CREATE PROCEDURE GetPageById(
	@PageId int
) AS
BEGIN
	SELECT PageId, PageTitle, PageBody
	FROM Pages
	WHERE PageId = @PageId;
END
GO

CREATE PROCEDURE DeletePage (
	@PageId int
) AS
BEGIN
	BEGIN TRANSACTION

	DELETE FROM Pages WHERE PageId = @PageId;

	COMMIT TRANSACTION
END
GO

CREATE PROCEDURE EditPage (
	@PageId int,
	@PageTitle nvarchar(100),
	@PageBody nvarchar(max)
) AS
BEGIN
	UPDATE Pages SET
	PageTitle = @PageTitle,
	PageBody = @PageBody

	WHERE PageId = @PageId;
END
GO

CREATE PROCEDURE AddPage (
	@PageId int output,
	@PageTitle nvarchar(100),
	@PageBody nvarchar(max)
) AS
BEGIN
	INSERT INTO Pages(PageTitle, PageBody)
	VALUES (@PageTitle, @PageBody)

	SET @PageId = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE GetReviewsByTagName(
	@TagName nvarchar(30)
) AS
BEGIN
	SELECT r.ReviewId, r.CategoryId, ReviewTitle, ReviewBody, Rating, DateCreated,
	DatePublished, UpVotes, DownVotes, IsApproved, UserId
	FROM Reviews r
	INNER JOIN ReviewTags rt ON rt.ReviewId = r.ReviewId
	INNER JOIN Tags t ON rt.TagId = t.TagId
	WHERE t.TagName = @TagName AND IsApproved = 1
END
GO

CREATE PROCEDURE GetTagByTagId(
	@TagId int
) AS
BEGIN
	SELECT TagId, TagName
	FROM Tags
	WHERE TagId = @TagId;
END
GO

CREATE PROCEDURE GetReviewsByTagId(
	@TagId int
) AS
BEGIN
	SELECT r.ReviewId, r.CategoryId, ReviewTitle, ReviewBody, Rating, DateCreated,
	DatePublished, UpVotes, DownVotes, IsApproved, UserId
	FROM Reviews r
	INNER JOIN ReviewTags rt ON rt.ReviewId = r.ReviewId
	INNER JOIN Tags t ON rt.TagId = t.TagId AND IsApproved = 1
	WHERE rt.TagId = @TagId
END
GO

CREATE PROCEDURE AddTagToReview(
	@ReviewId int,
	@TagId int
) AS
BEGIN
	INSERT INTO ReviewTags(ReviewId, TagId)
	VALUES (@ReviewId, @TagId)
END
GO

CREATE PROCEDURE UpdateActiveLayout(
	@LayoutId int
) AS
BEGIN
	UPDATE Layouts SET
	IsActive = 0

	WHERE LayoutId <> @LayoutId;
END
GO

CREATE PROCEDURE GetLayoutById(
	@LayoutId int
) AS
BEGIN
	SELECT LayoutId, LayoutName, LogoImageFile, ColorMain, ColorSecondary,
	HeaderTitle, BannerText, IsActive
	FROM Layouts
	WHERE LayoutId = @LayoutId;
END
GO

CREATE PROCEDURE EditLayout (
	@LayoutId int,
	@LayoutName nvarchar(50),
	@LogoImageFile nvarchar(300),
	@ColorMain nvarchar(50),
	@ColorSecondary nvarchar(50),
	@HeaderTitle nvarchar(50),
	@BannerText nvarchar(1000),
	@IsActive bit
) AS
BEGIN
	UPDATE Layouts SET
	LayoutName = @LayoutName,
	LogoImageFile = @LogoImageFile,
	ColorMain = @ColorMain,
	ColorSecondary = @ColorSecondary,
	HeaderTitle = @HeaderTitle,
	BannerText = @BannerText,
	IsActive = @IsActive

	WHERE LayoutId = @LayoutId;
END
GO

CREATE PROCEDURE DeleteLayout (
	@LayoutId int
) AS
BEGIN
	BEGIN TRANSACTION

	DELETE FROM Layouts WHERE LayoutId = @LayoutId;

	COMMIT TRANSACTION
END
GO

CREATE PROCEDURE AddLayout(
	@LayoutId int output,
	@LayoutName nvarchar(50),
	@LogoImageFile nvarchar(300),
	@ColorMain nvarchar(50),
	@ColorSecondary nvarchar(50),
	@HeaderTitle nvarchar(50),
	@BannerText nvarchar(1000),
	@IsActive bit
) AS
BEGIN
	INSERT INTO Layouts(LayoutName, LogoImageFile, ColorMain,ColorSecondary,
	HeaderTitle, BannerText, IsActive)
	VALUES (@LayoutName, @LogoImageFile, @ColorMain, @ColorSecondary,
	@HeaderTitle, @BannerText, @IsActive)

	SET @LayoutId = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE GetActiveLayout AS
BEGIN
	SELECT LayoutId, LayoutName, LogoImageFile, 
	ColorMain, ColorSecondary, HeaderTitle, BannerText, IsActive
	FROM Layouts
	WHERE IsActive = 1;
END
GO

CREATE PROCEDURE GetTagsByReviewId(
	@ReviewId int
) AS
BEGIN
	SELECT t.TagId, TagName
	FROM Tags t
	INNER JOIN ReviewTags rt ON t.TagId = rt.TagId
	INNER JOIN Reviews r ON r.ReviewId = rt.ReviewId
	WHERE rt.ReviewId = @ReviewId
	ORDER BY TagName;
END
GO

CREATE PROCEDURE GetCategoryByReviewId(
	@ReviewId int
) AS
BEGIN
	SELECT c.CategoryId, CategoryName
	FROM Categories c
	INNER JOIN Reviews r ON c.CategoryId = r.CategoryId
	WHERE r.ReviewId = @ReviewId;
END
GO

CREATE PROCEDURE GetTop5ReviewsByDate AS
BEGIN
	SELECT TOP 5 ReviewId, CategoryId, ReviewTitle, ReviewBody, Rating, DateCreated,
	DatePublished, UpVotes, DownVotes, IsApproved, UserId
	FROM Reviews
	WHERE DateCreated < CURRENT_TIMESTAMP AND IsApproved = 1
	ORDER BY DatePublished DESC
END
GO
	
CREATE PROCEDURE GetCategoryById(
	@CategoryId int
) AS
BEGIN
	SELECT CategoryId, CategoryName
	FROM Categories
	WHERE CategoryId = @CategoryId;
END
GO

CREATE PROCEDURE EditCategory (
	@CategoryId int,
	@CategoryName nvarchar(100)
) AS
BEGIN
	UPDATE Categories SET
	CategoryName = @CategoryName

	WHERE CategoryId = @CategoryId;
END
GO

CREATE PROCEDURE DeleteCategory (
	@CategoryId int
) AS
BEGIN
	BEGIN TRANSACTION
	UPDATE Reviews
	SET CategoryId = NULL
	WHERE CategoryId = @CategoryId
	DELETE FROM Categories WHERE CategoryId = @CategoryId;

	COMMIT TRANSACTION
END
GO

CREATE PROCEDURE AddCategory (
	@CategoryId int output,
	@CategoryName nvarchar(100)
) AS
BEGIN
	INSERT INTO Categories(CategoryName)
	VALUES (@CategoryName)

	SET @CategoryId = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE EditReview (
	@ReviewId int,
	@CategoryId int,
	@ReviewTitle nvarchar(150),
	@ReviewBody nvarchar(max),
	@Rating decimal(2,1),
	@DatePublished datetime2(7),
	@UpVotes int,
	@DownVotes int,
	@IsApproved bit
) AS
BEGIN
	UPDATE Reviews SET
	CategoryId = @CategoryId,
	ReviewTitle = @ReviewTitle,
	ReviewBody = @ReviewBody,
	Rating = @Rating,
	DatePublished = @DatePublished,
	UpVotes = @UpVotes,
	DownVotes = @DownVotes,
	IsApproved = @IsApproved


	WHERE ReviewId = @ReviewId;
END
GO

CREATE PROCEDURE AddReview (
	@ReviewId int output,
	@CategoryId int,
	@ReviewTitle nvarchar(150),
	@ReviewBody nvarchar(max),
	@Rating decimal(2,1),
	@DatePublished datetime2(7),
	@UpVotes int,
	@DownVotes int,
	@IsApproved bit,
	@UserId NVARCHAR(128)
) AS
BEGIN
	INSERT INTO Reviews(CategoryId, ReviewTitle, ReviewBody, Rating, 
	DatePublished, UpVotes, DownVotes, IsApproved, UserId)
	VALUES (@CategoryId, @ReviewTitle, @ReviewBody, @Rating, 
	@DatePublished, @UpVotes, @DownVotes, @IsApproved, @UserId)

	SET @ReviewId = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE DeleteReview (
	@ReviewId int
) AS
BEGIN
	BEGIN TRANSACTION

	DELETE FROM Comments WHERE ReviewId = @ReviewId
	DELETE FROM ReviewTags WHERE ReviewId = @ReviewId
	DELETE FROM Reviews WHERE ReviewId = @ReviewId;

	COMMIT TRANSACTION
END
GO

CREATE PROCEDURE DeleteComment (
	@CommentId int
) AS
BEGIN
	BEGIN TRANSACTION

	DELETE FROM Comments WHERE CommentId = @CommentId;

	COMMIT TRANSACTION
END
GO

CREATE PROCEDURE AddComment (
	@CommentId int output,
	@ReviewId int,
	@CommentBody nvarchar(180),
	@IsDisplayed bit
) AS
BEGIN
	INSERT INTO Comments(ReviewId, CommentBody, IsDisplayed)
	VALUES (@ReviewId, @CommentBody, @IsDisplayed)

	SET @CommentId = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE EditTag (
	@TagId int,
	@TagName nvarchar(30)
) AS
BEGIN
	UPDATE Tags SET
	TagName = @TagName

	WHERE TagId = @TagId;
END
GO

CREATE PROCEDURE AddTag (
	@TagId int output,
	@TagName nvarchar(30)
) AS 
BEGIN
	INSERT INTO Tags(TagName)
	VALUES (@TagName)

	SET @TagId = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE GetAllCategories AS
BEGIN
	SELECT CategoryId, CategoryName
	FROM Categories
	ORDER BY CategoryName;
END
GO

CREATE PROCEDURE GetAllTags AS
BEGIN
	SELECT TagId, TagName
	FROM Tags
	ORDER BY TagName
END
GO

CREATE PROCEDURE GetAllReviews AS
BEGIN
	SELECT ReviewId, CategoryId, ReviewTitle, ReviewBody, Rating, DateCreated,
	DatePublished, UpVotes, DownVotes, IsApproved, UserId
	FROM Reviews
END
GO

CREATE PROCEDURE GetReviewById(
	@ReviewId int
) AS
BEGIN
	SELECT ReviewId, c.CategoryId, ReviewTitle, ReviewBody, Rating, DateCreated,
	DatePublished, UpVotes, DownVotes, IsApproved, UserId
	FROM Reviews r
	INNER JOIN Categories c ON c.CategoryId = r.CategoryId
	WHERE ReviewId = @ReviewId
END
GO

CREATE PROCEDURE GetReviewsByCategoryName(
	@CategoryName nvarchar(100)
) AS
BEGIN
	SELECT ReviewId, c.CategoryId, ReviewTitle, ReviewBody, Rating, DateCreated,
	DatePublished, UpVotes, DownVotes, IsApproved, UserId
	FROM Reviews r
	INNER JOIN Categories c ON c.CategoryId = r.CategoryId
	WHERE c.CategoryName = @CategoryName AND IsApproved = 1
END
GO

CREATE PROCEDURE GetCommentsByReviewId(
	@ReviewId int
) AS
BEGIN
	SELECT CommentId, r.ReviewId, CommentBody, IsDisplayed
	FROM Comments c
	INNER JOIN Reviews r ON r.ReviewId = c.ReviewId
	WHERE c.ReviewId = @ReviewId
END
GO


CREATE PROCEDURE EditTagsForReview(
	@ReviewId int,
	@TagId int
)AS
BEGIN 
	--DELETE FROM ReviewTags WHERE ReviewId = @ReviewId AND TagId = @TagId

	INSERT INTO ReviewTags(ReviewId, TagId)
	VALUES(@ReviewId, @TagId)
END
GO


CREATE PROCEDURE DeleteTagsAssociatedWithReview(
	@ReviewId int
)AS
BEGIN
	DELETE FROM ReviewTags WHERE ReviewId = @ReviewId

END
GO