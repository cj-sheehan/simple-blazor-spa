CREATE PROCEDURE [dbo].[usp_NameInsert]
	@name NVARCHAR(200)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.Name (Name)
	VALUES (@name);

	DECLARE @id INT = SCOPE_IDENTITY();
	EXEC [dbo].[usp_NameGet] @id;
END;