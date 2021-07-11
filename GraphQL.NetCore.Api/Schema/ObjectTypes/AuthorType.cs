using System;
using GraphQL.NetCore.Api.Model;
using HotChocolate.Types;

namespace GraphQL.NetCore.Api.Schema.ObjectTypes
{
    public class AuthorType : ObjectType<Author>
    {
        protected override void Configure(IObjectTypeDescriptor<Author> descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            descriptor
                .Field(x => x.Id)
                .Type<NonNullType<IdType>>()
                .Description("Author's identifier");
            descriptor
                .Field(x => x.FirstName)
                .Type<NonNullType<StringType>>()
                .Description("Author's first name");
            descriptor
                .Field(x => x.LastName)
                .Type<NonNullType<StringType>>()
                .Description("Author's last name");
            descriptor
                .Field(x => x.Books)
                .Type<ListType<BookType>>()
                .Description("List of books written by this author");
        }
    }
}