-- Create the main application database
CREATE DATABASE QuantityMeasurementDb;
Use QuantityMeasurementDb;

CREATE TABLE dbo.QuantityMeasurements (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    TimestampUtc DATETIME2(7) NOT NULL,
    OperationType NVARCHAR(100) NOT NULL,
    Details NVARCHAR(MAX) NOT NULL,
    HasError BIT NOT NULL,
    ErrorMessage NVARCHAR(MAX) NULL,
    Category INT NULL
);
GO
-- Index on OperationType for faster queries by operation
CREATE INDEX IX_QuantityMeasurements_OperationType
    ON dbo.QuantityMeasurements (OperationType);
GO

-- Index on Category for faster queries by measurement category
CREATE INDEX IX_QuantityMeasurements_Category
    ON dbo.QuantityMeasurements (Category);
GO

-- Index on TimestampUtc for history/reporting queries
CREATE INDEX IX_QuantityMeasurements_TimestampUtc
    ON dbo.QuantityMeasurements (TimestampUtc);
GO

SELECT TOP (10) *
FROM dbo.QuantityMeasurements
ORDER BY TimestampUtc DESC;
GO