using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeChallenge.Migrations
{
    /// <inheritdoc />
    public partial class addedFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Module_Checkpoint_CheckpointId",
                table: "Module");

            migrationBuilder.DropIndex(
                name: "IX_Module_CheckpointId",
                table: "Module");

            migrationBuilder.DropColumn(
                name: "CheckpointId",
                table: "Module");

            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                table: "Checkpoint",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Checkpoint_ModuleId",
                table: "Checkpoint",
                column: "ModuleId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Checkpoint_Module_ModuleId",
                table: "Checkpoint",
                column: "ModuleId",
                principalTable: "Module",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkpoint_Module_ModuleId",
                table: "Checkpoint");

            migrationBuilder.DropIndex(
                name: "IX_Checkpoint_ModuleId",
                table: "Checkpoint");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Checkpoint");

            migrationBuilder.AddColumn<int>(
                name: "CheckpointId",
                table: "Module",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Module_CheckpointId",
                table: "Module",
                column: "CheckpointId");

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Checkpoint_CheckpointId",
                table: "Module",
                column: "CheckpointId",
                principalTable: "Checkpoint",
                principalColumn: "Id");
        }
    }
}
