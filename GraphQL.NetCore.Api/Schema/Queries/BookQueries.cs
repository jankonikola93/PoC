using System.Linq;
using GraphQL.NetCore.Api.Context;
using GraphQL.NetCore.Api.Model;
using HotChocolate;

namespace GraphQL.NetCore.Api.Schema.Queries
{
    public class BookQueries
    {
        public IQueryable<Book> GetBooks([Service] BookstoreDbContext context) =>
            context.Books;
    }
}