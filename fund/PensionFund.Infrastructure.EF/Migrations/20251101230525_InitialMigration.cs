using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PensionFund.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "PensionFund");

            migrationBuilder.CreateTable(
                name: "Clients",
                schema: "PensionFund",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "PensionFund",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProductType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sites",
                schema: "PensionFund",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inscriptions",
                schema: "PensionFund",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscriptions", x => new { x.ProductId, x.ClientId });
                    table.ForeignKey(
                        name: "FK_Inscriptions_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "PensionFund",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inscriptions_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "PensionFund",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Availabilities",
                schema: "PensionFund",
                columns: table => new
                {
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Availabilities", x => new { x.ProductId, x.SiteId });
                    table.ForeignKey(
                        name: "FK_Availabilities_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "PensionFund",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Availabilities_Sites_SiteId",
                        column: x => x.SiteId,
                        principalSchema: "PensionFund",
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Visits",
                schema: "PensionFund",
                columns: table => new
                {
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    VisitDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visits", x => new { x.SiteId, x.ClientId });
                    table.ForeignKey(
                        name: "FK_Visits_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "PensionFund",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visits_Sites_SiteId",
                        column: x => x.SiteId,
                        principalSchema: "PensionFund",
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Availabilities_SiteId",
                schema: "PensionFund",
                table: "Availabilities",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_ClientId",
                schema: "PensionFund",
                table: "Inscriptions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_ClientId",
                schema: "PensionFund",
                table: "Visits",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Availabilities",
                schema: "PensionFund");

            migrationBuilder.DropTable(
                name: "Inscriptions",
                schema: "PensionFund");

            migrationBuilder.DropTable(
                name: "Visits",
                schema: "PensionFund");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "PensionFund");

            migrationBuilder.DropTable(
                name: "Clients",
                schema: "PensionFund");

            migrationBuilder.DropTable(
                name: "Sites",
                schema: "PensionFund");
        }
    }
}
