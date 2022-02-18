using Microsoft.EntityFrameworkCore.Migrations;

namespace DockerizeTaskManagementApi.Migrations
{
    public partial class OrganisationEntityCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                schema: "Membership",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Organisations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganisationId",
                schema: "Membership",
                table: "Users",
                column: "OrganisationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Organisations_OrganisationId",
                schema: "Membership",
                table: "Users",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Organisations_OrganisationId",
                schema: "Membership",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Organisations");

            migrationBuilder.DropIndex(
                name: "IX_Users_OrganisationId",
                schema: "Membership",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                schema: "Membership",
                table: "Users");
        }
    }
}
