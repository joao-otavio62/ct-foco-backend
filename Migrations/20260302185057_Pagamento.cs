using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ct_foco_backend.Migrations
{
    /// <inheritdoc />
    public partial class Pagamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pagamento",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pagamento",
                table: "Members");
        }
    }
}
