using System;
using GraphQL.NetCore.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.NetCore.Api.Context
{
    public class BookstoreDbContext : DbContext
    {
        public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Seed();
            base.OnModelCreating(modelBuilder);
        }
    }
}