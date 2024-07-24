CREATE TABLE [dbo].[Cities] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (100) NOT NULL,
    [Country]   VARCHAR (100) NOT NULL,
    [Latitude]  FLOAT (53)    NOT NULL,
    [Longitude] FLOAT (53)    NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


