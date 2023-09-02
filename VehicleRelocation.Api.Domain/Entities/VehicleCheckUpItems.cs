using VehicleRelocation.Api.Domain.Interfaces;

namespace VehicleRelocation.Api.Domain.Entities
{
    public class VehicleCheckUpItems : IAuditable
	{
		public required Guid VehicleCheckUpId { get; set; }
		public required Guid VehicleId { get; set; }
		public required Guid VehicleCheckUpStatusUpId { get; set; }
		public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get ; set ; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get ; set ; }
    }
}

