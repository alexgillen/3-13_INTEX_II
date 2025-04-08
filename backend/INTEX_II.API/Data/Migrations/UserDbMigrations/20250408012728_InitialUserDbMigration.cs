using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mission11.API.Data.Migrations.UserDbMigrations
{
    /// <inheritdoc />
    public partial class InitialUserDbMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateTable(
            //     name: "Movie",
            //     columns: table => new
            //     {
            //         show_id = table.Column<string>(type: "TEXT", nullable: false),
            //         type = table.Column<string>(type: "TEXT", nullable: true),
            //         title = table.Column<string>(type: "TEXT", nullable: true),
            //         director = table.Column<string>(type: "TEXT", nullable: true),
            //         cast = table.Column<string>(type: "TEXT", nullable: true),
            //         country = table.Column<string>(type: "TEXT", nullable: true),
            //         release_year = table.Column<int>(type: "INTEGER", nullable: false),
            //         rating = table.Column<string>(type: "TEXT", nullable: true),
            //         duration = table.Column<string>(type: "TEXT", nullable: true),
            //         description = table.Column<string>(type: "TEXT", nullable: true),
            //         genres = table.Column<string>(type: "TEXT", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Movie", x => x.show_id);
            //     });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ExternalAuthId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Age = table.Column<int>(type: "INTEGER", nullable: true),
                    Gender = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Role = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRating",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ShowId = table.Column<string>(type: "TEXT", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRating_Movie_ShowId",
                        column: x => x.ShowId,
                        principalTable: "Movie",
                        principalColumn: "show_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRating_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Age", "CreatedAt", "Email", "ExternalAuthId", "FirstName", "Gender", "IsActive", "LastLogin", "LastName", "Phone", "ProfileImageUrl", "Role" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), null, new DateTime(2025, 4, 8, 1, 27, 28, 68, DateTimeKind.Utc).AddTicks(7060), "admin@cineniche.com", "admin-external-id", "Admin", null, true, null, "User", null, null, "Admin" });

            // migrationBuilder.CreateIndex(
            //     name: "IX_UserRating_ShowId",
            //     table: "UserRating",
            //     column: "ShowId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRating_UserId",
                table: "UserRating",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ExternalAuthId",
                table: "Users",
                column: "ExternalAuthId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropTable(
            //     name: "UserRating");

            migrationBuilder.DropTable(
                name: "Users");

            // migrationBuilder.DropTable(
            //     name: "Movie");
        }
    }
}
