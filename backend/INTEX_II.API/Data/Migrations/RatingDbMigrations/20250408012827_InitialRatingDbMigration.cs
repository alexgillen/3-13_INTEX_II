using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mission11.API.Data.Migrations.RatingDbMigrations
{
    /// <inheritdoc />
    public partial class InitialRatingDbMigration : Migration
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

            // migrationBuilder.CreateTable(
            //     name: "User",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "TEXT", nullable: false),
            //         FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
            //         LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
            //         Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
            //         Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            //         ExternalAuthId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
            //         ProfileImageUrl = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            //         Age = table.Column<int>(type: "INTEGER", nullable: true),
            //         Gender = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
            //         Role = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            //         IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
            //         CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
            //         LastLogin = table.Column<DateTime>(type: "TEXT", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_User", x => x.Id);
            //     });

            migrationBuilder.CreateTable(
                name: "UserRatings",
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
                    table.PrimaryKey("PK_UserRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRatings_Movie_ShowId",
                        column: x => x.ShowId,
                        principalTable: "Movie",
                        principalColumn: "show_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRatings_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRatings_ShowId",
                table: "UserRatings",
                column: "ShowId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRatings_UserId_ShowId",
                table: "UserRatings",
                columns: new[] { "UserId", "ShowId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRatings");

            // migrationBuilder.DropTable(
            //     name: "Movie");

            // migrationBuilder.DropTable(
            //     name: "User");
        }
    }
}
