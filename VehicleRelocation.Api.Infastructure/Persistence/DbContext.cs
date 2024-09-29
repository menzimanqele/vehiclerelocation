using LinqToDB;

namespace VehicleRelocation.Api.Infastructure;

public class DbContext : LinqToDB.Data.DataConnection
{
    public DbContext(DataOptions<DbContext> options)
        :base(options.Options)
    {
       
    }
}