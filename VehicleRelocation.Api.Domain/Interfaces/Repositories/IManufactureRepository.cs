using VehicleRelocation.Api.Domain.Entities;
using System.Data.SqlClient;

namespace VehicleRelocation.Api.Domain.Interfaces.Repositories
{
    public interface IManufactureRepository : IRepository<Manufacture,Guid, SqlConnection>,IRepository
	{
	}
}

