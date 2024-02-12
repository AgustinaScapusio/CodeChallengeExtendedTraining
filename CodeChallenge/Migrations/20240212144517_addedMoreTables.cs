using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeChallenge.Migrations
{
    /// <inheritdoc />
    public partial class addedMoreTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Teacher",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CheckpointId",
                table: "Module",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Checkpoint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checkpoint", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CheckpointStudent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    CheckpointID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckpointStudent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckpointStudent_Checkpoint_CheckpointID",
                        column: x => x.CheckpointID,
                        principalTable: "Checkpoint",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckpointStudent_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Module_CheckpointId",
                table: "Module",
                column: "CheckpointId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckpointStudent_CheckpointID",
                table: "CheckpointStudent",
                column: "CheckpointID");

            migrationBuilder.CreateIndex(
                name: "IX_CheckpointStudent_StudentID",
                table: "CheckpointStudent",
                column: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Checkpoint_CheckpointId",
                table: "Module",
                column: "CheckpointId",
                principalTable: "Checkpoint",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Module_Checkpoint_CheckpointId",
                table: "Module");

            migrationBuilder.DropTable(
                name: "CheckpointStudent");

            migrationBuilder.DropTable(
                name: "Checkpoint");

            migrationBuilder.DropIndex(
                name: "IX_Module_CheckpointId",
                table: "Module");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Teacher");

            migrationBuilder.DropColumn(
                name: "CheckpointId",
                table: "Module");
        }
    }
}
