using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
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
    public class LekListQuery : IRequest<List<GetLekListResponse>>
    {

    }

    public class GetLekListQueryHandle : IRequestHandler<LekListQuery, List<GetLekListResponse>>
    {
        private readonly IConfiguration configuration;
        private readonly IHash hash;
        public GetLekListQueryHandle(IConfiguration config, IHash _hash)
        {
            configuration = config;
            hash = _hash;
        }

        public async Task<List<GetLekListResponse>> Handle(LekListQuery req, CancellationToken cancellationToken)
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

            return list;
        }
    }
}