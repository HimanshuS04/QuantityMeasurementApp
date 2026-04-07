using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuantityMeasurementRepository.Migrations
{
    /// <inheritdoc />
    public partial class FixOperationsLogTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Drop trigger if it exists (separate batch)
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'trg_QuantityOperations_ToLogs', N'TR') IS NOT NULL
    DROP TRIGGER trg_QuantityOperations_ToLogs;
");

            // 2) Create trigger (CREATE TRIGGER must be first in its batch)
            migrationBuilder.Sql(@"
CREATE TRIGGER trg_QuantityOperations_ToLogs
ON QuantityOperations
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO QuantityMeasurementLogs
        (Id, TimestampUtc, OperationType, Details, HasError, ErrorMessage, Category)
    SELECT
        NEWID() AS Id,
        SYSUTCDATETIME() AT TIME ZONE 'India Standard Time' AS TimestampUtc,
        i.OperationType,
        CONCAT(
            'Category=', i.Category,
            '; First=', CONVERT(NVARCHAR(50), i.FirstValue), ' ', i.FirstUnit,
            '; Second=', CONVERT(NVARCHAR(50), i.SecondValue), ' ', ISNULL(i.SecondUnit, ''),
            '; Result=', CONVERT(NVARCHAR(50), i.ResultValue), ' ', ISNULL(i.ResultUnit, '')
        ) AS Details,
        0 AS HasError,
        NULL AS ErrorMessage,
        i.Category AS Category
    FROM inserted AS i;
END;
");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'trg_QuantityOperations_ToLogs', N'TR') IS NOT NULL
    DROP TRIGGER trg_QuantityOperations_ToLogs;
");

        }
    }
}
