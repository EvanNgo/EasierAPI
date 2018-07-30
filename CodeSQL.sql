USE [easi5810_easierjlpt]
GO

/****** Object: Table [dbo].[Users] Script Date: 27/07/2018 16:53:21 *****
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [Users]

CREATE TABLE [dbo].[Users] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [UserName]    VARCHAR (25) NOT NULL,
    [Email]       VARCHAR (50) NOT NULL,
    [Birthyear]   INT          NOT NULL,
    [HashPass]    VARCHAR (2083)  NULL,
    [CreatedDate] DATE         NOT NULL,
    [UpdatedDate] DATE         NOT NULL,
    [EmailVerity] BIT          NOT NULL,
	[Thumbnail] VARCHAR(2083)  NULL
);

CREATE TABLE [dbo].[Questions] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [UserId]      INT				NOT NULL,
    [Title]       VARCHAR (255)		NOT NULL,
	[Message]     VARCHAR (1000)	NULL,
	[Thumbnail]   VARCHAR(2083)		NULL,
	[Level]		  INT				NOT NULL,
	[ColorId]     INT				NULL,
    [CreatedDate] DATE				NOT NULL,
);

ALTER TABLE [dbo].[Users]  
ADD CONSTRAINT PK_Users PRIMARY KEY CLUSTERED (Id);  

ALTER TABLE [dbo].[Questions] ADD CONSTRAINT FK_User FOREIGN KEY(UserId) REFERENCES Users(Id);
*/