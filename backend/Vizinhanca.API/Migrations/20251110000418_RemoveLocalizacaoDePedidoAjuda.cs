using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vizinhanca.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLocalizacaoDePedidoAjuda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bairro",
                table: "pedidoajuda");

            migrationBuilder.DropColumn(
                name: "cidade",
                table: "pedidoajuda");

            migrationBuilder.DropColumn(
                name: "estado",
                table: "pedidoajuda");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "bairro",
                table: "pedidoajuda",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "cidade",
                table: "pedidoajuda",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "estado",
                table: "pedidoajuda",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");
        }
    }
}
