using Microsoft.EntityFrameworkCore.Migrations;

namespace ContactBookAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Tags = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Uploaded = table.Column<string>(nullable: true),
                    Width = table.Column<string>(nullable: true),
                    Height = table.Column<string>(nullable: true),
                    MobilePhone = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactItem", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactItem");
        }
    }
}
