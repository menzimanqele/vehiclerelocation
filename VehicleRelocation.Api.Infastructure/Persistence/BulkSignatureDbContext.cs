using LinqToDB;

namespace VehicleRelocation.Api.Infastructure.Persistence;

public class BulkSignatureDbContext :LinqToDB.Data.DataConnection
{

    public BulkSignatureDbContext(DataOptions<BulkSignatureDbContext> options)
    :base(options.Options)
    {

    }
}