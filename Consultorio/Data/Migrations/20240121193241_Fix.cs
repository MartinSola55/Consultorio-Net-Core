using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consultorio.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turno_Paciente_PacienteID",
                table: "Turno");

            migrationBuilder.DropIndex(
                name: "IX_Turno_PacienteID",
                table: "Turno");

            migrationBuilder.DropColumn(
                name: "PacienteID",
                table: "Turno");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PacienteID",
                table: "Turno",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Turno_PacienteID",
                table: "Turno",
                column: "PacienteID");

            migrationBuilder.AddForeignKey(
                name: "FK_Turno_Paciente_PacienteID",
                table: "Turno",
                column: "PacienteID",
                principalTable: "Paciente",
                principalColumn: "ID");
        }
    }
}
