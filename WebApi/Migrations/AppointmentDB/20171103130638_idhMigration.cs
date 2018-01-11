using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApi.Migrations.AppointmentDB
{
    public partial class idhMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Benches_Appointments_Guid",
                table: "Benches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Benches",
                table: "Benches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Benches");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Benches",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "BenchId",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Benches",
                table: "Benches",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "Id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Benches_BenchId",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Benches",
                table: "Benches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_BenchId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Benches");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "BenchId",
                table: "Appointments");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Benches",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Appointments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Benches",
                table: "Benches",
                column: "Guid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "Guid");

            migrationBuilder.AddForeignKey(
                name: "FK_Benches_Appointments_Guid",
                table: "Benches",
                column: "Guid",
                principalTable: "Appointments",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
