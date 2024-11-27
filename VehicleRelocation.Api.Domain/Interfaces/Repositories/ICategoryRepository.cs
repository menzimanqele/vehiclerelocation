using System.Data.SqlClient;
using VehicleRelocation.Api.Domain.Entities;

namespace VehicleRelocation.Api.Domain.Interfaces.Repositories;

public interface ICategoryRepository : IRepository<Category,Int32, SqlConnection>,IRepository
{
     
}
 