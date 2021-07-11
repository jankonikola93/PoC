using System;
using GraphQL.NetCore.Api.Model;
using HotChocolate.Types;

namespace GraphQL.NetCore.Api.Schema.ObjectTypes
{
    public class BookType : ObjectType<Book>
    {
        protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            descriptor
                .Field(x => x.Id)
                .Type<NonNullType<IdType>>()
                .Description("Book's identifier");
            descriptor
                .Field(x => x.Title)
                .Type<NonNullType<StringType>>()
                .Description("Book's title");
            descriptor
                .Field(x => x.Author)
                .Type<NonNullType<AuthorType>>()
                .Description("Book's author");
            descriptor.Ignore(x => x.AuthorId);
        }
    }
}