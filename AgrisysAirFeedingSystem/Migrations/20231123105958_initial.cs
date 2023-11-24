using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgrisysAirFeedingSystem.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    AreaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.AreaId);
                });

            migrationBuilder.CreateTable(
                name: "Farms",
                columns: table => new
                {
                    FarmId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Owner = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farms", x => x.FarmId);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GroupType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "Silos",
                columns: table => new
                {
                    SiloId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Capacity = table.Column<int>(type: "INTEGER", nullable: false),
                    AlarmMin = table.Column<int>(type: "INTEGER", nullable: false),
                    AfterRun = table.Column<int>(type: "INTEGER", nullable: false),
                    TransferSpeed = table.Column<int>(type: "INTEGER", nullable: false),
                    MixingTime = table.Column<int>(type: "INTEGER", nullable: false),
                    AlternativeSiloId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Silos", x => x.SiloId);
                    table.ForeignKey(
                        name: "FK_Silos_Silos_AlternativeSiloId",
                        column: x => x.AlternativeSiloId,
                        principalTable: "Silos",
                        principalColumn: "SiloId");
                });

            migrationBuilder.CreateTable(
                name: "FeedingTimes",
                columns: table => new
                {
                    FeedingTimeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AreaID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedingTimes", x => x.FeedingTimeId);
                    table.ForeignKey(
                        name: "FK_FeedingTimes_Areas_AreaID",
                        column: x => x.AreaID,
                        principalTable: "Areas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Entities",
                columns: table => new
                {
                    EntityId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityType = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entities", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_Entities_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mixtures",
                columns: table => new
                {
                    MixtureId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstSiloId = table.Column<int>(type: "INTEGER", nullable: false),
                    SecondSiloId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mixtures", x => x.MixtureId);
                    table.ForeignKey(
                        name: "FK_Mixtures_Silos_FirstSiloId",
                        column: x => x.FirstSiloId,
                        principalTable: "Silos",
                        principalColumn: "SiloId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mixtures_Silos_SecondSiloId",
                        column: x => x.SecondSiloId,
                        principalTable: "Silos",
                        principalColumn: "SiloId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventDesc = table.Column<string>(type: "TEXT", nullable: false),
                    EditLevel = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    EntityId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Events_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    SensorId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntityId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.SensorId);
                    table.ForeignKey(
                        name: "FK_Sensors_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixtureSetpoints",
                columns: table => new
                {
                    MixtureSetpointId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    age = table.Column<int>(type: "INTEGER", nullable: false),
                    SetPoint = table.Column<int>(type: "INTEGER", nullable: false),
                    MixtureId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixtureSetpoints", x => x.MixtureSetpointId);
                    table.ForeignKey(
                        name: "FK_MixtureSetpoints_Mixtures_MixtureId",
                        column: x => x.MixtureId,
                        principalTable: "Mixtures",
                        principalColumn: "MixtureId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Target",
                columns: table => new
                {
                    TargetId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PressureAlarm = table.Column<int>(type: "INTEGER", nullable: false),
                    BlowerStrength = table.Column<int>(type: "INTEGER", nullable: false),
                    Cellesluse = table.Column<int>(type: "INTEGER", nullable: false),
                    BeforeOpen = table.Column<int>(type: "INTEGER", nullable: false),
                    BeforeClose = table.Column<int>(type: "INTEGER", nullable: false),
                    pigCount = table.Column<int>(type: "INTEGER", nullable: false),
                    pigAge = table.Column<int>(type: "INTEGER", nullable: false),
                    MixtureId = table.Column<int>(type: "INTEGER", nullable: false),
                    AreaID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Target", x => x.TargetId);
                    table.ForeignKey(
                        name: "FK_Target_Areas_AreaID",
                        column: x => x.AreaID,
                        principalTable: "Areas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Target_Mixtures_MixtureId",
                        column: x => x.MixtureId,
                        principalTable: "Mixtures",
                        principalColumn: "MixtureId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    MeasurementId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SensorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.MeasurementId);
                    table.ForeignKey(
                        name: "FK_Measurements_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "SensorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entities_GroupId",
                table: "Entities",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EntityId",
                table: "Events",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedingTimes_AreaID",
                table: "FeedingTimes",
                column: "AreaID");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_SensorId",
                table: "Measurements",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Mixtures_FirstSiloId",
                table: "Mixtures",
                column: "FirstSiloId");

            migrationBuilder.CreateIndex(
                name: "IX_Mixtures_SecondSiloId",
                table: "Mixtures",
                column: "SecondSiloId");

            migrationBuilder.CreateIndex(
                name: "IX_MixtureSetpoints_MixtureId",
                table: "MixtureSetpoints",
                column: "MixtureId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_EntityId",
                table: "Sensors",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Silos_AlternativeSiloId",
                table: "Silos",
                column: "AlternativeSiloId");

            migrationBuilder.CreateIndex(
                name: "IX_Target_AreaID",
                table: "Target",
                column: "AreaID");

            migrationBuilder.CreateIndex(
                name: "IX_Target_MixtureId",
                table: "Target",
                column: "MixtureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Farms");

            migrationBuilder.DropTable(
                name: "FeedingTimes");

            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "MixtureSetpoints");

            migrationBuilder.DropTable(
                name: "Target");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Mixtures");

            migrationBuilder.DropTable(
                name: "Entities");

            migrationBuilder.DropTable(
                name: "Silos");

            migrationBuilder.DropTable(
                name: "Groups");
        }
    }
}
