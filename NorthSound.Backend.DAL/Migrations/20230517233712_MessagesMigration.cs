using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NorthSound.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MessagesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Messages",
                newName: "Text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Messages",
                newName: "Message");
        }
    }
}
