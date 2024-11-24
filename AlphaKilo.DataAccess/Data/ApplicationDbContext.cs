using AlphaKilo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlphaKilo.DataAccess.Data {
    public class ApplicationDbContext : IdentityDbContext<IdentityUser> {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers {  get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 2 },
                new Category { Id = 2, Name = "Sci-Fi", DisplayOrder = 4 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 },
                new Category { Id = 4, Name = "Fantasy", DisplayOrder = 1 }
            );
            modelBuilder.Entity<Product>().HasData(
                new Product { 
                    Id = 1,
                    Title = "The Fellowship of the Ring",
                    Author = "J.R.R. Tolkien",
                    Description = "Continuing the story begun in The Hobbit, this is the first part of Tolkien's epic masterpiece," +
                    " The Lord of the Rings, featuring a striking black cover based on Tolkien's own design, the definitive text, " +
                    "and a detailed map of Middle-earth. Sauron, the Dark Lord, has gathered to him all the Rings of Power " +
                    "- the means by which he intends to rule Middle-earth. All he lacks in his plans for dominion is the One Ring - " +
                    "the ring that rules them all - which has fallen into the hands of the hobbit, Bilbo Baggins. " +
                    "In a sleepy village in the Shire, young Frodo Baggins finds himself faced with an immense task," +
                    " as his elderly cousin Bilbo entrusts the Ring to his care. Frodo must leave his home and make a perilous journey" +
                    " across Middle-earth to the Cracks of Doom, there to destroy the Ring and foil the Dark Lord in his evil purpose." +
                    " Part of a set of three paperbacks, this popular edition is once again available in its classic black livery " +
                    "designed by Tolkien himself.",
                    ISBN = "9780261102354",
                    ListPrice = 11.25,
                    Price50 = 9.75,
                    Price100 = 8,
                    CategoryId = 4
                },
                new Product {
                    Id = 2,
                    Title = "The Two Towers",
                    Author = "J.R.R. Tolkien",
                    Description = "Building on the story begun in The Hobbit and The Fellowship of the Ring this is the second part of " +
                    "Tolkien's epic masterpiece, The Lord of the Rings, featuring a striking black cover based on Tolkien's own design, " +
                    "the definitive text, and a detailed map of Middle-earth. Frodo and the Companions of the Ring have been beset by " +
                    "danger during their quest to prevent the Ruling Ring from falling into the hands of the Dark Lord by destroying it " +
                    "in the Cracks of Doom. They have lost the wizard, Gandalf, in the battle with an evil spirit in the Mines of Moria; " +
                    "and at the Falls of Rauros, Boromir, seduced by the power of the Ring, tried to seize it by force. While Frodo and " +
                    "Sam made their escape the rest of the company were attacked by Orcs. Now they continue their journey alone down the " +
                    "great River Anduin - alone, that is, save for the mysterious creeping figure that follows wherever they go.",
                    ISBN = "9780261102361",
                    ListPrice = 11.25,
                    Price50 = 9.75,
                    Price100 = 8,
                    CategoryId = 4
                },
                new Product {
                    Id = 3,
                    Title = "The Return of the King",
                    Author = "J.R.R. Tolkien",
                    Description = "Concluding the story of The Hobbit, this is the final part of Tolkien's epic masterpiece, The Lord of " +
                    "the Rings, featuring a striking black cover based on Tolkien's own design, the definitive text, and a detailed map " +
                    "of Middle-earth. The armies of the Dark Lord Sauron are massing as his evil shadow spreads even wider. Men, Dwarves, " +
                    "Elves and Ents unite forces to do battle against the Dark. Meanwhile, Frodo and Sam struggle further into Mordor, " +
                    "guided by the treacherous creature Gollum, in their heroic quest to destroy the One Ring... JRR Tolkien's great work" +
                    " of imaginative fiction has been labelled both a heroic romance and a classic fantasy fiction. By turns comic and " +
                    "homely, epic and diabolic, the narrative moves through countless changes of scene and character in an imaginary world " +
                    "which is totally convincing in its detail. Tolkien created a vast new mythology in an invented world which has proved" +
                    " timeless in its appeal. Part of a set of three paperbacks, this popular edition is once again available in its classic" +
                    " black livery designed by Tolkien himself.",
                    ISBN = "9780261102378",
                    ListPrice = 11.25,
                    Price50 = 9.75,
                    Price100 = 8,
                    CategoryId = 4
                }
            );
            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, Name = "Google", PhoneNumber = "(650) 253-0000", StreetAddress = "1600 Amphitheatre Parkway", City = "Mountain View",  State="California", PostalCode = "94035",},
                new Company { Id = 2, Name = "Amazon", PhoneNumber = "(206) 266-1000", StreetAddress = "410 Terry Ave. North", City = "Seattle",  State="Washington", PostalCode = "98109", },
                new Company { Id = 3, Name = "Meta", PhoneNumber = "(650) 543-4800", StreetAddress = "1 Meta Way", City = "Menlo Park",  State="California", PostalCode = "94025",}
            );
        }

    }
}
