using System.Data.SqlClient;
using LinqToDB.DataProvider.SqlServer;
using Microsoft.Extensions.Options;
using VehicleRelocation.Api.Domain;
using VehicleRelocation.Api.Domain.Entities;
using VehicleRelocation.Api.Domain.Interfaces.Repositories;
using VehicleRelocation.Api.Infastructure.Persistence;

namespace VehicleRelocation.Api.Infastructure.Repositories;

public class CategoryRepository :Repository<Category,Int32, SqlConnection>, ICategoryRepository
{
    private const string connectionName = "bookConnection";
    
    public CategoryRepository(IOptions<List<DatabaseConfig>> databaseConfigOptions) : base(databaseConfigOptions.Value?.Where(x=>x.Name == connectionName).FirstOrDefault())
    {
        var res = databaseConfigOptions;
    }
}