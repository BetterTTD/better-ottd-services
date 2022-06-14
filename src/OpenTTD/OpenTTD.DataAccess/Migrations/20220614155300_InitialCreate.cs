using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenTTD.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "ServerConfiguration",
                schema: "dbo",
                columns: table => new
                {
                    ServerConfigurationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    BotName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    BotVersion = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    ServerPassword = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerConfiguration", x => x.ServerConfigurationID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServerConfiguration",
                schema: "dbo");
        }
    }
}
