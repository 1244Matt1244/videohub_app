IF DB_ID('VideoApp') IS NULL
BEGIN
    CREATE DATABASE VideoApp;
END
GO

USE VideoApp;
GO

IF OBJECT_ID('dbo.Users', 'U') IS NULL
BEGIN
    CREATE TABLE Users (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        Email NVARCHAR(255) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(255) NULL,
        FullName NVARCHAR(255) NOT NULL,
        GoogleId NVARCHAR(255) NULL,
        ProfilePictureUrl NVARCHAR(500) NULL,
        IsPremium BIT NOT NULL DEFAULT 0,
        PremiumExpiresAt DATETIME2 NULL,
        DarkMode BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );
END
GO

IF OBJECT_ID('dbo.Videos', 'U') IS NULL
BEGIN
    CREATE TABLE Videos (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        UserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id),
        Title NVARCHAR(255) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        MuxUploadId NVARCHAR(255) NULL,
        MuxAssetId NVARCHAR(255) NULL,
        MuxPlaybackId NVARCHAR(255) NULL,
        ThumbnailUrl NVARCHAR(500) NULL,
        IsPremium BIT NOT NULL DEFAULT 0,
        Price DECIMAL(10,2) NOT NULL DEFAULT 0,
        Status NVARCHAR(50) NOT NULL DEFAULT 'preparing',
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );

    CREATE INDEX IX_Videos_UserId ON Videos(UserId);
    CREATE INDEX IX_Videos_MuxUploadId ON Videos(MuxUploadId);
    CREATE INDEX IX_Videos_MuxAssetId ON Videos(MuxAssetId);
END
GO

IF OBJECT_ID('dbo.Payments', 'U') IS NULL
BEGIN
    CREATE TABLE Payments (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        UserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id),
        VideoId UNIQUEIDENTIFIER NULL FOREIGN KEY REFERENCES Videos(Id),
        StripeSessionId NVARCHAR(255) NOT NULL,
        Amount DECIMAL(10,2) NOT NULL,
        Status NVARCHAR(50) NOT NULL DEFAULT 'pending',
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );

    CREATE INDEX IX_Payments_SessionId ON Payments(StripeSessionId);
END
GO
