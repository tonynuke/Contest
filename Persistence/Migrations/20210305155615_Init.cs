using System;
using System.Collections.Generic;
using Domain.Contest;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CallbackApiConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: true),
                    ConfirmationKey = table.Column<string>(type: "text", nullable: true),
                    IsConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallbackApiConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VkPostId = table.Column<long>(type: "bigint", nullable: false),
                    WinnerParticipantIds = table.Column<IList<Guid>>(type: "jsonb", nullable: true),
                    IsFinished = table.Column<bool>(type: "boolean", nullable: false),
                    Configuration = table.Column<ContestConfigurationBase>(type: "jsonb", nullable: true),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    WinnerParticipantId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContestId = table.Column<Guid>(type: "uuid", nullable: false),
                    VkUserId = table.Column<long>(type: "bigint", nullable: false),
                    MaxAttemptsCount = table.Column<int>(type: "integer", nullable: true),
                    ActualAttemptsCount = table.Column<int>(type: "integer", nullable: true),
                    LastCommentDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsWinner = table.Column<bool>(type: "boolean", nullable: false),
                    ContestBaseId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participants_Contests_ContestBaseId",
                        column: x => x.ContestBaseId,
                        principalTable: "Contests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CallbackApiConfigurations_GroupId",
                table: "CallbackApiConfigurations",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_ContestBaseId",
                table: "Participants",
                column: "ContestBaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallbackApiConfigurations");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "Contests");
        }
    }
}
