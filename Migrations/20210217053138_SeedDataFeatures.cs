using Microsoft.EntityFrameworkCore.Migrations;

namespace vega.Migrations
{
    public partial class SeedDataFeatures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Features (Name) VALUES ('ABS')");
            migrationBuilder.Sql("INSERT INTO Features (Name) VALUES ('Sunroof')");
            migrationBuilder.Sql("INSERT INTO Features (Name) VALUES ('Stereo System')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Features");
        }
    }
}