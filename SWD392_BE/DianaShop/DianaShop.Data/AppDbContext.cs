using DianaShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } // Add your entities here
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductIngredient> ProductIngredients { get; set; }
        public DbSet<ProductSkinType> ProductSkinTypes { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<SkincareRoutine> Routines { get; set; }
        public DbSet<RoutineSkinType> RoutineSkinTypes { get; set; }
        public DbSet<SkinType> SkinTypes { get; set; }
        public DbSet<FormulationType> FormulationTypes { get; set; }
        public DbSet<Packaging> Packagings { get; set; }
        public DbSet<ProductBatch> ProductBatches { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherStorage> VoucherStorages { get; set; }
        public DbSet<Storage> Storages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductSkinType>()
                .HasKey(pst => new { pst.ProductId, pst.SkinTypeId });
            modelBuilder.Entity<ProductSkinType>()
                .HasOne(pst => pst.Product)
                .WithMany(p => p.ProductSkinTypes)
                .HasForeignKey(pst => pst.ProductId);
            modelBuilder.Entity<ProductSkinType>()
                .HasOne(pst => pst.SkinType)
                .WithMany(st => st.ProductSkinTypes)
                .HasForeignKey(pst => pst.SkinTypeId);

            modelBuilder.Entity<ProductImage>()
                .HasKey(pi => new { pi.ProductId, pi.ImageId });
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(pi => pi.ProductId);
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Image)
                .WithMany(i => i.ProductImages)
                .HasForeignKey(pi => pi.ImageId);

            modelBuilder.Entity<ProductIngredient>()
                .HasKey(pi => new { pi.ProductId, pi.IngredientId });
            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductIngredients)
                .HasForeignKey(pi => pi.ProductId);
            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Ingredient)
                .WithMany(i => i.ProductIngredients)
                .HasForeignKey(pi => pi.IngredientId);

            modelBuilder.Entity<ProductTag>()
                .HasKey(pt => new { pt.ProductId, pt.TagId });
            modelBuilder.Entity<ProductTag>()
                .HasOne(pt => pt.Product)
                .WithMany(p => p.ProductTags)
                .HasForeignKey(pt => pt.ProductId);
            modelBuilder.Entity<ProductTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.ProductTags)
                .HasForeignKey(pt => pt.TagId);

            modelBuilder.Entity<PostTag>()
                .HasKey(pt => new { pt.PostId, pt.TagId });
            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId);
            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId);

            modelBuilder.Entity<PostImage>()
                .HasKey(pi => new { pi.PostId, pi.ImageId });
            modelBuilder.Entity<PostImage>()
                .HasOne(pi => pi.Post)
                .WithMany(p => p.PostImages)
                .HasForeignKey(pi => pi.PostId);
            modelBuilder.Entity<PostImage>()
                .HasOne(pi => pi.Image)
                .WithMany(i => i.PostImages)
                .HasForeignKey(pi => pi.ImageId);

            modelBuilder.Entity<RoutineSkinType>()
                .HasKey(rst => new { rst.RoutineId, rst.SkinTypeId });
            modelBuilder.Entity<RoutineSkinType>()
                .HasOne(rst => rst.Routine)
                .WithMany(r => r.RoutineSkinTypes)
                .HasForeignKey(rst => rst.RoutineId);
            modelBuilder.Entity<RoutineSkinType>()
                .HasOne(rst => rst.SkinType)
                .WithMany(st => st.RoutineSkinTypes)
                .HasForeignKey(rst => rst.SkinTypeId);

            modelBuilder.Entity<Storage>()
                .HasKey(s => new { s.VoucherId, s.VoucherStorageId });
            modelBuilder.Entity<Storage>()
                .HasOne(s => s.Voucher)
                .WithMany(v => v.Storages)
                .HasForeignKey(s => s.VoucherId);
            modelBuilder.Entity<Storage>()
                .HasOne(s => s.VoucherStorage)
                .WithMany(vs => vs.Storages)
                .HasForeignKey(s => s.VoucherStorageId);

            //SeedData
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Manager" },
                new Role { Id = 2, Name = "Staff" },
                new Role { Id = 3, Name = "Customer" }
                );

        }
    }
}
