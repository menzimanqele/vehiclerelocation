using System;
using VehicleRelocation.Api.Domain.Entities.Base;

namespace VehicleRelocation.Api.Domain.Entities
{
	public class VehicleCheckStatus : BaseEntity<Guid>
	{
		public required string Status { get; set; }
	}
}

