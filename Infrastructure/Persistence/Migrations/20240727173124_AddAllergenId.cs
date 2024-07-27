using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaApi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAllergenId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllergenIngredient_Allergens_AllergensNormalizedName",
                table: "AllergenIngredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Allergens",
                table: "Allergens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AllergenIngredient",
                table: "AllergenIngredient");

            migrationBuilder.DropColumn(
                name: "AllergensNormalizedName",
                table: "AllergenIngredient");

            migrationBuilder.AddColumn<Guid>(
                name: "_id",
                table: "Allergens",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Allergens_id",
                table: "AllergenIngredient",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Allergens",
                table: "Allergens",
                column: "_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllergenIngredient",
                table: "AllergenIngredient",
                columns: new[] { "Allergens_id", "IngredientId" });

            migrationBuilder.CreateIndex(
                name: "IX_Allergens_NormalizedName",
                table: "Allergens",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AllergenIngredient_Allergens_Allergens_id",
                table: "AllergenIngredient",
                column: "Allergens_id",
                principalTable: "Allergens",
                principalColumn: "_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllergenIngredient_Allergens_Allergens_id",
                table: "AllergenIngredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Allergens",
                table: "Allergens");

            migrationBuilder.DropIndex(
                name: "IX_Allergens_NormalizedName",
                table: "Allergens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AllergenIngredient",
                table: "AllergenIngredient");

            migrationBuilder.DropColumn(
                name: "_id",
                table: "Allergens");

            migrationBuilder.DropColumn(
                name: "Allergens_id",
                table: "AllergenIngredient");

            migrationBuilder.AddColumn<string>(
                name: "AllergensNormalizedName",
                table: "AllergenIngredient",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Allergens",
                table: "Allergens",
                column: "NormalizedName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllergenIngredient",
                table: "AllergenIngredient",
                columns: new[] { "AllergensNormalizedName", "IngredientId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AllergenIngredient_Allergens_AllergensNormalizedName",
                table: "AllergenIngredient",
                column: "AllergensNormalizedName",
                principalTable: "Allergens",
                principalColumn: "NormalizedName",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
