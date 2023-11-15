using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Roleta.Aplicacao.Extensions
{
    public static class Pagination
    {
        public static void AddPagination(this HttpResponse response, string nameHeader,
            int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var pagination = new PaginationHeader(currentPage,
                                                  itemsPerPage,
                                                  totalItems,
                                                  totalPages);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            response.Headers.Add(nameHeader, JsonSerializer.Serialize(pagination, options));
            response.Headers.Add("Access-Control-Expose-Headers", nameHeader);
        }
    }
}
