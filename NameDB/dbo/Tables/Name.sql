﻿CREATE TABLE [dbo].[Name]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(200) NOT NULL, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
)
