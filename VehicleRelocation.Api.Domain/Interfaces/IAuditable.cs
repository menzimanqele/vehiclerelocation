namespace VehicleRelocation.Api.Domain.Interfaces
{
    public interface IAuditable
	{
		public DateTime DateCreated { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? DateUpdated { get; set; }
		public string UpdatedBy { get; set; }
	}
}

