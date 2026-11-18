using ElearningQuiz.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElearningQuiz.Infrastructure.Migrations
{
    [DbContext(typeof(ElearningQuizDbContext))]
    [Migration("20261019000000_AddMultipleQuizBatchTargets")]
    public partial class AddMultipleQuizBatchTargets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuizBatchTargets",
                columns: table => new
                {
                    Quiz_id = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    Batch_id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizBatchTargets", x => new { x.Quiz_id, x.Batch_id });
                    table.ForeignKey(
                        name: "FK_QuizBatchTargets_Batches",
                        column: x => x.Batch_id,
                        principalTable: "Batches",
                        principalColumn: "Batch_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuizBatchTargets_Quizzes",
                        column: x => x.Quiz_id,
                        principalTable: "Quizzes",
                        principalColumn: "Quiz_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuizBatchTargets_Batch_id",
                table: "QuizBatchTargets",
                column: "Batch_id");

            migrationBuilder.Sql(
                "INSERT INTO `QuizBatchTargets` (`Quiz_id`, `Batch_id`) " +
                "SELECT `Quiz_id`, `CreatedForBatchId` FROM `Quizzes` " +
                "WHERE `CreatedForBatchId` IS NOT NULL AND `CreatedForBatchId` <> ''");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "QuizBatchTargets");
        }
    }
}
