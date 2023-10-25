using System;
namespace VehicleRelocation.Api.Domain
{
	public class DatabaseConfig
	{
		public required string ConnnectionString { get; set; }
        public required string ConnectionType { get; set; }
	}
}

