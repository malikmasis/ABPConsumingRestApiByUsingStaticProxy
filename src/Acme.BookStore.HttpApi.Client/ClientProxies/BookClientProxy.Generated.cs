// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Acme.BookStore.Books;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Acme.BookStore.Books.ClientProxies;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IBookAppService), typeof(BookClientProxy))]
public partial class BookClientProxy : ClientProxyBase<IBookAppService>, IBookAppService
{
    public virtual async Task<List<BookDto>> GetListAsync()
    {
        return await RequestAsync<List<BookDto>>(nameof(GetListAsync));
    }
}