using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chroma.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Palettes",
                columns: table => new
                {
                    PaletteId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Palettes", x => x.PaletteId);
                });

            migrationBuilder.CreateTable(
                name: "PaletteColors",
                columns: table => new
                {
                    PaletteColorId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    R = table.Column<int>(type: "int", nullable: false),
                    G = table.Column<int>(type: "int", nullable: false),
                    B = table.Column<int>(type: "int", nullable: false),
                    A = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaletteId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaletteColors", x => x.PaletteColorId);
                    table.ForeignKey(
                        name: "FK_PaletteColors_Palettes_PaletteId",
                        column: x => x.PaletteId,
                        principalTable: "Palettes",
                        principalColumn: "PaletteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaletteColors_PaletteId_R_G_B_A",
                table: "PaletteColors",
                columns: new[] { "PaletteId", "R", "G", "B", "A" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaletteColors");

            migrationBuilder.DropTable(
                name: "Palettes");
        }
    }
}
