using Application.Interfaces.Context;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Context
{
    public class DataBaseContext : IdentityDbContext<User,IdentityRole<int>,int> , IDataBaseContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
                
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            builder.Entity<IdentityRole<int>>().HasData(
                    new IdentityRole<int>
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN",
                        Id = 1,
                    }
                    ,
                    new IdentityRole<int>
                    {
                        Name = "Operator",
                        NormalizedName = "OPERATOR",
                        Id = 2
                    }
                    ,
                    new IdentityRole<int>
                    {
                        Name = "Customer",
                        NormalizedName = "CUSTOMER",
                        Id = 3
                    }
                    ,
                    new IdentityRole<int>
                    {
                        Name = "Boss",
                        NormalizedName ="BOSS",
                        Id = 4
                    }
                );

            builder.Entity<PrivacyEntity>().HasData(
                    new PrivacyEntity
                    {
                        Id = 1,
                        PolicyText = "Text For Policy",
                        PrivacyText = "Text For Privacy"
                    }
                );
        }

        public DbSet<PrivacyEntity> privacyEntities { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<ProductImages> productsImages { get; set; }
        public DbSet<ProductProperties> ProductsProperties { get; set; }

    }
}
