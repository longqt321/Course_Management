using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace asp_project.Migrations
{
    /// <inheritdoc />
    public partial class RenameTableStudents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(name: "Students", newName: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(name: "Users", newName: "Students");

        }
    }
}
