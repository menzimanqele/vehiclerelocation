using System;
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;

namespace VehicleRelocation.Api.Domain.Entities.Base
{
	public  abstract class BaseEntity<TKey>
	{
		
		[ExplicitKey]
		public virtual required TKey Id{ get; set; }
	//	public bool IsDeleted { get; set; }

	}
}
