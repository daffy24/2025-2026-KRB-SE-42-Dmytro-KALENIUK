using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class AddCreators : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "creators",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    bio = table.Column<string>(type: "text", nullable: false),
                    experience_years = table.Column<int>(type: "integer", nullable: false),
                    areas_of_expertise = table.Column<string[]>(type: "text[]", nullable: false),
                    languages = table.Column<string[]>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_creators", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "creators");
        }
    }
}
