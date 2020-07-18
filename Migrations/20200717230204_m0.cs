using Microsoft.EntityFrameworkCore.Migrations;

namespace iris_server.Migrations
{
    public partial class m0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CarerId",
                table: "Messages",
                newName: "CarerEmail");

            migrationBuilder.AlterColumn<string>(
                name: "CarerEmail",
                table: "Messages",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_CarerEmail",
                table: "Messages",
                column: "CarerEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Carers_CarerEmail",
                table: "Messages",
                column: "CarerEmail",
                principalTable: "Carers",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Carers_CarerEmail",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_CarerEmail",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "CarerEmail",
                table: "Messages",
                newName: "CarerId");

            migrationBuilder.AlterColumn<string>(
                name: "CarerId",
                table: "Messages",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
