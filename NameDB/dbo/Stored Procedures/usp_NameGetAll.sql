CREATE PROCEDURE [dbo].[usp_NameGetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		n.Id,
		n.Name,
		n.DateCreated
	FROM dbo.Name AS n
END;
