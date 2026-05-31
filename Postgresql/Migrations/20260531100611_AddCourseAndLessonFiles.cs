using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseAndLessonFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "course_id",
                table: "lessons",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "media_type",
                table: "lessons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "text",
                table: "lessons",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "video_content_type",
                table: "lessons",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "video_path",
                table: "lessons",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "preview_image_content_type",
                table: "courses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "preview_image_path",
                table: "courses",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_lessons_course_id",
                table: "lessons",
                column: "course_id");

            migrationBuilder.AddForeignKey(
                name: "FK_lessons_courses_course_id",
                table: "lessons",
                column: "course_id",
                principalTable: "courses",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lessons_courses_course_id",
                table: "lessons");

            migrationBuilder.DropIndex(
                name: "IX_lessons_course_id",
                table: "lessons");

            migrationBuilder.DropColumn(
                name: "course_id",
                table: "lessons");

            migrationBuilder.DropColumn(
                name: "media_type",
                table: "lessons");

            migrationBuilder.DropColumn(
                name: "text",
                table: "lessons");

            migrationBuilder.DropColumn(
                name: "video_content_type",
                table: "lessons");

            migrationBuilder.DropColumn(
                name: "video_path",
                table: "lessons");

            migrationBuilder.DropColumn(
                name: "preview_image_content_type",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "preview_image_path",
                table: "courses");
        }
    }
}
