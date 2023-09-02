using VehicleRelocation.Api.Domain.Entities.Base;
using VehicleRelocation.Api.Domain.Interfaces;

namespace VehicleRelocation.Api.Domain.Entities
{
    public class Vehicle : BaseEntity<Guid>, IAuditable
    {
        public Guid VehicleModelTypeId { get; set; }
        public DateTime DateCreated { get ; set ; }
        public string CreatedBy { get ; set ; }
        public DateTime? DateUpdated { get ; set ; }
        public string UpdatedBy { get ; set ; }
        public VehicleModelType VehicleModelType { get; set; }
        public string License { get; set; }
    }
}

