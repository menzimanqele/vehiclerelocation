using VehicleRelocation.Api.Domain.Entities;
using System.Data.SqlClient;
using VehicleRelocation.Api.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Options;
using VehicleRelocation.Api.Domain;

namespace VehicleRelocation.Api.Infastructure.Repositories
{
    public class ManufactureRepository : Repository<Manufacture, Guid, SqlConnection>, IManufactureRepository //IRepository<Manufacture, Guid, SqlConnection>
    {
        private const string connectionName = "Default";
        public ManufactureRepository(IOptions<List<DatabaseConfig>> databaseConfigOptions) : base(databaseConfigOptions.Value?.Where(x=>x.Name == connectionName).FirstOrDefault())
        {
        }
        
        public Task<Manufacture> GetByCode(string code)
        {
            throw new NotImplementedException();
        }
    }
}

