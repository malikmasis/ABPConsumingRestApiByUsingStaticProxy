using Acme.BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Books
{
    //[Authorize(BookStorePermissions.Books.Default)]
    public class BookAppService : ApplicationService, IBookAppService
    {
        public async Task<PagedResultDto<BookDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var bookDtos = new List<BookDto>()
            {
                new BookDto(){ Name = "Anna Karenina", AuthorName ="Tolstoy", Price = 50},
                new BookDto(){ Name = "Crime and Punishment", AuthorName ="Dostoevsky", Price = 60},
                new BookDto(){ Name = "Mother", AuthorName ="Gorki", Price = 70}
            };
            return new PagedResultDto<BookDto>(
               3,
               bookDtos
           );
        }

        //public Task<List<BookDto>> GetListAsync()
        //{
        //    return Task.FromResult(new List<BookDto>()
        //    {
        //        new BookDto(){ Name = "Anna Karenina", AuthorName ="Tolstoy", Price = 50},
        //        new BookDto(){ Name = "Crime and Punishment", AuthorName ="Dostoevsky", Price = 60},
        //        new BookDto(){ Name = "Mother", AuthorName ="Gorki", Price = 70}
        //    });
        //}
    }
}
