using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace dotnet_rpg.Migrations
{
    public partial class finalSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "PasswordSalt", "Username" },
                values: new object[] { 1, new byte[] { 212, 66, 223, 151, 7, 54, 130, 255, 54, 217, 227, 254, 239, 106, 53, 147, 45, 181, 176, 214, 85, 65, 9, 255, 21, 183, 232, 40, 82, 123, 169, 220, 151, 52, 187, 103, 136, 134, 101, 194, 203, 57, 108, 193, 65, 59, 217, 239, 117, 41, 3, 193, 85, 201, 101, 196, 138, 1, 124, 167, 30, 57, 131, 191 }, new byte[] { 46, 164, 72, 126, 53, 124, 5, 42, 19, 58, 202, 196, 226, 19, 60, 26, 29, 16, 84, 229, 106, 221, 194, 145, 100, 198, 78, 150, 78, 16, 150, 215, 254, 33, 56, 246, 46, 137, 224, 69, 232, 249, 16, 120, 213, 255, 178, 136, 224, 214, 107, 223, 219, 109, 43, 95, 89, 108, 40, 31, 95, 70, 185, 134, 29, 246, 57, 27, 175, 150, 217, 41, 204, 139, 240, 124, 115, 165, 101, 77, 124, 106, 190, 186, 39, 114, 86, 28, 170, 205, 179, 145, 82, 18, 140, 140, 12, 222, 239, 180, 133, 165, 149, 27, 178, 246, 56, 112, 214, 183, 66, 99, 246, 154, 40, 109, 198, 207, 44, 99, 93, 0, 40, 13, 222, 11, 105, 141 }, "user1" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "PasswordSalt", "Username" },
                values: new object[] { 2, new byte[] { 196, 190, 237, 77, 212, 20, 184, 184, 115, 65, 158, 213, 56, 27, 11, 184, 218, 192, 252, 76, 31, 76, 250, 252, 91, 95, 159, 202, 212, 249, 149, 140, 149, 17, 93, 14, 131, 211, 213, 178, 59, 52, 136, 101, 112, 26, 155, 229, 83, 214, 250, 200, 83, 142, 195, 16, 226, 10, 240, 143, 87, 188, 238, 220 }, new byte[] { 194, 27, 88, 79, 118, 112, 252, 118, 248, 29, 54, 243, 164, 136, 219, 149, 108, 123, 46, 128, 231, 61, 128, 186, 161, 124, 60, 81, 33, 23, 3, 175, 172, 121, 107, 202, 249, 26, 39, 27, 239, 155, 23, 83, 131, 249, 31, 231, 153, 130, 13, 250, 116, 200, 14, 253, 146, 132, 189, 16, 205, 9, 254, 19, 101, 171, 40, 60, 101, 221, 5, 90, 138, 5, 20, 207, 228, 174, 101, 72, 214, 27, 5, 26, 61, 170, 37, 225, 217, 122, 162, 6, 82, 207, 188, 148, 0, 110, 23, 150, 25, 68, 110, 28, 84, 178, 19, 80, 68, 6, 69, 177, 180, 229, 177, 138, 152, 64, 197, 76, 99, 168, 251, 76, 153, 30, 195, 98 }, "user2" });

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "Id", "Class", "Defeats", "Defence", "Fights", "HitPoints", "Intelligence", "Name", "Strength", "UserId", "Victories" },
                values: new object[] { 1, 1, 0, 10, 0, 100, 10, "Frodo", 15, 1, 0 });

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "Id", "Class", "Defeats", "Defence", "Fights", "HitPoints", "Intelligence", "Name", "Strength", "UserId", "Victories" },
                values: new object[] { 2, 2, 0, 5, 0, 100, 20, "Raistlin", 5, 2, 0 });

            migrationBuilder.InsertData(
                table: "CharacterSkills",
                columns: new[] { "CharacterId", "SkillId" },
                values: new object[] { 1, 2 });

            migrationBuilder.InsertData(
                table: "CharacterSkills",
                columns: new[] { "CharacterId", "SkillId" },
                values: new object[] { 2, 1 });

            migrationBuilder.InsertData(
                table: "CharacterSkills",
                columns: new[] { "CharacterId", "SkillId" },
                values: new object[] { 2, 3 });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Id", "CharacterId", "Damage", "Name" },
                values: new object[] { 1, 1, 20, "The Master Sword" });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Id", "CharacterId", "Damage", "Name" },
                values: new object[] { 2, 2, 5, "Crystal Wand" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CharacterSkills",
                keyColumns: new[] { "CharacterId", "SkillId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "CharacterSkills",
                keyColumns: new[] { "CharacterId", "SkillId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "CharacterSkills",
                keyColumns: new[] { "CharacterId", "SkillId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "Weapons",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Weapons",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
