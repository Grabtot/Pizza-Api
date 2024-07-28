using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaApi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TagId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientTag_Tags_TagsNormalizedName",
                table: "IngredientTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IngredientTag",
                table: "IngredientTag");

            migrationBuilder.DropIndex(
                name: "IX_IngredientTag_TagsNormalizedName",
                table: "IngredientTag");

            migrationBuilder.DropColumn(
                name: "TagsNormalizedName",
                table: "IngredientTag");

            migrationBuilder.AddColumn<Guid>(
                name: "_id",
                table: "Tags",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Tags_id",
                table: "IngredientTag",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngredientTag",
                table: "IngredientTag",
                columns: new[] { "IngredientId", "Tags_id" });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_NormalizedName",
                table: "Tags",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IngredientTag_Tags_id",
                table: "IngredientTag",
                column: "Tags_id");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientTag_Tags_Tags_id",
                table: "IngredientTag",
                column: "Tags_id",
                principalTable: "Tags",
                principalColumn: "_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientTag_Tags_Tags_id",
                table: "IngredientTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_NormalizedName",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IngredientTag",
                table: "IngredientTag");

            migrationBuilder.DropIndex(
                name: "IX_IngredientTag_Tags_id",
                table: "IngredientTag");

            migrationBuilder.DropColumn(
                name: "_id",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Tags_id",
                table: "IngredientTag");

            migrationBuilder.AddColumn<string>(
                name: "TagsNormalizedName",
                table: "IngredientTag",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "NormalizedName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngredientTag",
                table: "IngredientTag",
                columns: new[] { "IngredientId", "TagsNormalizedName" });

            migrationBuilder.CreateIndex(
                name: "IX_IngredientTag_TagsNormalizedName",
                table: "IngredientTag",
                column: "TagsNormalizedName");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientTag_Tags_TagsNormalizedName",
                table: "IngredientTag",
                column: "TagsNormalizedName",
                principalTable: "Tags",
                principalColumn: "NormalizedName",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
