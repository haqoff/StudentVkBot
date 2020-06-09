using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentBotCore.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VkChats",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TimeOffset = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VkChats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VkUsers",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VkUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChatId = table.Column<ulong>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_VkChats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "VkChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EveryDayRegularEventNotifications",
                columns: table => new
                {
                    ChatId = table.Column<ulong>(nullable: false),
                    StartUtcTime = table.Column<TimeSpan>(nullable: false),
                    Scope = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EveryDayRegularEventNotifications", x => new { x.ChatId, x.StartUtcTime });
                    table.ForeignKey(
                        name: "FK_EveryDayRegularEventNotifications_VkChats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "VkChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EveryDayRegularSchedules",
                columns: table => new
                {
                    ChatId = table.Column<ulong>(nullable: false),
                    StartUtcTime = table.Column<TimeSpan>(nullable: false),
                    Duration = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EveryDayRegularSchedules", x => new { x.ChatId, x.StartUtcTime, x.Duration });
                    table.ForeignKey(
                        name: "FK_EveryDayRegularSchedules_VkChats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "VkChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(maxLength: 32, nullable: false),
                    LastName = table.Column<string>(maxLength: 32, nullable: false),
                    Patronymic = table.Column<string>(maxLength: 32, nullable: true),
                    Email = table.Column<string>(maxLength: 320, nullable: true),
                    VkUserId = table.Column<ulong>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_VkUsers_VkUserId",
                        column: x => x.VkUserId,
                        principalTable: "VkUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VkChatAdmins",
                columns: table => new
                {
                    ChatId = table.Column<ulong>(nullable: false),
                    VkUserId = table.Column<ulong>(nullable: false),
                    IsSuperAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VkChatAdmins", x => new { x.ChatId, x.VkUserId });
                    table.ForeignKey(
                        name: "FK_VkChatAdmins_VkChats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "VkChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VkChatAdmins_VkUsers_VkUserId",
                        column: x => x.VkUserId,
                        principalTable: "VkUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<ulong>(nullable: false),
                    TagId = table.Column<ulong>(nullable: true),
                    Description = table.Column<string>(maxLength: 300, nullable: true),
                    Location = table.Column<string>(maxLength: 64, nullable: true),
                    StartUtcDateTime = table.Column<DateTime>(nullable: true),
                    Duration = table.Column<TimeSpan>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventOrganizers",
                columns: table => new
                {
                    EventId = table.Column<ulong>(nullable: false),
                    PersonId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventOrganizers", x => new { x.EventId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_EventOrganizers_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventOrganizers_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventParticipants",
                columns: table => new
                {
                    EventId = table.Column<ulong>(nullable: false),
                    PersonId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventParticipants", x => new { x.EventId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_EventParticipants_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventParticipants_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1ul, "Лекция" },
                    { 2ul, "Практика" },
                    { 3ul, "Лабораторная" },
                    { 4ul, "Факультатив" },
                    { 5ul, "Урок" },
                    { 6ul, "Экзамен" },
                    { 7ul, "Консультация" },
                    { 8ul, "Другое" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ChatId",
                table: "Categories",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_EventOrganizers_PersonId",
                table: "EventOrganizers",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipants_PersonId",
                table: "EventParticipants",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CategoryId",
                table: "Events",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_TagId",
                table: "Events",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_VkUserId",
                table: "Persons",
                column: "VkUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VkChatAdmins_VkUserId",
                table: "VkChatAdmins",
                column: "VkUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventOrganizers");

            migrationBuilder.DropTable(
                name: "EventParticipants");

            migrationBuilder.DropTable(
                name: "EveryDayRegularEventNotifications");

            migrationBuilder.DropTable(
                name: "EveryDayRegularSchedules");

            migrationBuilder.DropTable(
                name: "VkChatAdmins");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "VkUsers");

            migrationBuilder.DropTable(
                name: "VkChats");
        }
    }
}
