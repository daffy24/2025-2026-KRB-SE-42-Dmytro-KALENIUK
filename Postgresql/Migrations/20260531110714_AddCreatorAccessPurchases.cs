using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatorAccessPurchases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "creator_access_purchases",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    timespan = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_creator_access_purchases", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "creator_access_purchases");
        }
    }
}
