using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Scente.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Brand = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Category = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gender = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TopNotes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MiddleNotes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BaseNotes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PromoCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiscountRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCodes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JoinDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Role = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "customer")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductVolumes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVolumes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVolumes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaymentMethod = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalPaid = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ShippingAddress = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PostalCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PasswordResetTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Token = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResetTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AuthorName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Wishlists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wishlists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WishlistItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WishlistId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishlistItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WishlistItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WishlistItems_Wishlists_WishlistId",
                        column: x => x.WishlistId,
                        principalTable: "Wishlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BaseNotes", "Brand", "Category", "Description", "Gender", "Image", "MiddleNotes", "Name", "Price", "Status", "Stock", "TopNotes" },
                values: new object[,]
                {
                    { 1, "Vetiver, Musk, Guaiac Wood, Benzoin", "Orto Parisi", "Niche", "A deep, earthy fragrance with raspberry and pomegranate top notes, birch and amber in the heart, and a woody-musky base.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.45475.2x.avif", "Birch, Amber, Geranium", "Terroni", 155m, "active", 10, "Raspberry, Pomegranate" },
                    { 2, "Musk, Ambroxan, Cedar", "Orto Parisi", "Niche", "An oceanic, aquatic fragrance with fresh citrus opening and a deep marine heart.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.53471.2x.avif", "Seaweed, Calone, Hedione", "Megamare", 155m, "active", 8, "Bergamot, Lemon" },
                    { 3, "Leather, Animal Notes, Incense, Labdanum, Cade oil, Patchouli, Cedar, Styrax", "Orto Parisi", "Niche", "A bold leather fragrance with spicy top notes and a complex, resinous base.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.69923.2x.avif", "Violet", "Cuoium", 155m, "active", 5, "Black Pepper, Mandarin Orange" },
                    { 4, "Woody Notes, Musk", "Orto Parisi", "Niche", "A fresh, green fragrance capturing the essence of nature.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.24193.2x.avif", "Green Accord", "Viride", 155m, "active", 7, "Herbal Notes, Green Notes" },
                    { 5, "Musk, Oakmoss, Ambergris, Vanille", "Creed", "Luxury", "An iconic fruity-floral fragrance with pineapple and birch, perfect for the confident man.", "Men", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.9828.2x.avif", "Birch, Patchouli, Moroccan Jasmine, Rose", "Aventus", 235m, "active", 15, "Pineapple, Bergamot, Black Currant, Apple" },
                    { 6, "Ambergris, Sandalwood", "Creed", "Luxury", "A fresh, green fragrance inspired by the Irish countryside.", "Men", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.474.2x.avif", "Violet Leaf", "Green Irish Tweed", 220m, "active", 12, "Lemon Verbena, Iris" },
                    { 7, "Musk, Petitgrain, Sandalwood, Galbanum", "Creed", "Luxury", "A fresh, aquatic fragrance evoking crisp mountain air.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.472.2x.avif", "Green Tea, Black Currant", "Silver Mountain Water", 220m, "active", 9, "Bergamot, Mandarin Orange" },
                    { 8, "Sea Notes, Musk, Woody Notes", "Creed", "Luxury", "A luxurious fruity-floral with sea salt and citrus notes.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.466.2x.avif", "Sicilian Lemon, Bergamot, Iris, Mandarin Orange", "Millésime Impérial", 220m, "active", 6, "Sea Salt, Fruity Notes" },
                    { 9, "Praline, Vanilla, Sandalwood", "Kilian", "Luxury", "A gourmand fragrance inspired by cognac, with warm cinnamon and vanilla.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.62615.2x.avif", "Cinnamon, Tonka Bean, Oak", "Angels' Share", 275m, "active", 14, "Cognac" },
                    { 10, "Sugar, Vanilla, Caramel, Musk, Civet, Labdanum", "Kilian", "Luxury", "A sweet, romantic fragrance with marshmallow and orange blossom.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.4322.2x.avif", "Orange Blossom, Jasmine, Honeysuckle, Rose, Iris", "Love, Don't Be Shy", 295m, "active", 11, "Neroli, Bergamot, Pink Pepper, Coriander" },
                    { 11, "Heliotrope, Sandalwood", "Kilian", "Luxury", "A dark gourmand with rum, coffee, and chocolate notes.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.43632.2x.avif", "Dark Chocolate, Coffee, Caramel, Almond", "Black Phantom", 295m, "active", 4, "Rum, Sugar Cane" },
                    { 12, "Virginia Cedar, Musk, Amber, Vanilla", "Kilian", "Luxury", "A woody-spicy fragrance with rum and dried fruits.", "Men", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.4323.2x.avif", "Nutmeg, Patchouli, Jasmine", "Straight to Heaven", 295m, "active", 8, "Dried Fruits, Rum" },
                    { 13, "Ambergris, Woody Notes, Musk", "Maison Francis Kurkdjian", "Luxury", "An iconic amber-woody fragrance with saffron and cedar.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.46066.2x.avif", "Egyptian Jasmine, Cedar", "Baccarat Rouge 540 Extrait", 375m, "active", 20, "Bitter Almond, Saffron" },
                    { 14, "Vanilla, Spanish Labdanum, Lavender", "Maison Francis Kurkdjian", "Luxury", "A warm amber-vanilla fragrance for evening wear.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.40816.2x.avif", "Tonka Bean", "Grand Soir", 205m, "active", 13, "Amber, Siam Benzoin" },
                    { 15, "Vanilla, Agarwood (Oud), Benzoin", "Maison Francis Kurkdjian", "Luxury", "A luxurious rose-oud fragrance with vanilla.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.30352.2x.avif", "Bulgarian Rose, Turkish Rose", "Oud Satin Mood", 255m, "active", 7, "Violet" },
                    { 16, "Vanilla, Amber, Musk, Woody Notes", "Maison Francis Kurkdjian", "Luxury", "A spicy-vanilla fragrance with juniper berries and nutmeg.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.53401.2x.avif", "Coriander", "Gentle Fluidity Gold", 205m, "active", 9, "Juniper Berries, Nutmeg" },
                    { 17, "Vetiver, Amber, Musk, Virginia Cedar", "Byredo", "Niche", "A vibrant, sunny fragrance with vetiver and African orange flower.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.6458.2x.avif", "Violet, Cyclamen, Jasmine", "Bal d'Afrique", 165m, "active", 18, "Amalfi Lemon, Tagetes, Black Currant, Bergamot, African Orange Flower" },
                    { 18, "Vanilla, Sandalwood, Amber", "Byredo", "Niche", "A woody-vanilla fragrance inspired by the Romani lifestyle.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.25293.2x.avif", "Pine Needles, Incense, Orris Root", "Gypsy Water", 165m, "active", 16, "Juniper, Lemon, Bergamot, Pepper" },
                    { 19, "Ambergris, Cedar", "Byredo", "Niche", "A fresh, woody fragrance inspired by the Mojave Desert.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.27040.2x.avif", "Magnolia, Violet, Sandalwood", "Mojave Ghost", 165m, "active", 14, "Sapodilla, Ambrette (Musk Mallow)" },
                    { 20, "Musk, Woody Notes, Sandalwood", "Byredo", "Niche", "A clean, aldehydic floral fragrance like fresh laundry.", "Women", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.6686.2x.avif", "Peony, Violet, African Orange Flower", "Blanche", 165m, "active", 11, "Aldehydes, Rose, Pink Pepper" },
                    { 21, "Leather, Agarwood (Oud), Patchouli, Sandalwood", "Amouage", "Luxury", "A bold, smoky fragrance with oregano and leather.", "Men", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.15294.2x.avif", "Incense, Amber, Labdanum, Opoponax", "Interlude Man", 395m, "active", 5, "Oregano, Pepper, Bergamot" },
                    { 22, "Sandalwood, Vetiver, Cedar, Patchouli", "Amouage", "Luxury", "A sophisticated floral-woody fragrance.", "Men", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.920.2x.avif", "Jasmine, Neroli, Orris Root, Ylang-Ylang", "Reflection Man", 395m, "active", 6, "Rosemary, Pink Pepper, Petitgrain" },
                    { 23, "Myrrh, Agarwood (Oud), Opoponax, Patchouli, Ambergris, Cedar, Musk", "Amouage", "Luxury", "A complex, fruity-woody fragrance with blackberry and myrrh.", "Men", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.2366.2x.avif", "Honey, Guaiac Wood, Cinnamon, Bay Leaf, Clove, Rose, Orchid", "Jubilation XXV", 395m, "active", 4, "Blackberry, Olibanum, Orange, Coriander, Labdanum, Tarragon" },
                    { 24, "Agarwood (Oud), Incense, Patchouli, Guaiac Wood, Orris Root, Sandalwood, Amber, Vanilla, Musk", "Amouage", "Luxury", "A spicy-woody fragrance with rose and oud.", "Women", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.6538.2x.avif", "Rose, Geranium, Jasmine, Tea", "Epic Woman", 395m, "active", 8, "Caraway, Pink Pepper, Cinnamon" },
                    { 25, "Ambergris, Benzoin, Musk", "Zoologist", "Niche", "An oceanic-amber fragrance with ink and ambergris.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.56294.2x.avif", "Sea Salt, Ink, Opoponax", "Squid", 200m, "active", 7, "Incense, Pink Pepper, Salicylate" },
                    { 26, "Cade Oil, Leather, Resins, Civet, Amber, Cedar, Frankincense, Patchouli, Sandalwood, Vanilla", "Zoologist", "Niche", "A fiery, metallic fragrance with smoke and leather.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.51353.2x.avif", "Champaca, Geranium, Jasmine, Osmanthus, Rose, Ylang-Ylang", "T-Rex", 240m, "active", 3, "Bay Leaf, Black Pepper, Bergamot, Pine, Neroli, Nutmeg" },
                    { 27, "Benzoin, Labdanum, Musks, Sandalwood, Vanilla", "Zoologist", "Niche", "A sweet, honey-forward fragrance with royal jelly.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.58140.2x.avif", "Broom, Heliotrope, Mimosa, Orange Blossom", "Bee", 240m, "active", 6, "Orange, Ginger Syrup, Royal Jelly Accord" },
                    { 28, "Ambergris, Benzoin, Cypriol Oil, Guaiac Wood, Honey, Musk, Patchouli, Resins, Smoke", "Zoologist", "Niche", "A dark, powdery fragrance with smoke and honey.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.49270.2x.avif", "Heliotrope, Iris, Jasmine, Mimosa, Rose", "Moth", 200m, "active", 5, "Black Pepper, Clove, Cumin, Lemon, Nutmeg, Saffron" },
                    { 29, "Patchouli, Musk", "Initio", "Niche", "A powerful oud-saffron fragrance with nutmeg and lavender.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.53641.2x.avif", "Agarwood (Oud)", "Oud for Greatness", 250m, "active", 12, "Saffron, Nutmeg, Lavender" },
                    { 30, "Cinnamon", "Initio", "Niche", "A boozy, tobacco-vanilla fragrance with cinnamon.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.42260.2x.avif", "Tobacco", "Side Effect", 220m, "active", 10, "Rum, Vanilla" },
                    { 31, "Black Currant, Sandalwood, Pink Musk", "Initio", "Niche", "A clean, white musk fragrance with citrus and magnolia.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.66097.2x.avif", "Mandarin Orange, Magnolia", "Musk Therapy", 230m, "active", 9, "White Musk, Bergamot" },
                    { 32, "Sandalwood, Musk, Guaiac Wood", "Initio", "Niche", "A fresh, lavender-based fragrance with vetiver and sandalwood.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.50351.2x.avif", "Vetiver, Patchouli, Cedar", "Rehab", 290m, "active", 8, "Lavender, Bergamot" },
                    { 33, "Agarwood (Oud), Cedar, Leather, Sandalwood, Amber, Musk, Tonka Bean, Vanilla", "Penhaligon's", "Luxury", "A dark, spicy-woody fragrance with rose and oud.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.31396.2x.avif", "Bulgarian Rose, Nutmeg, Jasmine", "Halfeti", 245m, "active", 11, "Cypress Leaf, Saffron, Cardamom, Artemisia, Bergamot, Grapefruit" },
                    { 34, "Amber, Tonka Bean", "Penhaligon's", "Luxury", "A sophisticated brandy-amber fragrance with shaving soap notes.", "Men", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.40716.2x.avif", "Woodsy Notes, Shaving Soap", "The Tragedy of Lord George", 275m, "active", 6, "Brandy" },
                    { 35, "Sandalwood, Leather, Myrrh, Musk, Vetiver, Cardamom, Pepper, Olibanum", "Penhaligon's", "Luxury", "A lavender-coffee fragrance with leather and sandalwood.", "Men", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.5674.2x.avif", "Coffee, Geranium", "Endymion", 160m, "active", 13, "Lavender, Bergamot, Sage, Mandarin Orange" },
                    { 36, "Incense, Tonka Bean", "Penhaligon's", "Luxury", "A vanilla-incense fragrance with grapefruit and ambrette.", "Men", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.71646.2x.avif", "Ambrette (Musk Mallow), Clary Sage", "The World According To Arthur", 275m, "active", 5, "Vanilla, Grapefruit" },
                    { 37, "Vanilla, Cardamom, Sandalwood, Pepper, Patchouli, Guaiac Wood", "Parfums de Marly", "Luxury", "A fresh, apple-lavender fragrance with vanilla and cardamom.", "Men", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.39314.2x.avif", "Geranium, Violet, Jasmine", "Layton", 225m, "active", 18, "Apple, Lavender, Bergamot, Mandarin Orange" },
                    { 38, "Agarwood (Oud), Cedar", "Parfums de Marly", "Luxury", "An oud-praline fragrance with clary sage and lavender.", "Men", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.70776.2x.avif", "Saffron, Praline", "Haltane", 275m, "active", 7, "Clary Sage, Lavender, Bergamot" },
                    { 39, "Vanilla, Iso E Super, Musk, Cedar, Cypriol Oil, Vetiver", "Parfums de Marly", "Luxury", "A warm, spicy-vanilla fragrance with cinnamon and tobacco.", "Men", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.16939.2x.avif", "Tobacco Leaf, Incense, Osmanthus, Labdanum", "Herod", 225m, "active", 14, "Cinnamon, Pepper" },
                    { 40, "Cashmeran, Haitian Vetiver, Cedar, Incense", "Parfums de Marly", "Luxury", "A vibrant floral-fruity fragrance with rose and lychee.", "Women", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.43871.2x.avif", "Turkish Rose, Peony, Petalia, Vanilla, Musk", "Delina", 285m, "active", 16, "Rhubarb, Litchi, Bergamot, Nutmeg" },
                    { 41, "Oakmoss, Woody Notes", "Nishane", "Niche", "A fresh pineapple and cedar fragrance with oakmoss depth.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.44174.2x.avif", "Cedar, Patchouli, Jasmine", "Hacivat", 280m, "active", 11, "Pineapple, Grapefruit, Bergamot" },
                    { 42, "Vanilla, Benzoin, Sandalwood, Musk, Patchouli, Cedar, Ambergris", "Nishane", "Niche", "A complex spicy-floral fragrance with rose and vanilla.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.54785.2x.avif", "Cardamom, Black Currant, Turkish Rose", "Ani", 260m, "active", 9, "Ginger, Bergamot, Pink Pepper, Green Notes" },
                    { 43, "Fig, Musk", "Nishane", "Niche", "A delicate tea fragrance with citrus and fig.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.30567.2x.avif", "Tea, Nutmeg", "Wulong Cha", 260m, "active", 8, "Bergamot, Orange, Mandarin Orange, Litchi" },
                    { 44, "Chinese Cedar, Oakmoss", "Nishane", "Niche", "A warm, tropical fragrance with coconut and tobacco.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.37603.2x.avif", "Tobacco, Tonka Bean", "Fan Your Flames", 260m, "active", 7, "Coconut, Rum" },
                    { 45, "Agarwood (Oud), Incense", "Nasomatto", "Niche", "A dark, resinous fragrance with oud and incense.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.6472.2x.avif", "Resins, Woodsy Notes, Tobacco, Coffee", "Black Afgano", 130m, "active", 9, "Cannabis, Green Notes" },
                    { 46, "Woody Notes, Musk, Ambrette (Musk Mallow), Ambroxan", "Nasomatto", "Niche", "A boozy, rose-tinged fragrance with woody musk.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.40200.2x.avif", "Rose", "Baraonda", 130m, "active", 7, "Whiskey" },
                    { 47, "Agarwood (Oud), Sandalwood", "Nasomatto", "Niche", "A floral-chocolate fragrance with oud warmth.", "Men", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.12130.2x.avif", "Dark Chocolate, Tonka Bean, Cinnamon", "Pardon", 130m, "active", 6, "Magnolia, Flowers" },
                    { 48, "Woody Notes", "Nasomatto", "Niche", "A bold leather and woody spice fragrance.", "Men", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.4291.2x.avif", "Spices", "Duro", 130m, "active", 5, "Leather" },
                    { 49, "White Musk", "Etat Libre d'Orange", "Niche", "A fresh, green-aquatic fragrance with mint and white musk.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.43531.2x.avif", "Green Notes, Cassis, Rose, Hedione", "You Or Someone Like You", 105m, "active", 12, "Mint, Grapefruit, Bergamot, Anise" },
                    { 50, "Vanilla, Opoponax, Chestnut", "Etat Libre d'Orange", "Niche", "A creamy vetiver fragrance with myrrh and vanilla.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.7139.2x.avif", "Myrrh, Olive Leaf", "Fat Electrician", 105m, "active", 10, "Vetiver, Whipped Cream" },
                    { 51, "Iris, Coconut, Opoponax", "Etat Libre d'Orange", "Niche", "A radically avant-garde fragrance — not for the faint-hearted.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.4523.2x.avif", "Milk, Adrenaline, Blood", "Secretions Magnifiques", 105m, "active", 8, "Seaweed, Iodine" },
                    { 52, "Sandalwood, Lorenox, Labdanum", "Etat Libre d'Orange", "Niche", "A spicy, effervescent fragrance with cardamom and jasmine.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.30504.2x.avif", "Curry Tree, Black Pepper, Jasmine", "Remarkable People", 105m, "active", 14, "Champagne, Grapefruit, Cardamom" },
                    { 53, "Tobacco Leaf, Vanilla, Tonka Bean", "Xerjoff", "Luxury", "A honeyed lavender fragrance with tobacco and vanilla.", "Men", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.30529.2x.avif", "Honey, Cinnamon, Cashmeran, Jasmine Sambac", "Naxos", 200m, "active", 9, "Lavender, Bergamot, Lemon" },
                    { 54, "Agarwood (Oud), Sandalwood, Vanilla, Amber, Musk", "Xerjoff", "Luxury", "A rich, oriental fragrance with oud, rose, and cedar.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.17786.2x.avif", "Rose, Cedar, Lily-of-the-Valley", "Alexandria II", 266m, "active", 7, "Palisander Rosewood, Lavender, Cinnamon, Apple" },
                    { 55, "White Musk, Madagascar Vanilla, Amber", "Xerjoff", "Luxury", "A sunny, citrus-vanilla fragrance bursting with Mediterranean warmth.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.55157.2x.avif", "Fruits", "Erba Pura", 135m, "active", 15, "Sicilian Orange, Calabrian Bergamot, Sicilian Lemon" },
                    { 56, "Musk, Amber, Virginia Cedar, Patchouli", "Xerjoff", "Luxury", "A fresh citrus fragrance with floral heart and woody base.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.12126.2x.avif", "Mint, Lily-of-the-Valley, Rose", "Renaissance", 200m, "active", 8, "Amalfi Lemon, Tangerine, Bergamot, Petitgrain" },
                    { 57, "Ambrofix", "Tom Ford", "Luxury", "A sleek, modern leather fragrance with spiced warmth.", "Men", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.95389.2x.avif", "Vanilla, Leather", "Eau d'Ombré Leather", 140m, "active", 12, "Cardamom, Ginger, Coriander" },
                    { 58, "Vetiver, Oakmoss, Amber", "Tom Ford", "Luxury", "A sophisticated vetiver fragrance with citrus and oakmoss.", "Men", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.6697.2x.avif", "Nutmeg, Orris Root, Pimento", "Grey Vetiver", 165m, "active", 10, "Grapefruit, Orange Blossom, Sage" },
                    { 59, "Incense, Vetiver, Oak", "Tom Ford", "Luxury", "A coastal, woody fragrance inspired by the Italian Riviera.", "Unisex", "https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.64617.2x.avif", "Cypress, Lemon, Yellow Mandarin, Lavender", "Costa Azzurra", 165m, "active", 9, "Driftwood, Seaweed, Agarwood" }
                });

            migrationBuilder.InsertData(
                table: "ProductVolumes",
                columns: new[] { "Id", "Price", "ProductId", "Size" },
                values: new object[,]
                {
                    { 1, 155m, 1, "50ml" },
                    { 2, 155m, 2, "50ml" },
                    { 3, 155m, 3, "50ml" },
                    { 4, 155m, 4, "50ml" },
                    { 5, 170m, 5, "30ml" },
                    { 6, 235m, 5, "50ml" },
                    { 7, 330m, 5, "100ml" },
                    { 8, 220m, 6, "50ml" },
                    { 9, 320m, 6, "100ml" },
                    { 10, 165m, 7, "30ml" },
                    { 11, 220m, 7, "50ml" },
                    { 12, 320m, 7, "100ml" },
                    { 13, 220m, 8, "50ml" },
                    { 14, 320m, 8, "100ml" },
                    { 15, 275m, 9, "50ml" },
                    { 16, 415m, 9, "100ml" },
                    { 17, 295m, 10, "50ml" },
                    { 18, 450m, 10, "100ml" },
                    { 19, 295m, 11, "50ml" },
                    { 20, 450m, 11, "100ml" },
                    { 21, 295m, 12, "50ml" },
                    { 22, 450m, 12, "100ml" },
                    { 23, 245m, 13, "35ml" },
                    { 24, 375m, 13, "70ml" },
                    { 25, 135m, 14, "35ml" },
                    { 26, 205m, 14, "70ml" },
                    { 27, 165m, 15, "35ml" },
                    { 28, 255m, 15, "70ml" },
                    { 29, 135m, 16, "35ml" },
                    { 30, 205m, 16, "70ml" },
                    { 31, 165m, 17, "50ml" },
                    { 32, 240m, 17, "100ml" },
                    { 33, 165m, 18, "50ml" },
                    { 34, 240m, 18, "100ml" },
                    { 35, 165m, 19, "50ml" },
                    { 36, 240m, 19, "100ml" },
                    { 37, 165m, 20, "50ml" },
                    { 38, 240m, 20, "100ml" },
                    { 39, 395m, 21, "100ml" },
                    { 40, 395m, 22, "100ml" },
                    { 41, 395m, 23, "100ml" },
                    { 42, 395m, 24, "100ml" },
                    { 43, 200m, 25, "60ml" },
                    { 44, 240m, 26, "60ml" },
                    { 45, 240m, 27, "60ml" },
                    { 46, 200m, 28, "60ml" },
                    { 47, 250m, 29, "50ml" },
                    { 48, 320m, 29, "90ml" },
                    { 49, 220m, 30, "50ml" },
                    { 50, 280m, 30, "90ml" },
                    { 51, 230m, 31, "50ml" },
                    { 52, 290m, 31, "90ml" },
                    { 53, 290m, 32, "90ml" },
                    { 54, 245m, 33, "100ml" },
                    { 55, 275m, 34, "75ml" },
                    { 56, 83m, 35, "30ml" },
                    { 57, 160m, 35, "100ml" },
                    { 58, 275m, 36, "75ml" },
                    { 59, 225m, 37, "75ml" },
                    { 60, 290m, 37, "125ml" },
                    { 61, 275m, 38, "75ml" },
                    { 62, 325m, 38, "125ml" },
                    { 63, 225m, 39, "75ml" },
                    { 64, 290m, 39, "125ml" },
                    { 65, 180m, 40, "30ml" },
                    { 66, 285m, 40, "75ml" },
                    { 67, 280m, 41, "50ml" },
                    { 68, 395m, 41, "100ml" },
                    { 69, 260m, 42, "50ml" },
                    { 70, 375m, 42, "100ml" },
                    { 71, 260m, 43, "50ml" },
                    { 72, 375m, 43, "100ml" },
                    { 73, 260m, 44, "50ml" },
                    { 74, 375m, 44, "100ml" },
                    { 75, 130m, 45, "30ml" },
                    { 76, 130m, 46, "30ml" },
                    { 77, 130m, 47, "30ml" },
                    { 78, 130m, 48, "30ml" },
                    { 79, 105m, 49, "50ml" },
                    { 80, 160m, 49, "100ml" },
                    { 81, 105m, 50, "50ml" },
                    { 82, 160m, 50, "100ml" },
                    { 83, 105m, 51, "50ml" },
                    { 84, 105m, 52, "50ml" },
                    { 85, 160m, 52, "100ml" },
                    { 86, 200m, 53, "100ml" },
                    { 87, 266m, 54, "50ml" },
                    { 88, 446m, 54, "100ml" },
                    { 89, 135m, 55, "50ml" },
                    { 90, 200m, 55, "100ml" },
                    { 91, 200m, 56, "100ml" },
                    { 92, 140m, 57, "100ml" },
                    { 93, 165m, 58, "100ml" },
                    { 94, 165m, 59, "100ml" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                table: "Orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_UserId",
                table: "PasswordResetTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVolumes_ProductId",
                table: "ProductVolumes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_Code",
                table: "PromoCodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_ProductId",
                table: "WishlistItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_WishlistId_ProductId",
                table: "WishlistItems",
                columns: new[] { "WishlistId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_UserId",
                table: "Wishlists",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "PasswordResetTokens");

            migrationBuilder.DropTable(
                name: "ProductVolumes");

            migrationBuilder.DropTable(
                name: "PromoCodes");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "WishlistItems");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Wishlists");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
