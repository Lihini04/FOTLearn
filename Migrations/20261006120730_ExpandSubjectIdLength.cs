using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElearningQuiz.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExpandSubjectIdLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Subjects",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_LecturerSubjects_Subjects",
                table: "LecturerSubjects");

            migrationBuilder.AlterColumn<string>(
                name: "Subject_id",
                table: "Subjects",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(7)",
                oldMaxLength: 7)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Subject_id",
                table: "Quizzes",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(7)",
                oldMaxLength: 7)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedForBatchId",
                table: "Quizzes",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(7)",
                oldMaxLength: 7,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Subject_id",
                table: "LecturerSubjects",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(7)",
                oldMaxLength: 7)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Subjects",
                table: "Quizzes",
                column: "Subject_id",
                principalTable: "Subjects",
                principalColumn: "Subject_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LecturerSubjects_Subjects",
                table: "LecturerSubjects",
                column: "Subject_id",
                principalTable: "Subjects",
                principalColumn: "Subject_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Subjects",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_LecturerSubjects_Subjects",
                table: "LecturerSubjects");

            migrationBuilder.AlterColumn<string>(
                name: "Subject_id",
                table: "Subjects",
                type: "varchar(7)",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Subject_id",
                table: "Quizzes",
                type: "varchar(7)",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedForBatchId",
                table: "Quizzes",
                type: "varchar(7)",
                maxLength: 7,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Subject_id",
                table: "LecturerSubjects",
                type: "varchar(7)",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Subjects",
                table: "Quizzes",
                column: "Subject_id",
                principalTable: "Subjects",
                principalColumn: "Subject_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LecturerSubjects_Subjects",
                table: "LecturerSubjects",
                column: "Subject_id",
                principalTable: "Subjects",
                principalColumn: "Subject_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
