using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryService.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "passport_datas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    series = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    number = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    issued_by = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    issued_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    birth_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    birth_place = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    registration_address = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_passport_datas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "receivers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    passport_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receivers", x => x.id);
                    table.ForeignKey(
                        name: "FK_receivers_passport_datas_passport_id",
                        column: x => x.passport_id,
                        principalTable: "passport_datas",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "senders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    passport_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_senders", x => x.id);
                    table.ForeignKey(
                        name: "FK_senders_passport_datas_passport_id",
                        column: x => x.passport_id,
                        principalTable: "passport_datas",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "deliveries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sender_id = table.Column<Guid>(type: "uuid", nullable: false),
                    receiver_id = table.Column<Guid>(type: "uuid", nullable: false),
                    pickup_address = table.Column<string>(type: "text", nullable: false),
                    delivery_address = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    payment_method = table.Column<string>(type: "text", nullable: false),
                    receiver_passport_shown = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deliveries", x => x.id);
                    table.ForeignKey(
                        name: "FK_deliveries_receivers_receiver_id",
                        column: x => x.receiver_id,
                        principalTable: "receivers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_deliveries_senders_sender_id",
                        column: x => x.sender_id,
                        principalTable: "senders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "delivery_notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    delivery_id = table.Column<Guid>(type: "uuid", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    sent_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_notifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_delivery_notifications_deliveries_delivery_id",
                        column: x => x.delivery_id,
                        principalTable: "deliveries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_deliveries_receiver_id",
                table: "deliveries",
                column: "receiver_id");

            migrationBuilder.CreateIndex(
                name: "IX_deliveries_sender_id",
                table: "deliveries",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_delivery_notifications_delivery_id",
                table: "delivery_notifications",
                column: "delivery_id");

            migrationBuilder.CreateIndex(
                name: "IX_receivers_passport_id",
                table: "receivers",
                column: "passport_id");

            migrationBuilder.CreateIndex(
                name: "IX_senders_passport_id",
                table: "senders",
                column: "passport_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "delivery_notifications");

            migrationBuilder.DropTable(
                name: "deliveries");

            migrationBuilder.DropTable(
                name: "receivers");

            migrationBuilder.DropTable(
                name: "senders");

            migrationBuilder.DropTable(
                name: "passport_datas");
        }
    }
}
