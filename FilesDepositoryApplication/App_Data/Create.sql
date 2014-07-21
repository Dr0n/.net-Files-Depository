CREATE TABLE [dbo].[Files] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [name]          NVARCHAR (MAX) NOT NULL,
    [original_name] NVARCHAR (MAX) NOT NULL,
    [date]          DATETIME       NOT NULL,
    [type]          VARCHAR (20)   NOT NULL,
    CONSTRAINT [PK_Files] PRIMARY KEY CLUSTERED ([Id] ASC)
);

