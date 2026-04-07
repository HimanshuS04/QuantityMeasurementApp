using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuantityMeasurementRepository.Migrations
{
    /// <inheritdoc />
    public partial class AddPostgresOperationsLogTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          migrationBuilder.Sql(@"CREATE EXTENSION IF NOT EXISTS ""pgcrypto"";");

            migrationBuilder.Sql(@"
CREATE OR REPLACE FUNCTION log_quantity_operation()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO ""QuantityMeasurementLogs""
    (
        ""Id"",
        ""TimestampUtc"",
        ""OperationType"",
        ""Details"",
        ""HasError"",
        ""ErrorMessage"",
        ""Category""
    )
    VALUES
    (
        gen_random_uuid(),
        NOW(),
        NEW.""OperationType"",
        CONCAT(
            'UserId=', COALESCE(NEW.""UserId""::text, 'NULL'),
            '; Category=', NEW.""Category""::text,
            '; First=', NEW.""FirstValue""::text, ' ', NEW.""FirstUnit"",
            '; Second=', COALESCE(NEW.""SecondValue""::text, ''), ' ', COALESCE(NEW.""SecondUnit"", ''),
            '; Result=', COALESCE(NEW.""ResultValue""::text, ''), ' ', COALESCE(NEW.""ResultUnit"", '')
        ),
        FALSE,
        NULL,
        NEW.""Category""
    );

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;
");

            migrationBuilder.Sql(@"
DROP TRIGGER IF EXISTS trg_quantity_operations_to_logs ON ""QuantityOperations"";
");

            migrationBuilder.Sql(@"
CREATE TRIGGER trg_quantity_operations_to_logs
AFTER INSERT ON ""QuantityOperations""
FOR EACH ROW
EXECUTE FUNCTION log_quantity_operation();
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
             migrationBuilder.Sql(@"
DROP TRIGGER IF EXISTS trg_quantity_operations_to_logs ON ""QuantityOperations"";
");

            migrationBuilder.Sql(@"
DROP FUNCTION IF EXISTS log_quantity_operation();
");

        }
    }
}
