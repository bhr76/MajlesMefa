IF OBJECT_ID(N'dbo.LoanErrorLogs', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.LoanErrorLogs
    (
        Id BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_LoanErrorLogs PRIMARY KEY,
        OccurredAtUtc DATETIME2(3) NOT NULL,
        Action NVARCHAR(100) NOT NULL,
        Category NVARCHAR(50) NOT NULL,
        UserId UNIQUEIDENTIFIER NULL,
        UserName NVARCHAR(256) NULL,
        InputJson NVARCHAR(MAX) NULL,
        ErrorMessage NVARCHAR(MAX) NULL,
        ExceptionDetails NVARCHAR(MAX) NULL,
        TraceIdentifier NVARCHAR(256) NULL
    );

    CREATE INDEX IX_LoanErrorLogs_OccurredAtUtc ON dbo.LoanErrorLogs (OccurredAtUtc DESC);
    CREATE INDEX IX_LoanErrorLogs_UserId_OccurredAtUtc ON dbo.LoanErrorLogs (UserId, OccurredAtUtc DESC);
END;
