using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Consultorio.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Horario",
                columns: table => new
                {
                    ID = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hora = table.Column<TimeOnly>(type: "time", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horario", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ObraSocial",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Habilitada = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObraSocial", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiaHorario",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HorarioID = table.Column<short>(type: "smallint", nullable: false),
                    Dia = table.Column<DateTime>(type: "date", nullable: false),
                    Disponible = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaHorario", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DiaHorario_Horario_HorarioID",
                        column: x => x.HorarioID,
                        principalTable: "Horario",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Paciente",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Apellido = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObraSocialID = table.Column<long>(type: "bigint", nullable: true),
                    FechaNacimiento = table.Column<DateTime>(type: "date", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Localidad = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paciente", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Paciente_ObraSocial_ObraSocialID",
                        column: x => x.ObraSocialID,
                        principalTable: "ObraSocial",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Persona",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObraSocialID = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persona", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Persona_ObraSocial_ObraSocialID",
                        column: x => x.ObraSocialID,
                        principalTable: "ObraSocial",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoriaClinica",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PacienteID = table.Column<long>(type: "bigint", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoriaClinica", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HistoriaClinica_Paciente_PacienteID",
                        column: x => x.PacienteID,
                        principalTable: "Paciente",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Turno",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonaID = table.Column<long>(type: "bigint", nullable: false),
                    DiaHorarioID = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turno", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Turno_DiaHorario_DiaHorarioID",
                        column: x => x.DiaHorarioID,
                        principalTable: "DiaHorario",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Turno_Persona_PersonaID",
                        column: x => x.PersonaID,
                        principalTable: "Persona",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Horario",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Hora", "UpdatedAt" },
                values: new object[,]
                {
                    { (short)1, new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(89), null, new TimeOnly(9, 0, 0), new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(93) },
                    { (short)2, new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(192), null, new TimeOnly(9, 20, 0), new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(192) },
                    { (short)3, new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(196), null, new TimeOnly(9, 40, 0), new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(196) },
                    { (short)4, new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(227), null, new TimeOnly(10, 0, 0), new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(227) },
                    { (short)5, new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(230), null, new TimeOnly(10, 20, 0), new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(230) },
                    { (short)6, new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(232), null, new TimeOnly(10, 40, 0), new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(232) },
                    { (short)7, new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(234), null, new TimeOnly(11, 0, 0), new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(234) },
                    { (short)8, new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(236), null, new TimeOnly(11, 20, 0), new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(236) },
                    { (short)9, new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(238), null, new TimeOnly(11, 40, 0), new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(238) },
                    { (short)10, new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(241), null, new TimeOnly(12, 0, 0), new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(241) },
                    { (short)11, new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(243), null, new TimeOnly(12, 20, 0), new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(243) },
                    { (short)12, new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(245), null, new TimeOnly(12, 40, 0), new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(245) },
                    { (short)13, new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(247), null, new TimeOnly(13, 0, 0), new DateTime(2024, 1, 23, 23, 38, 42, 699, DateTimeKind.Utc).AddTicks(247) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DiaHorario_HorarioID",
                table: "DiaHorario",
                column: "HorarioID");

            migrationBuilder.CreateIndex(
                name: "IX_HistoriaClinica_PacienteID",
                table: "HistoriaClinica",
                column: "PacienteID");

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_ObraSocialID",
                table: "Paciente",
                column: "ObraSocialID");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_ObraSocialID",
                table: "Persona",
                column: "ObraSocialID");

            migrationBuilder.CreateIndex(
                name: "IX_Turno_DiaHorarioID",
                table: "Turno",
                column: "DiaHorarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Turno_PersonaID",
                table: "Turno",
                column: "PersonaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "HistoriaClinica");

            migrationBuilder.DropTable(
                name: "Turno");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Paciente");

            migrationBuilder.DropTable(
                name: "DiaHorario");

            migrationBuilder.DropTable(
                name: "Persona");

            migrationBuilder.DropTable(
                name: "Horario");

            migrationBuilder.DropTable(
                name: "ObraSocial");
        }
    }
}
