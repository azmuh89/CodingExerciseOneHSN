
If OBJECT_ID('dbo.Rectangle') Is Null
	CREATE TABLE [dbo].[Rectangle] (
		Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
		Name nvarchar(100) NOT NULL,
		Width int NOT NULL,
		Height int NOT NULL,
	)
	
If OBJECT_ID('dbo.RectangleCoordinates') Is Null
	CREATE TABLE [dbo].[RectangleCoordinates] (
		Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
		RectangleId int Foreign Key References dbo.Rectangle (Id) NOT NULL,
		X int NOT NULL,
		Y int NOT NULL
	)

If IndexProperty(OBJECT_ID('dbo.Rectangle'), 'ix_Rectangle_Width', 'IndexId') Is Null
	Create Index ix_Rectangle_Width On dbo.Rectangle (Width)
	
If IndexProperty(OBJECT_ID('dbo.Rectangle'), 'ix_Rectangle_Height', 'IndexId') Is Null
	Create Index ix_Rectangle_Height On dbo.Rectangle (Height)
	
If IndexProperty(OBJECT_ID('dbo.RectangleCoordinates'), 'ix_RectangleCoordinates_X', 'IndexId') Is Null
	Create Index ix_RectangleCoordinates_X On dbo.RectangleCoordinates (X)
	
If IndexProperty(OBJECT_ID('dbo.RectangleCoordinates'), 'ix_RectangleCoordinates_Y', 'IndexId') Is Null
	Create Index ix_RectangleCoordinates_Y On dbo.RectangleCoordinates (Y)

