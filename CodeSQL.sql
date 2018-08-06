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
	[AnswerCount]   INT	DEFAULT(0)	NULL,
	[Level]		  INT				NOT NULL,
	[ColorId]     INT				NULL,
    [CreatedDate] DATE				NOT NULL,
);

CREATE TABLE [dbo].[Choices] (
    [Id]          INT PRIMARY KEY IDENTITY (1, 1) NOT NULL,
    [QuestionId]      INT				NOT NULL,
    [Message]       VARCHAR (255)	 NULL,
	[Thumbnail]   VARCHAR(2083)		NULL,
	[SelectedCount]   INT	DEFAULT(0)	NULL,
	[IsTrue]		  BIT				DEFAULT (0)
);

CREATE TABLE [dbo].[QuestionLikes] (
    [Id]          INT PRIMARY KEY IDENTITY (1, 1) NOT NULL,
    [QuestionId]      INT				NOT NULL,
    [UserId]       INT				NOT NULL,
);

CREATE TABLE [dbo].[QuestionAnswers] (
    [Id]          INT PRIMARY KEY IDENTITY (1, 1) NOT NULL,
    [QuestionId]      INT				NOT NULL,
    [UserId]       INT				NOT NULL,
	[ChoiceId]       INT				NOT NULL,
);

CREATE TABLE [dbo].[QuestionComments] (
    [Id]          INT PRIMARY KEY IDENTITY (1, 1) NOT NULL,
    [QuestionId]      INT				NOT NULL,
    [UserId]       INT				NOT NULL,
	[Message]        NVARCHAR(2083)				NOT NULL,
);

ALTER TABLE [dbo].[Users]  

ADD CONSTRAINT PK_Users PRIMARY KEY CLUSTERED (Id);  

ALTER TABLE [dbo].[Questions] ADD CONSTRAINT FK_User FOREIGN KEY(UserId) REFERENCES Users(Id);

ALTER TABLE [dbo].[Choices] ADD CONSTRAINT FK_Question FOREIGN KEY(QuestionId) REFERENCES Questions(Id);

ALTER TABLE [dbo].[Questions] ADD AnswerCount INT DEFAULT(0);

ALTER TABLE [dbo].[QuestionComments] ADD CreatedDate DATE NOT NULL DEFAULT(GETDATE());
*/
