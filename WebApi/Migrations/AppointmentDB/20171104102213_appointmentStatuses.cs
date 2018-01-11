using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApi.Migrations.AppointmentDB
{
    public partial class appointmentStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Benches_BenchId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_BenchId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "ClientID",
                table: "Appointments",
                newName: "ClientId");

            migrationBuilder.AlterColumn<int>(
                name: "BenchId",
                table: "Appointments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppointmentStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentStatuses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentStatuses");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Appointments",
                newName: "ClientID");

            migrationBuilder.AlterColumn<int>(
                name: "BenchId",
                table: "Appointments",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "Appointments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_BenchId",
                table: "Appointments",
                column: "BenchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Benches_BenchId",
                table: "Appointments",
                column: "BenchId",
                principalTable: "Benches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
