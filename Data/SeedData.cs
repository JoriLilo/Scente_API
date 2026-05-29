using Microsoft.EntityFrameworkCore;
using Scente.API.Entity;

namespace Scente.API.Data;

public static class SeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var products = new List<Product>
        {
            // ── Orto Parisi ──────────────────────────────────
            new() { Id=1,  Name="Terroni",            Brand="Orto Parisi",            Category="Niche",  Gender="Unisex", Price=155, Stock=10, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.45475.2x.avif", TopNotes="Raspberry, Pomegranate",                  MiddleNotes="Birch, Amber, Geranium",                               BaseNotes="Vetiver, Musk, Guaiac Wood, Benzoin",                            Description="A deep, earthy fragrance with raspberry and pomegranate top notes, birch and amber in the heart, and a woody-musky base.",           Status="active" },
            new() { Id=2,  Name="Megamare",           Brand="Orto Parisi",            Category="Niche",  Gender="Unisex", Price=155, Stock=8,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.53471.2x.avif", TopNotes="Bergamot, Lemon",                         MiddleNotes="Seaweed, Calone, Hedione",                             BaseNotes="Musk, Ambroxan, Cedar",                                          Description="An oceanic, aquatic fragrance with fresh citrus opening and a deep marine heart.",                                                     Status="active" },
            new() { Id=3,  Name="Cuoium",             Brand="Orto Parisi",            Category="Niche",  Gender="Unisex", Price=155, Stock=5,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.69923.2x.avif", TopNotes="Black Pepper, Mandarin Orange",            MiddleNotes="Violet",                                               BaseNotes="Leather, Animal Notes, Incense, Labdanum, Cade oil, Patchouli, Cedar, Styrax", Description="A bold leather fragrance with spicy top notes and a complex, resinous base.",                                        Status="active" },
            new() { Id=4,  Name="Viride",             Brand="Orto Parisi",            Category="Niche",  Gender="Unisex", Price=155, Stock=7,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.24193.2x.avif", TopNotes="Herbal Notes, Green Notes",               MiddleNotes="Green Accord",                                         BaseNotes="Woody Notes, Musk",                                              Description="A fresh, green fragrance capturing the essence of nature.",                                                                            Status="active" },

            // ── Creed ────────────────────────────────────────
            new() { Id=5,  Name="Aventus",            Brand="Creed",                  Category="Luxury", Gender="Men",    Price=235, Stock=15, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.9828.2x.avif",  TopNotes="Pineapple, Bergamot, Black Currant, Apple", MiddleNotes="Birch, Patchouli, Moroccan Jasmine, Rose",             BaseNotes="Musk, Oakmoss, Ambergris, Vanille",                              Description="An iconic fruity-floral fragrance with pineapple and birch, perfect for the confident man.",                                          Status="active" },
            new() { Id=6,  Name="Green Irish Tweed",  Brand="Creed",                  Category="Luxury", Gender="Men",    Price=220, Stock=12, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.474.2x.avif",   TopNotes="Lemon Verbena, Iris",                     MiddleNotes="Violet Leaf",                                          BaseNotes="Ambergris, Sandalwood",                                          Description="A fresh, green fragrance inspired by the Irish countryside.",                                                                          Status="active" },
            new() { Id=7,  Name="Silver Mountain Water", Brand="Creed",              Category="Luxury", Gender="Unisex", Price=220, Stock=9,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.472.2x.avif",   TopNotes="Bergamot, Mandarin Orange",               MiddleNotes="Green Tea, Black Currant",                             BaseNotes="Musk, Petitgrain, Sandalwood, Galbanum",                         Description="A fresh, aquatic fragrance evoking crisp mountain air.",                                                                               Status="active" },
            new() { Id=8,  Name="Millésime Impérial", Brand="Creed",                  Category="Luxury", Gender="Unisex", Price=220, Stock=6,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.466.2x.avif",   TopNotes="Sea Salt, Fruity Notes",                  MiddleNotes="Sicilian Lemon, Bergamot, Iris, Mandarin Orange",      BaseNotes="Sea Notes, Musk, Woody Notes",                                   Description="A luxurious fruity-floral with sea salt and citrus notes.",                                                                            Status="active" },

            // ── Kilian ───────────────────────────────────────
            new() { Id=9,  Name="Angels' Share",      Brand="Kilian",                 Category="Luxury", Gender="Unisex", Price=275, Stock=14, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.62615.2x.avif", TopNotes="Cognac",                                  MiddleNotes="Cinnamon, Tonka Bean, Oak",                            BaseNotes="Praline, Vanilla, Sandalwood",                                   Description="A gourmand fragrance inspired by cognac, with warm cinnamon and vanilla.",                                                             Status="active" },
            new() { Id=10, Name="Love, Don't Be Shy", Brand="Kilian",                 Category="Luxury", Gender="Unisex", Price=295, Stock=11, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.4322.2x.avif",  TopNotes="Neroli, Bergamot, Pink Pepper, Coriander", MiddleNotes="Orange Blossom, Jasmine, Honeysuckle, Rose, Iris",     BaseNotes="Sugar, Vanilla, Caramel, Musk, Civet, Labdanum",                Description="A sweet, romantic fragrance with marshmallow and orange blossom.",                                                                     Status="active" },
            new() { Id=11, Name="Black Phantom",      Brand="Kilian",                 Category="Luxury", Gender="Unisex", Price=295, Stock=4,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.43632.2x.avif", TopNotes="Rum, Sugar Cane",                         MiddleNotes="Dark Chocolate, Coffee, Caramel, Almond",              BaseNotes="Heliotrope, Sandalwood",                                         Description="A dark gourmand with rum, coffee, and chocolate notes.",                                                                               Status="active" },
            new() { Id=12, Name="Straight to Heaven", Brand="Kilian",                 Category="Luxury", Gender="Men",    Price=295, Stock=8,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.4323.2x.avif",  TopNotes="Dried Fruits, Rum",                       MiddleNotes="Nutmeg, Patchouli, Jasmine",                           BaseNotes="Virginia Cedar, Musk, Amber, Vanilla",                           Description="A woody-spicy fragrance with rum and dried fruits.",                                                                                   Status="active" },

            // ── MFK ──────────────────────────────────────────
            new() { Id=13, Name="Baccarat Rouge 540 Extrait", Brand="Maison Francis Kurkdjian", Category="Luxury", Gender="Unisex", Price=375, Stock=20, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.46066.2x.avif", TopNotes="Bitter Almond, Saffron", MiddleNotes="Egyptian Jasmine, Cedar", BaseNotes="Ambergris, Woody Notes, Musk", Description="An iconic amber-woody fragrance with saffron and cedar.", Status="active" },
            new() { Id=14, Name="Grand Soir",          Brand="Maison Francis Kurkdjian", Category="Luxury", Gender="Unisex", Price=205, Stock=13, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.40816.2x.avif", TopNotes="Amber, Siam Benzoin",            MiddleNotes="Tonka Bean",                                           BaseNotes="Vanilla, Spanish Labdanum, Lavender",                            Description="A warm amber-vanilla fragrance for evening wear.",                                                                                     Status="active" },
            new() { Id=15, Name="Oud Satin Mood",      Brand="Maison Francis Kurkdjian", Category="Luxury", Gender="Unisex", Price=255, Stock=7,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.30352.2x.avif", TopNotes="Violet",                         MiddleNotes="Bulgarian Rose, Turkish Rose",                         BaseNotes="Vanilla, Agarwood (Oud), Benzoin",                               Description="A luxurious rose-oud fragrance with vanilla.",                                                                                         Status="active" },
            new() { Id=16, Name="Gentle Fluidity Gold",Brand="Maison Francis Kurkdjian", Category="Luxury", Gender="Unisex", Price=205, Stock=9,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.53401.2x.avif", TopNotes="Juniper Berries, Nutmeg",        MiddleNotes="Coriander",                                            BaseNotes="Vanilla, Amber, Musk, Woody Notes",                              Description="A spicy-vanilla fragrance with juniper berries and nutmeg.",                                                                           Status="active" },

            // ── Byredo ───────────────────────────────────────
            new() { Id=17, Name="Bal d'Afrique",       Brand="Byredo",                 Category="Niche",  Gender="Unisex", Price=165, Stock=18, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.6458.2x.avif",  TopNotes="Amalfi Lemon, Tagetes, Black Currant, Bergamot, African Orange Flower", MiddleNotes="Violet, Cyclamen, Jasmine", BaseNotes="Vetiver, Amber, Musk, Virginia Cedar", Description="A vibrant, sunny fragrance with vetiver and African orange flower.", Status="active" },
            new() { Id=18, Name="Gypsy Water",         Brand="Byredo",                 Category="Niche",  Gender="Unisex", Price=165, Stock=16, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.25293.2x.avif", TopNotes="Juniper, Lemon, Bergamot, Pepper",        MiddleNotes="Pine Needles, Incense, Orris Root",                    BaseNotes="Vanilla, Sandalwood, Amber",                                     Description="A woody-vanilla fragrance inspired by the Romani lifestyle.",                                                                          Status="active" },
            new() { Id=19, Name="Mojave Ghost",        Brand="Byredo",                 Category="Niche",  Gender="Unisex", Price=165, Stock=14, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.27040.2x.avif", TopNotes="Sapodilla, Ambrette (Musk Mallow)",       MiddleNotes="Magnolia, Violet, Sandalwood",                         BaseNotes="Ambergris, Cedar",                                               Description="A fresh, woody fragrance inspired by the Mojave Desert.",                                                                              Status="active" },
            new() { Id=20, Name="Blanche",             Brand="Byredo",                 Category="Niche",  Gender="Women",  Price=165, Stock=11, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.6686.2x.avif",  TopNotes="Aldehydes, Rose, Pink Pepper",            MiddleNotes="Peony, Violet, African Orange Flower",                 BaseNotes="Musk, Woody Notes, Sandalwood",                                  Description="A clean, aldehydic floral fragrance like fresh laundry.",                                                                              Status="active" },

            // ── Amouage ──────────────────────────────────────
            new() { Id=21, Name="Interlude Man",       Brand="Amouage",                Category="Luxury", Gender="Men",    Price=395, Stock=5,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.15294.2x.avif", TopNotes="Oregano, Pepper, Bergamot",               MiddleNotes="Incense, Amber, Labdanum, Opoponax",                   BaseNotes="Leather, Agarwood (Oud), Patchouli, Sandalwood",                 Description="A bold, smoky fragrance with oregano and leather.",                                                                                    Status="active" },
            new() { Id=22, Name="Reflection Man",      Brand="Amouage",                Category="Luxury", Gender="Men",    Price=395, Stock=6,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.920.2x.avif",   TopNotes="Rosemary, Pink Pepper, Petitgrain",       MiddleNotes="Jasmine, Neroli, Orris Root, Ylang-Ylang",             BaseNotes="Sandalwood, Vetiver, Cedar, Patchouli",                          Description="A sophisticated floral-woody fragrance.",                                                                                              Status="active" },
            new() { Id=23, Name="Jubilation XXV",      Brand="Amouage",                Category="Luxury", Gender="Men",    Price=395, Stock=4,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.2366.2x.avif",  TopNotes="Blackberry, Olibanum, Orange, Coriander, Labdanum, Tarragon", MiddleNotes="Honey, Guaiac Wood, Cinnamon, Bay Leaf, Clove, Rose, Orchid", BaseNotes="Myrrh, Agarwood (Oud), Opoponax, Patchouli, Ambergris, Cedar, Musk", Description="A complex, fruity-woody fragrance with blackberry and myrrh.", Status="active" },
            new() { Id=24, Name="Epic Woman",          Brand="Amouage",                Category="Luxury", Gender="Women",  Price=395, Stock=8,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.6538.2x.avif",  TopNotes="Caraway, Pink Pepper, Cinnamon",          MiddleNotes="Rose, Geranium, Jasmine, Tea",                         BaseNotes="Agarwood (Oud), Incense, Patchouli, Guaiac Wood, Orris Root, Sandalwood, Amber, Vanilla, Musk", Description="A spicy-woody fragrance with rose and oud.", Status="active" },

            // ── Zoologist ────────────────────────────────────
            new() { Id=25, Name="Squid",               Brand="Zoologist",              Category="Niche",  Gender="Unisex", Price=200, Stock=7,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.56294.2x.avif", TopNotes="Incense, Pink Pepper, Salicylate",        MiddleNotes="Sea Salt, Ink, Opoponax",                              BaseNotes="Ambergris, Benzoin, Musk",                                       Description="An oceanic-amber fragrance with ink and ambergris.",                                                                                   Status="active" },
            new() { Id=26, Name="T-Rex",               Brand="Zoologist",              Category="Niche",  Gender="Unisex", Price=240, Stock=3,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.51353.2x.avif", TopNotes="Bay Leaf, Black Pepper, Bergamot, Pine, Neroli, Nutmeg", MiddleNotes="Champaca, Geranium, Jasmine, Osmanthus, Rose, Ylang-Ylang", BaseNotes="Cade Oil, Leather, Resins, Civet, Amber, Cedar, Frankincense, Patchouli, Sandalwood, Vanilla", Description="A fiery, metallic fragrance with smoke and leather.", Status="active" },
            new() { Id=27, Name="Bee",                 Brand="Zoologist",              Category="Niche",  Gender="Unisex", Price=240, Stock=6,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.58140.2x.avif", TopNotes="Orange, Ginger Syrup, Royal Jelly Accord", MiddleNotes="Broom, Heliotrope, Mimosa, Orange Blossom",           BaseNotes="Benzoin, Labdanum, Musks, Sandalwood, Vanilla",                  Description="A sweet, honey-forward fragrance with royal jelly.",                                                                                   Status="active" },
            new() { Id=28, Name="Moth",                Brand="Zoologist",              Category="Niche",  Gender="Unisex", Price=200, Stock=5,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.49270.2x.avif", TopNotes="Black Pepper, Clove, Cumin, Lemon, Nutmeg, Saffron", MiddleNotes="Heliotrope, Iris, Jasmine, Mimosa, Rose",             BaseNotes="Ambergris, Benzoin, Cypriol Oil, Guaiac Wood, Honey, Musk, Patchouli, Resins, Smoke", Description="A dark, powdery fragrance with smoke and honey.", Status="active" },

            // ── Initio ───────────────────────────────────────
            new() { Id=29, Name="Oud for Greatness",   Brand="Initio",                 Category="Niche",  Gender="Unisex", Price=250, Stock=12, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.53641.2x.avif", TopNotes="Saffron, Nutmeg, Lavender",               MiddleNotes="Agarwood (Oud)",                                       BaseNotes="Patchouli, Musk",                                                Description="A powerful oud-saffron fragrance with nutmeg and lavender.",                                                                           Status="active" },
            new() { Id=30, Name="Side Effect",         Brand="Initio",                 Category="Niche",  Gender="Unisex", Price=220, Stock=10, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.42260.2x.avif", TopNotes="Rum, Vanilla",                            MiddleNotes="Tobacco",                                              BaseNotes="Cinnamon",                                                       Description="A boozy, tobacco-vanilla fragrance with cinnamon.",                                                                                    Status="active" },
            new() { Id=31, Name="Musk Therapy",        Brand="Initio",                 Category="Niche",  Gender="Unisex", Price=230, Stock=9,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.66097.2x.avif", TopNotes="White Musk, Bergamot",                   MiddleNotes="Mandarin Orange, Magnolia",                            BaseNotes="Black Currant, Sandalwood, Pink Musk",                           Description="A clean, white musk fragrance with citrus and magnolia.",                                                                              Status="active" },
            new() { Id=32, Name="Rehab",               Brand="Initio",                 Category="Niche",  Gender="Unisex", Price=290, Stock=8,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.50351.2x.avif", TopNotes="Lavender, Bergamot",                     MiddleNotes="Vetiver, Patchouli, Cedar",                            BaseNotes="Sandalwood, Musk, Guaiac Wood",                                  Description="A fresh, lavender-based fragrance with vetiver and sandalwood.",                                                                       Status="active" },

            // ── Penhaligon's ─────────────────────────────────
            new() { Id=33, Name="Halfeti",             Brand="Penhaligon's",           Category="Luxury", Gender="Unisex", Price=245, Stock=11, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.31396.2x.avif", TopNotes="Cypress Leaf, Saffron, Cardamom, Artemisia, Bergamot, Grapefruit", MiddleNotes="Bulgarian Rose, Nutmeg, Jasmine", BaseNotes="Agarwood (Oud), Cedar, Leather, Sandalwood, Amber, Musk, Tonka Bean, Vanilla", Description="A dark, spicy-woody fragrance with rose and oud.", Status="active" },
            new() { Id=34, Name="The Tragedy of Lord George", Brand="Penhaligon's",   Category="Luxury", Gender="Men",    Price=275, Stock=6,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.40716.2x.avif", TopNotes="Brandy",                                  MiddleNotes="Woodsy Notes, Shaving Soap",                           BaseNotes="Amber, Tonka Bean",                                              Description="A sophisticated brandy-amber fragrance with shaving soap notes.",                                                                      Status="active" },
            new() { Id=35, Name="Endymion",            Brand="Penhaligon's",           Category="Luxury", Gender="Men",    Price=160, Stock=13, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.5674.2x.avif",  TopNotes="Lavender, Bergamot, Sage, Mandarin Orange", MiddleNotes="Coffee, Geranium",                                    BaseNotes="Sandalwood, Leather, Myrrh, Musk, Vetiver, Cardamom, Pepper, Olibanum", Description="A lavender-coffee fragrance with leather and sandalwood.", Status="active" },
            new() { Id=36, Name="The World According To Arthur", Brand="Penhaligon's", Category="Luxury", Gender="Men",   Price=275, Stock=5,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.71646.2x.avif", TopNotes="Vanilla, Grapefruit",                     MiddleNotes="Ambrette (Musk Mallow), Clary Sage",                   BaseNotes="Incense, Tonka Bean",                                            Description="A vanilla-incense fragrance with grapefruit and ambrette.",                                                                            Status="active" },

            // ── Parfums de Marly ─────────────────────────────
            new() { Id=37, Name="Layton",              Brand="Parfums de Marly",       Category="Luxury", Gender="Men",    Price=225, Stock=18, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.39314.2x.avif", TopNotes="Apple, Lavender, Bergamot, Mandarin Orange", MiddleNotes="Geranium, Violet, Jasmine",                           BaseNotes="Vanilla, Cardamom, Sandalwood, Pepper, Patchouli, Guaiac Wood", Description="A fresh, apple-lavender fragrance with vanilla and cardamom.", Status="active" },
            new() { Id=38, Name="Haltane",             Brand="Parfums de Marly",       Category="Luxury", Gender="Men",    Price=275, Stock=7,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.70776.2x.avif", TopNotes="Clary Sage, Lavender, Bergamot",          MiddleNotes="Saffron, Praline",                                     BaseNotes="Agarwood (Oud), Cedar",                                          Description="An oud-praline fragrance with clary sage and lavender.",                                                                               Status="active" },
            new() { Id=39, Name="Herod",               Brand="Parfums de Marly",       Category="Luxury", Gender="Men",    Price=225, Stock=14, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.16939.2x.avif", TopNotes="Cinnamon, Pepper",                        MiddleNotes="Tobacco Leaf, Incense, Osmanthus, Labdanum",           BaseNotes="Vanilla, Iso E Super, Musk, Cedar, Cypriol Oil, Vetiver",        Description="A warm, spicy-vanilla fragrance with cinnamon and tobacco.",                                                                           Status="active" },
            new() { Id=40, Name="Delina",              Brand="Parfums de Marly",       Category="Luxury", Gender="Women",  Price=285, Stock=16, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.43871.2x.avif", TopNotes="Rhubarb, Litchi, Bergamot, Nutmeg",      MiddleNotes="Turkish Rose, Peony, Petalia, Vanilla, Musk",          BaseNotes="Cashmeran, Haitian Vetiver, Cedar, Incense",                     Description="A vibrant floral-fruity fragrance with rose and lychee.",                                                                              Status="active" },

            // ── Nishane ──────────────────────────────────────
            new() { Id=41, Name="Hacivat",             Brand="Nishane",                Category="Niche",  Gender="Unisex", Price=280, Stock=11, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.44174.2x.avif", TopNotes="Pineapple, Grapefruit, Bergamot",         MiddleNotes="Cedar, Patchouli, Jasmine",                            BaseNotes="Oakmoss, Woody Notes",                                           Description="A fresh pineapple and cedar fragrance with oakmoss depth.",                                                                            Status="active" },
            new() { Id=42, Name="Ani",                 Brand="Nishane",                Category="Niche",  Gender="Unisex", Price=260, Stock=9,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.54785.2x.avif", TopNotes="Ginger, Bergamot, Pink Pepper, Green Notes", MiddleNotes="Cardamom, Black Currant, Turkish Rose",               BaseNotes="Vanilla, Benzoin, Sandalwood, Musk, Patchouli, Cedar, Ambergris", Description="A complex spicy-floral fragrance with rose and vanilla.",                                                                    Status="active" },
            new() { Id=43, Name="Wulong Cha",          Brand="Nishane",                Category="Niche",  Gender="Unisex", Price=260, Stock=8,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.30567.2x.avif", TopNotes="Bergamot, Orange, Mandarin Orange, Litchi", MiddleNotes="Tea, Nutmeg",                                         BaseNotes="Fig, Musk",                                                      Description="A delicate tea fragrance with citrus and fig.",                                                                                        Status="active" },
            new() { Id=44, Name="Fan Your Flames",     Brand="Nishane",                Category="Niche",  Gender="Unisex", Price=260, Stock=7,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.37603.2x.avif", TopNotes="Coconut, Rum",                            MiddleNotes="Tobacco, Tonka Bean",                                  BaseNotes="Chinese Cedar, Oakmoss",                                         Description="A warm, tropical fragrance with coconut and tobacco.",                                                                                 Status="active" },

            // ── Nasomatto ────────────────────────────────────
            new() { Id=45, Name="Black Afgano",        Brand="Nasomatto",              Category="Niche",  Gender="Unisex", Price=130, Stock=9,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.6472.2x.avif",  TopNotes="Cannabis, Green Notes",                  MiddleNotes="Resins, Woodsy Notes, Tobacco, Coffee",                BaseNotes="Agarwood (Oud), Incense",                                        Description="A dark, resinous fragrance with oud and incense.",                                                                                     Status="active" },
            new() { Id=46, Name="Baraonda",            Brand="Nasomatto",              Category="Niche",  Gender="Unisex", Price=130, Stock=7,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.40200.2x.avif", TopNotes="Whiskey",                                 MiddleNotes="Rose",                                                 BaseNotes="Woody Notes, Musk, Ambrette (Musk Mallow), Ambroxan",           Description="A boozy, rose-tinged fragrance with woody musk.",                                                                                      Status="active" },
            new() { Id=47, Name="Pardon",              Brand="Nasomatto",              Category="Niche",  Gender="Men",    Price=130, Stock=6,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.12130.2x.avif", TopNotes="Magnolia, Flowers",                       MiddleNotes="Dark Chocolate, Tonka Bean, Cinnamon",                 BaseNotes="Agarwood (Oud), Sandalwood",                                     Description="A floral-chocolate fragrance with oud warmth.",                                                                                        Status="active" },
            new() { Id=48, Name="Duro",                Brand="Nasomatto",              Category="Niche",  Gender="Men",    Price=130, Stock=5,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.4291.2x.avif",  TopNotes="Leather",                                MiddleNotes="Spices",                                               BaseNotes="Woody Notes",                                                    Description="A bold leather and woody spice fragrance.",                                                                                            Status="active" },

            // ── Etat Libre d'Orange ──────────────────────────
            new() { Id=49, Name="You Or Someone Like You", Brand="Etat Libre d'Orange", Category="Niche", Gender="Unisex", Price=105, Stock=12, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.43531.2x.avif", TopNotes="Mint, Grapefruit, Bergamot, Anise",       MiddleNotes="Green Notes, Cassis, Rose, Hedione",                   BaseNotes="White Musk",                                                     Description="A fresh, green-aquatic fragrance with mint and white musk.",                                                                           Status="active" },
            new() { Id=50, Name="Fat Electrician",     Brand="Etat Libre d'Orange",    Category="Niche",  Gender="Unisex", Price=105, Stock=10, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.7139.2x.avif",  TopNotes="Vetiver, Whipped Cream",                 MiddleNotes="Myrrh, Olive Leaf",                                    BaseNotes="Vanilla, Opoponax, Chestnut",                                    Description="A creamy vetiver fragrance with myrrh and vanilla.",                                                                                   Status="active" },
            new() { Id=51, Name="Secretions Magnifiques", Brand="Etat Libre d'Orange", Category="Niche", Gender="Unisex", Price=105, Stock=8,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.4523.2x.avif",  TopNotes="Seaweed, Iodine",                         MiddleNotes="Milk, Adrenaline, Blood",                              BaseNotes="Iris, Coconut, Opoponax",                                        Description="A radically avant-garde fragrance — not for the faint-hearted.",                                                                       Status="active" },
            new() { Id=52, Name="Remarkable People",   Brand="Etat Libre d'Orange",    Category="Niche",  Gender="Unisex", Price=105, Stock=14, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.30504.2x.avif", TopNotes="Champagne, Grapefruit, Cardamom",         MiddleNotes="Curry Tree, Black Pepper, Jasmine",                    BaseNotes="Sandalwood, Lorenox, Labdanum",                                  Description="A spicy, effervescent fragrance with cardamom and jasmine.",                                                                           Status="active" },

            // ── Xerjoff ──────────────────────────────────────
            new() { Id=53, Name="Naxos",               Brand="Xerjoff",                Category="Luxury", Gender="Men",    Price=200, Stock=9,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.30529.2x.avif", TopNotes="Lavender, Bergamot, Lemon",               MiddleNotes="Honey, Cinnamon, Cashmeran, Jasmine Sambac",           BaseNotes="Tobacco Leaf, Vanilla, Tonka Bean",                              Description="A honeyed lavender fragrance with tobacco and vanilla.",                                                                               Status="active" },
            new() { Id=54, Name="Alexandria II",       Brand="Xerjoff",                Category="Luxury", Gender="Unisex", Price=266, Stock=7,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.17786.2x.avif", TopNotes="Palisander Rosewood, Lavender, Cinnamon, Apple", MiddleNotes="Rose, Cedar, Lily-of-the-Valley",                    BaseNotes="Agarwood (Oud), Sandalwood, Vanilla, Amber, Musk",               Description="A rich, oriental fragrance with oud, rose, and cedar.",                                                                                Status="active" },
            new() { Id=55, Name="Erba Pura",           Brand="Xerjoff",                Category="Luxury", Gender="Unisex", Price=135, Stock=15, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.55157.2x.avif", TopNotes="Sicilian Orange, Calabrian Bergamot, Sicilian Lemon", MiddleNotes="Fruits",                                            BaseNotes="White Musk, Madagascar Vanilla, Amber",                          Description="A sunny, citrus-vanilla fragrance bursting with Mediterranean warmth.",                                                                Status="active" },
            new() { Id=56, Name="Renaissance",         Brand="Xerjoff",                Category="Luxury", Gender="Unisex", Price=200, Stock=8,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.12126.2x.avif", TopNotes="Amalfi Lemon, Tangerine, Bergamot, Petitgrain", MiddleNotes="Mint, Lily-of-the-Valley, Rose",                     BaseNotes="Musk, Amber, Virginia Cedar, Patchouli",                         Description="A fresh citrus fragrance with floral heart and woody base.",                                                                           Status="active" },

            // ── Tom Ford ─────────────────────────────────────
            new() { Id=57, Name="Eau d'Ombré Leather", Brand="Tom Ford",               Category="Luxury", Gender="Men",    Price=140, Stock=12, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.95389.2x.avif", TopNotes="Cardamom, Ginger, Coriander",             MiddleNotes="Vanilla, Leather",                                     BaseNotes="Ambrofix",                                                       Description="A sleek, modern leather fragrance with spiced warmth.",                                                                                Status="active" },
            new() { Id=58, Name="Grey Vetiver",        Brand="Tom Ford",               Category="Luxury", Gender="Men",    Price=165, Stock=10, Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.6697.2x.avif",  TopNotes="Grapefruit, Orange Blossom, Sage",        MiddleNotes="Nutmeg, Orris Root, Pimento",                          BaseNotes="Vetiver, Oakmoss, Amber",                                        Description="A sophisticated vetiver fragrance with citrus and oakmoss.",                                                                           Status="active" },
            new() { Id=59, Name="Costa Azzurra",       Brand="Tom Ford",               Category="Luxury", Gender="Unisex", Price=165, Stock=9,  Image="https://fimgs.net/mdimg/perfume-thumbs/dark-375x500.64617.2x.avif", TopNotes="Driftwood, Seaweed, Agarwood",            MiddleNotes="Cypress, Lemon, Yellow Mandarin, Lavender",            BaseNotes="Incense, Vetiver, Oak",                                          Description="A coastal, woody fragrance inspired by the Italian Riviera.",                                                                          Status="active" },
        };

        // Seed volumes for each product based on Parfumes.txt
        var volumes = new List<ProductVolume>();
        int volId = 1;

        void AddVolumes(int productId, (string size, decimal price)[] sizes)
        {
            foreach (var (size, price) in sizes)
                volumes.Add(new ProductVolume { Id = volId++, ProductId = productId, Size = size, Price = price });
        }

        // Orto Parisi (all 50ml only at $155)
        foreach (var id in new[] { 1, 2, 3, 4 })
            AddVolumes(id, [("50ml", 155)]);

        // Creed
        AddVolumes(5,  [("30ml", 170), ("50ml", 235), ("100ml", 330)]); // Aventus
        AddVolumes(6,  [("50ml", 220), ("100ml", 320)]);                  // Green Irish Tweed
        AddVolumes(7,  [("30ml", 165), ("50ml", 220), ("100ml", 320)]); // Silver Mountain Water
        AddVolumes(8,  [("50ml", 220), ("100ml", 320)]);                  // Millesime Imperial

        // Kilian
        AddVolumes(9,  [("50ml", 275), ("100ml", 415)]);
        AddVolumes(10, [("50ml", 295), ("100ml", 450)]);
        AddVolumes(11, [("50ml", 295), ("100ml", 450)]);
        AddVolumes(12, [("50ml", 295), ("100ml", 450)]);

        // MFK
        AddVolumes(13, [("35ml", 245), ("70ml", 375)]);
        AddVolumes(14, [("35ml", 135), ("70ml", 205)]);
        AddVolumes(15, [("35ml", 165), ("70ml", 255)]);
        AddVolumes(16, [("35ml", 135), ("70ml", 205)]);

        // Byredo
        foreach (var id in new[] { 17, 18, 19, 20 })
            AddVolumes(id, [("50ml", 165), ("100ml", 240)]);

        // Amouage (all 100ml at $395)
        foreach (var id in new[] { 21, 22, 23, 24 })
            AddVolumes(id, [("100ml", 395)]);

        // Zoologist (60ml)
        AddVolumes(25, [("60ml", 200)]);
        AddVolumes(26, [("60ml", 240)]);
        AddVolumes(27, [("60ml", 240)]);
        AddVolumes(28, [("60ml", 200)]);

        // Initio
        AddVolumes(29, [("50ml", 250), ("90ml", 320)]);
        AddVolumes(30, [("50ml", 220), ("90ml", 280)]);
        AddVolumes(31, [("50ml", 230), ("90ml", 290)]);
        AddVolumes(32, [("90ml", 290)]);

        // Penhaligon's
        AddVolumes(33, [("100ml", 245)]);
        AddVolumes(34, [("75ml", 275)]);
        AddVolumes(35, [("30ml", 83), ("100ml", 160)]);
        AddVolumes(36, [("75ml", 275)]);

        // Parfums de Marly
        AddVolumes(37, [("75ml", 225), ("125ml", 290)]);
        AddVolumes(38, [("75ml", 275), ("125ml", 325)]);
        AddVolumes(39, [("75ml", 225), ("125ml", 290)]);
        AddVolumes(40, [("30ml", 180), ("75ml", 285)]);

        // Nishane
        AddVolumes(41, [("50ml", 280), ("100ml", 395)]);
        AddVolumes(42, [("50ml", 260), ("100ml", 375)]);
        AddVolumes(43, [("50ml", 260), ("100ml", 375)]);
        AddVolumes(44, [("50ml", 260), ("100ml", 375)]);

        // Nasomatto (all 30ml at $130)
        foreach (var id in new[] { 45, 46, 47, 48 })
            AddVolumes(id, [("30ml", 130)]);

        // Etat Libre d'Orange
        AddVolumes(49, [("50ml", 105), ("100ml", 160)]);
        AddVolumes(50, [("50ml", 105), ("100ml", 160)]);
        AddVolumes(51, [("50ml", 105)]);
        AddVolumes(52, [("50ml", 105), ("100ml", 160)]);

        // Xerjoff
        AddVolumes(53, [("100ml", 200)]);
        AddVolumes(54, [("50ml", 266), ("100ml", 446)]);
        AddVolumes(55, [("50ml", 135), ("100ml", 200)]);
        AddVolumes(56, [("100ml", 200)]);

        // Tom Ford
        AddVolumes(57, [("100ml", 140)]);
        AddVolumes(58, [("100ml", 165)]);
        AddVolumes(59, [("100ml", 165)]);

        var promoCodes = new List<PromoCode>
        {
            new() { Id = 1, Code = "SCENTE10", DiscountRate = 0.10m, IsActive = true, ExpiresAt = null },
            new() { Id = 2, Code = "SUMMER20", DiscountRate = 0.20m, IsActive = true, ExpiresAt = null },
            new() { Id = 3, Code = "VIP30",    DiscountRate = 0.30m, IsActive = true, ExpiresAt = null },
        };

        modelBuilder.Entity<Product>().HasData(products);
        modelBuilder.Entity<ProductVolume>().HasData(volumes);
        modelBuilder.Entity<PromoCode>().HasData(promoCodes);
    }
}
