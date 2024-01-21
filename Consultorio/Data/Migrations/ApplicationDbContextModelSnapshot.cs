﻿// <auto-generated />
using System;
using Consultorio.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Consultorio.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Consultorio.Models.DiaHorario", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Dia")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Disponible")
                        .HasColumnType("bit");

                    b.Property<short>("HorarioID")
                        .HasColumnType("smallint");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("HorarioID");

                    b.ToTable("DiaHorario");
                });

            modelBuilder.Entity("Consultorio.Models.HistoriaClinica", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(1500)
                        .HasColumnType("nvarchar(1500)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<long>("PacienteID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("PacienteID");

                    b.ToTable("HistoriaClinica");
                });

            modelBuilder.Entity("Consultorio.Models.Horario", b =>
                {
                    b.Property<short>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<short>("ID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<TimeOnly>("Hora")
                        .HasColumnType("time");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.ToTable("Horario");

                    b.HasData(
                        new
                        {
                            ID = (short)1,
                            CreatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6038),
                            Hora = new TimeOnly(9, 0, 0),
                            UpdatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6041)
                        },
                        new
                        {
                            ID = (short)2,
                            CreatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6139),
                            Hora = new TimeOnly(9, 20, 0),
                            UpdatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6139)
                        },
                        new
                        {
                            ID = (short)3,
                            CreatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6142),
                            Hora = new TimeOnly(9, 40, 0),
                            UpdatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6143)
                        },
                        new
                        {
                            ID = (short)4,
                            CreatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6145),
                            Hora = new TimeOnly(10, 0, 0),
                            UpdatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6145)
                        },
                        new
                        {
                            ID = (short)5,
                            CreatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6147),
                            Hora = new TimeOnly(10, 20, 0),
                            UpdatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6147)
                        },
                        new
                        {
                            ID = (short)6,
                            CreatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6149),
                            Hora = new TimeOnly(10, 40, 0),
                            UpdatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6149)
                        },
                        new
                        {
                            ID = (short)7,
                            CreatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6151),
                            Hora = new TimeOnly(11, 0, 0),
                            UpdatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6151)
                        },
                        new
                        {
                            ID = (short)8,
                            CreatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6153),
                            Hora = new TimeOnly(11, 20, 0),
                            UpdatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6154)
                        },
                        new
                        {
                            ID = (short)9,
                            CreatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6156),
                            Hora = new TimeOnly(11, 40, 0),
                            UpdatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6156)
                        },
                        new
                        {
                            ID = (short)10,
                            CreatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6158),
                            Hora = new TimeOnly(12, 0, 0),
                            UpdatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6158)
                        },
                        new
                        {
                            ID = (short)11,
                            CreatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6160),
                            Hora = new TimeOnly(12, 20, 0),
                            UpdatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6160)
                        },
                        new
                        {
                            ID = (short)12,
                            CreatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6162),
                            Hora = new TimeOnly(12, 40, 0),
                            UpdatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6162)
                        },
                        new
                        {
                            ID = (short)13,
                            CreatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6164),
                            Hora = new TimeOnly(13, 0, 0),
                            UpdatedAt = new DateTime(2024, 1, 21, 16, 32, 41, 394, DateTimeKind.Utc).AddTicks(6164)
                        });
                });

            modelBuilder.Entity("Consultorio.Models.ObraSocial", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Habilitada")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.ToTable("ObraSocial");
                });

            modelBuilder.Entity("Consultorio.Models.Paciente", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Localidad")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<long>("ObraSocialID")
                        .HasColumnType("bigint");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("ObraSocialID");

                    b.ToTable("Paciente");
                });

            modelBuilder.Entity("Consultorio.Models.Persona", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<long>("ObraSocialID")
                        .HasColumnType("bigint");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("ObraSocialID");

                    b.ToTable("Persona");
                });

            modelBuilder.Entity("Consultorio.Models.Turno", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("DiaHorarioID")
                        .HasColumnType("bigint");

                    b.Property<long>("PersonaID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("DiaHorarioID");

                    b.HasIndex("PersonaID");

                    b.ToTable("Turno");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Consultorio.Models.ApplicationUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.HasDiscriminator().HasValue("ApplicationUser");
                });

            modelBuilder.Entity("Consultorio.Models.DiaHorario", b =>
                {
                    b.HasOne("Consultorio.Models.Horario", "Horario")
                        .WithMany()
                        .HasForeignKey("HorarioID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Horario");
                });

            modelBuilder.Entity("Consultorio.Models.HistoriaClinica", b =>
                {
                    b.HasOne("Consultorio.Models.Paciente", "Paciente")
                        .WithMany("HistoriasClinicas")
                        .HasForeignKey("PacienteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Paciente");
                });

            modelBuilder.Entity("Consultorio.Models.Paciente", b =>
                {
                    b.HasOne("Consultorio.Models.ObraSocial", "ObraSocial")
                        .WithMany("Pacientes")
                        .HasForeignKey("ObraSocialID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ObraSocial");
                });

            modelBuilder.Entity("Consultorio.Models.Persona", b =>
                {
                    b.HasOne("Consultorio.Models.ObraSocial", "ObraSocial")
                        .WithMany("Personas")
                        .HasForeignKey("ObraSocialID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ObraSocial");
                });

            modelBuilder.Entity("Consultorio.Models.Turno", b =>
                {
                    b.HasOne("Consultorio.Models.DiaHorario", "DiaHorario")
                        .WithMany()
                        .HasForeignKey("DiaHorarioID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Consultorio.Models.Persona", "Persona")
                        .WithMany()
                        .HasForeignKey("PersonaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiaHorario");

                    b.Navigation("Persona");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Consultorio.Models.ObraSocial", b =>
                {
                    b.Navigation("Pacientes");

                    b.Navigation("Personas");
                });

            modelBuilder.Entity("Consultorio.Models.Paciente", b =>
                {
                    b.Navigation("HistoriasClinicas");
                });
#pragma warning restore 612, 618
        }
    }
}
