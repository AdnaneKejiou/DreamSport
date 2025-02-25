using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chatEtInvitation.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "ChatAmisMessageSequence");

            migrationBuilder.CreateSequence(
                name: "TeamChatMessageSequence");

            migrationBuilder.CreateTable(
                name: "AmisChats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Member1 = table.Column<int>(type: "int", nullable: false),
                    Member2 = table.Column<int>(type: "int", nullable: false),
                    AdminId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmisChats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BloqueList",
                columns: table => new
                {
                    Bloked = table.Column<int>(type: "int", nullable: false),
                    BlokedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloqueList", x => new { x.Bloked, x.BlokedBy });
                });

            migrationBuilder.CreateTable(
                name: "MemberInvitations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Emetteur = table.Column<int>(type: "int", nullable: false),
                    Recerpteur = table.Column<int>(type: "int", nullable: false),
                    AdminId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberInvitations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamChats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    AdminId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamChats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamInvitations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Emetteur = table.Column<int>(type: "int", nullable: false),
                    Recerpteur = table.Column<int>(type: "int", nullable: false),
                    AdminId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamInvitations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatAmisMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [ChatAmisMessageSequence]"),
                    ChatAmisId = table.Column<int>(type: "int", nullable: false),
                    Emetteur = table.Column<int>(type: "int", nullable: false),
                    when = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Contenue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatAmisMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatAmisMessages_AmisChats_ChatAmisId",
                        column: x => x.ChatAmisId,
                        principalTable: "AmisChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [TeamChatMessageSequence]"),
                    TeamChatId = table.Column<int>(type: "int", nullable: false),
                    Emetteur = table.Column<int>(type: "int", nullable: false),
                    when = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Contenue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamChatMessages_TeamChats_TeamChatId",
                        column: x => x.TeamChatId,
                        principalTable: "TeamChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatAmisMessages_ChatAmisId",
                table: "ChatAmisMessages",
                column: "ChatAmisId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamChatMessages_TeamChatId",
                table: "TeamChatMessages",
                column: "TeamChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BloqueList");

            migrationBuilder.DropTable(
                name: "ChatAmisMessages");

            migrationBuilder.DropTable(
                name: "MemberInvitations");

            migrationBuilder.DropTable(
                name: "TeamChatMessages");

            migrationBuilder.DropTable(
                name: "TeamInvitations");

            migrationBuilder.DropTable(
                name: "AmisChats");

            migrationBuilder.DropTable(
                name: "TeamChats");

            migrationBuilder.DropSequence(
                name: "ChatAmisMessageSequence");

            migrationBuilder.DropSequence(
                name: "TeamChatMessageSequence");
        }
    }
}
