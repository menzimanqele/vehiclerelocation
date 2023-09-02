﻿using VehicleRelocation.Api.Domain.Entities.Base;

namespace VehicleRelocation.Api.Domain.Entities
{
    public class VehicleCheckUp : BaseEntity<Guid>
	{
		public string Name { get; set; }
		public string Description { get; set; }
	}
}

