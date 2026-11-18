using ElearningQuiz.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElearningQuiz.Infrastructure.Migrations
{
    [DbContext(typeof(ElearningQuizDbContext))]
    [Migration("20261010000000_ExpandQuizForeignKeyLength")]
    public partial class ExpandQuizForeignKeyLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizzes",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_Quizzes",
                table: "QuizAttempts");

            migrationBuilder.AlterColumn<string>(
                name: "Quiz_id",
                table: "Questions",
                type: "varchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(7)",
                oldMaxLength: 7)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Quiz_id",
                table: "QuizAttempts",
                type: "varchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(7)",
                oldMaxLength: 7)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quizzes",
                table: "Questions",
                column: "Quiz_id",
                principalTable: "Quizzes",
                principalColumn: "Quiz_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_Quizzes",
                table: "QuizAttempts",
                column: "Quiz_id",
                principalTable: "Quizzes",
                principalColumn: "Quiz_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizzes",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_Quizzes",
                table: "QuizAttempts");

            migrationBuilder.AlterColumn<string>(
                name: "Quiz_id",
                table: "Questions",
                type: "varchar(7)",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldMaxLength: 15)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Quiz_id",
                table: "QuizAttempts",
                type: "varchar(7)",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldMaxLength: 15)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quizzes",
                table: "Questions",
                column: "Quiz_id",
                principalTable: "Quizzes",
                principalColumn: "Quiz_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_Quizzes",
                table: "QuizAttempts",
                column: "Quiz_id",
                principalTable: "Quizzes",
                principalColumn: "Quiz_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
