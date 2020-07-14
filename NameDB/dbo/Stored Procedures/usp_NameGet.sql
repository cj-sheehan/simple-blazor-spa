CREATE PROCEDURE [dbo].[usp_NameGet]
	@id int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		n.Id,
		n.Name,
		n.DateCreated
	FROM dbo.Name AS n
	WHERE n.Id = @id;
END;
