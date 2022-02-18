using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DockerizeTaskManagementApi.Migrations
{
    public partial class TaskEntitiesAddCreatedDateColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Boards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Boards");
        }
    }
}
