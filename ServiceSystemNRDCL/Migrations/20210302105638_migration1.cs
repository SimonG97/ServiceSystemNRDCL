using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceSystemNRDCL.Migrations
{
    public partial class migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerCID = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    CustomerName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Phone = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerCID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
