using System;
namespace VehicleRelocation.Api.Domain.Entities.Base
{
	public  abstract class BaseEntity<TKey>
	{
		public virtual required TKey Id{ get; set; }
		public bool IsDeleted { get; set; }

	}
}

