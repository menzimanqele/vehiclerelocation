using VehicleRelocation.Api.Domain.Entities;
using System.Data.SqlClient;
using VehicleRelocation.Api.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Options;
using VehicleRelocation.Api.Domain;

namespace VehicleRelocation.Api.Infastructure.Repositories
{
    public class ManufactureRepository : Repository<Manufacture, Guid, SqlConnection>, IManufactureRepository //IRepository<Manufacture, Guid, SqlConnection>
    {
        public ManufactureRepository(IOptions<DatabaseConfig> databaseConfigOptions) : base(databaseConfigOptions)
        {
        }
    }
}

