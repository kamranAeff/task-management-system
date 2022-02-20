using Microsoft.EntityFrameworkCore.Migrations;

namespace DockerizeTaskManagementApi.Migrations
{
    public partial class TaskBoardDescriptionCreateColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Boards",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Boards");
        }
    }
}
