using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApi.Migrations.AppointmentDB
{
    public partial class addedHealthworkerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HealthworkerName",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "HealthworkerId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HealthworkerId",
                table: "Appointments");

            migrationBuilder.AddColumn<string>(
                name: "HealthworkerName",
                table: "Appointments",
                nullable: true);
        }
    }
}
