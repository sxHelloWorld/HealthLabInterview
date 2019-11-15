using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AddTestData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO Forecasts (Date, TemperatureC, Zipcode)
                VALUES(DATE('now'), 22, '14020'); 
            ");

            migrationBuilder.Sql(@"
                INSERT INTO Forecasts (Date, TemperatureC, Zipcode)
                VALUES(DATE('now'), 24, '14586'); 
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM Forecasts
                WHERE Zipcode = '14020';
            ");

            migrationBuilder.Sql(@"
                DELETE FROM Forecasts
                WHERE Zipcode = '14586';
            ");
        }
    }
}
