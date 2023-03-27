using Application.DTO.Responses;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Configuration;
using ServiceLayer.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Leki.Queries
{
    public class LekListQuery : IRequest<PaginatedResponse<GetLekListResponse>>
    {
        public string? SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class GetLekListQueryHandle : IRequestHandler<LekListQuery, PaginatedResponse<GetLekListResponse>>
    {
        private readonly IConfiguration configuration;
        private readonly IHash hash;
        public GetLekListQueryHandle(IConfiguration config, IHash _hash)
        {
            configuration = config;
            hash = _hash;
        }

        public async Task<PaginatedResponse<GetLekListResponse>> Handle(LekListQuery req, CancellationToken cancellationToken)
        {
            var query = "SELECT l.ID_lek, l.Nazwa, SUM(ISNULL(ilosc, 0)) AS Ilosc, l.Jednostka_Miary, l.Producent FROM Lek l " +
                "LEFT join LeK_w_magazynie m on m.ID_lek = l.ID_lek " +
                "GROUP BY Nazwa, Jednostka_Miary, l.ID_lek, l.Producent " +
                "ORDER BY Nazwa";

            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("KlinikaDatabase"));
            await connection.OpenAsync();

            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
            var list = new List<GetLekListResponse>();

            while (reader.Read())
            {
                list.Add(new GetLekListResponse
                {
                    IdLek = hash.Encode(int.Parse(reader["ID_lek"].ToString())),
                    Nazwa = reader["Nazwa"].ToString(),
                    Ilosc = int.Parse(reader["Ilosc"].ToString()),
                    JednostkaMiary = reader["Jednostka_Miary"].ToString(),
                    Producent = reader["Producent"].ToString()
                });
            }

            await reader.CloseAsync();
            await connection.CloseAsync();

            var results = list
                .Where(
                x => req.SearchWord == null ||
                x.Nazwa.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.JednostkaMiary.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Producent.ToLower().Contains(req.SearchWord.ToLower())
                )
                .OrderBy(x => x.Nazwa);

            return new PaginatedResponse<GetLekListResponse>
            {
                Items = results
                    .Skip((req.Page - 1) * GlobalValues.PAGE_SIZE)
                    .Take(GlobalValues.PAGE_SIZE)
                    .ToList(),
                PageCount = (int)Math.Ceiling(results.Count() / (double)GlobalValues.PAGE_SIZE),
                PageIndex = req.Page
            };
        }
    }
}