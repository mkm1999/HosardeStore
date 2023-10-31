using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Context
{
    public interface IDataBaseContext
    {
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));

        DbSet<PrivacyEntity> privacyEntities { get; set; }
        DbSet<Category> categories { get; set; }
        DbSet<Product> products { get; set; }
        DbSet<ProductImages> productsImages { get; set; }
        DbSet<ProductProperties> ProductsProperties { get; set; }
        DbSet<Domain.Entities.Cart> Carts { get; set; }
        DbSet<CartItem> CartItems { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderDetail> OrderDetails { get; set; }
        DbSet<Address> Addresses { get; set; }
    }
}
