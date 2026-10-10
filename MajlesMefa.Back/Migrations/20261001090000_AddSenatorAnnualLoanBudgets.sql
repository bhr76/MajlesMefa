/* Applies migration 20261001090000_AddSenatorAnnualLoanBudgets to SQL Server.
   Safe to rerun: existing table/index/FK and migration-history entry are preserved. */
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.__EFMigrationsHistory', N'U') IS NULL
    THROW 50001, 'EF migrations history table dbo.__EFMigrationsHistory was not found.', 1;

BEGIN TRY
    BEGIN TRANSACTION;

    IF OBJECT_ID(N'dbo.SenatorAnnualLoanBudgets', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.SenatorAnnualLoanBudgets
        (
            Id UNIQUEIDENTIFIER NOT NULL,
            SenatorId UNIQUEIDENTIFIER NOT NULL,
            [Year] INT NOT NULL,
            GharzolHasaneLimit BIGINT NOT NULL,
            MorabeheLimit BIGINT NOT NULL,
            GharzolHasaneUsed BIGINT NOT NULL,
            MorabeheUsed BIGINT NOT NULL,
            Created DATETIME2 NOT NULL CONSTRAINT DF_SenatorAnnualLoanBudgets_Created DEFAULT (GETDATE()),
            CreatorUserId UNIQUEIDENTIFIER NOT NULL,
            CONSTRAINT PK_SenatorAnnualLoanBudgets PRIMARY KEY (Id),
            CONSTRAINT FK_SenatorAnnualLoanBudgets_SenatorProfiles_SenatorId
                FOREIGN KEY (SenatorId) REFERENCES dbo.SenatorProfiles (UserId) ON DELETE NO ACTION
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1 FROM sys.indexes
        WHERE object_id = OBJECT_ID(N'dbo.SenatorAnnualLoanBudgets')
          AND name = N'IX_SenatorAnnualLoanBudgets_Created'
    )
    BEGIN
        CREATE INDEX IX_SenatorAnnualLoanBudgets_Created
            ON dbo.SenatorAnnualLoanBudgets (Created);
    END;

    IF NOT EXISTS
    (
        SELECT 1 FROM sys.indexes
        WHERE object_id = OBJECT_ID(N'dbo.SenatorAnnualLoanBudgets')
          AND name = N'IX_SenatorAnnualLoanBudgets_SenatorId_Year'
    )
    BEGIN
        CREATE UNIQUE INDEX IX_SenatorAnnualLoanBudgets_SenatorId_Year
            ON dbo.SenatorAnnualLoanBudgets (SenatorId, [Year]);
    END;

    IF NOT EXISTS
    (
        SELECT 1 FROM sys.foreign_keys
        WHERE parent_object_id = OBJECT_ID(N'dbo.SenatorAnnualLoanBudgets')
          AND name = N'FK_SenatorAnnualLoanBudgets_SenatorProfiles_SenatorId'
    )
    BEGIN
        ALTER TABLE dbo.SenatorAnnualLoanBudgets WITH CHECK
            ADD CONSTRAINT FK_SenatorAnnualLoanBudgets_SenatorProfiles_SenatorId
            FOREIGN KEY (SenatorId) REFERENCES dbo.SenatorProfiles (UserId) ON DELETE NO ACTION;
        ALTER TABLE dbo.SenatorAnnualLoanBudgets
            CHECK CONSTRAINT FK_SenatorAnnualLoanBudgets_SenatorProfiles_SenatorId;
    END;

    IF NOT EXISTS
    (
        SELECT 1 FROM dbo.__EFMigrationsHistory
        WHERE MigrationId = N'20261001090000_AddSenatorAnnualLoanBudgets'
    )
    BEGIN
        INSERT INTO dbo.__EFMigrationsHistory (MigrationId, ProductVersion)
        VALUES (N'20261001090000_AddSenatorAnnualLoanBudgets', N'7.0.5');
    END;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
