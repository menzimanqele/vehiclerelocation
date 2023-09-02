using VehicleRelocation.Api.Domain.Entities.Base;
using VehicleRelocation.Api.Domain.Interfaces;

namespace VehicleRelocation.Api.Domain.Entities
{
    public class VehicleModel : BaseEntity<Guid>, IAuditable
    {
        public required string Description { get; set; }
        public DateTime DateCreated { get ; set ; }
        public string CreatedBy { get ; set ; }
        public DateTime? DateUpdated { get ; set ; }
        public string UpdatedBy { get ; set ; }
        public Manufacture Manufacture { get; set; }
    }
}

