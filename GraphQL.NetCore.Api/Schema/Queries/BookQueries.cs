using System;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.NetCore.Api.Context;
using GraphQL.NetCore.Api.Model;
using GraphQL.NetCore.Api.Schema.ObjectTypes;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.NetCore.Api.Schema.Queries
{
    public class BookQueries : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }
            
            descriptor.Name(OperationTypeNames.Query);
            descriptor
                .Field("books")
                .Type<ListType<BookType>>()
                .ResolveWith<BookResolvers>(x => x.GetBooks(default!));

            descriptor
                .Field("book")
                .Argument("id", x => x.Type<NonNullType<IntType>>())
                .Type<BookType>()
                .ResolveWith<BookResolvers>(x => x.GetBook(default!, default!));
        }

        private class BookResolvers
        {
            public IQueryable<Book> GetBooks([Service] BookstoreDbContext context) =>
            context.Books.Include(x => x.Author).AsNoTracking();

            public Task<Book> GetBook(int id, [Service] BookstoreDbContext context) =>
            context.Books.Include(x => x.Author).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);   
        }
    }
}