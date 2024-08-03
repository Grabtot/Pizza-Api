using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaApi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AllergenAndTagId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllergenIngredient_Allergens_Allergens_id",
                table: "AllergenIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_IngredientTag_Tags_Tags_id",
                table: "IngredientTag");

            migrationBuilder.RenameColumn(
                name: "_id",
                table: "Tags",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Tags_id",
                table: "IngredientTag",
                newName: "TagsId");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientTag_Tags_id",
                table: "IngredientTag",
                newName: "IX_IngredientTag_TagsId");

            migrationBuilder.RenameColumn(
                name: "_id",
                table: "Allergens",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Allergens_id",
                table: "AllergenIngredient",
                newName: "AllergensId");

            migrationBuilder.AddForeignKey(
                name: "FK_AllergenIngredient_Allergens_AllergensId",
                table: "AllergenIngredient",
                column: "AllergensId",
                principalTable: "Allergens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientTag_Tags_TagsId",
                table: "IngredientTag",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllergenIngredient_Allergens_AllergensId",
                table: "AllergenIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_IngredientTag_Tags_TagsId",
                table: "IngredientTag");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tags",
                newName: "_id");

            migrationBuilder.RenameColumn(
                name: "TagsId",
                table: "IngredientTag",
                newName: "Tags_id");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientTag_TagsId",
                table: "IngredientTag",
                newName: "IX_IngredientTag_Tags_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Allergens",
                newName: "_id");

            migrationBuilder.RenameColumn(
                name: "AllergensId",
                table: "AllergenIngredient",
                newName: "Allergens_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AllergenIngredient_Allergens_Allergens_id",
                table: "AllergenIngredient",
                column: "Allergens_id",
                principalTable: "Allergens",
                principalColumn: "_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientTag_Tags_Tags_id",
                table: "IngredientTag",
                column: "Tags_id",
                principalTable: "Tags",
                principalColumn: "_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
