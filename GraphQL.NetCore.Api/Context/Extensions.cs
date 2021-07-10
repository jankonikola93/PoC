using System;
using GraphQL.NetCore.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.NetCore.Api.Context
{
    public static class Extensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<Author>().HasData(
                new Author
                    {
                        Id = 1,
                        FirstName = "Eric",
                        LastName = "Evans",
                    },
                    new Author
                    {
                        Id = 2,
                        FirstName = "Robert",
                        LastName = "Martin",
                    },
                    new Author
                    {
                        Id = 3,
                        FirstName = "Martin",
                        LastName = "Fowler",
                    }
            );
            modelBuilder.Entity<Book>().HasData(
                new Book 
                {
                    Id = 1,
                    Title = "Domain-driven design",
                    AuthorId = 1,
                },
                new Book 
                {
                    Id = 2,
                    Title = "Clean Architecture",
                    AuthorId = 2,
                },
                new Book 
                {
                    Id = 3,
                    Title = "Refactoring",
                    AuthorId = 3,
                }
            );
        }
    }
}