using System;
using Microsoft.EntityFrameworkCore;

namespace ShoppingListApi
{
    public class ShoppingListContext : DbContext
    {
        public ShoppingListContext(DbContextOptions<ShoppingListContext> options)
            : base(options)
        {
        }

        public DbSet<ShoppingListItem> ShoppingListItems { get; set; }

    }
}
