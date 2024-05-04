using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Practice_01.Migrations
{
    /// <inheritdoc />
    public partial class first4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prices",
                table: "Images");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Prices",
                table: "Images",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
