BEGIN TRANSACTION;
GO

CREATE TABLE [Types] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Types] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230218064742_AddType', N'7.0.2');
GO

COMMIT;
GO

