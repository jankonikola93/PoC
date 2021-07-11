using System;
using System.Linq;
using GraphQL.NetCore.Api.Context;
using GraphQL.NetCore.Api.Model;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.NetCore.Api.Schema.Queries
{
    public class AuthorQueries : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }
            descriptor.Name(OperationTypeNames.Query);
            descriptor
                .Field("authors")
                .ResolveWith<AuthorQueryResolver>(x => x.GetAuthors(default!));
        }

        private class AuthorQueryResolver
        {
            public IQueryable<Author> GetAuthors([Service] BookstoreDbContext context) =>
            context.Authors.Include(x => x.Books).AsNoTracking();
        }
    }
}